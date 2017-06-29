#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// The most recent version of this license can be found at: http://opensource.org/licenses/MIT
#endregion

using PaySimpleSdk.Exceptions;
using PaySimpleSdk.Models;
using System;
using System.Net;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace PaySimpleSdk.Helpers
{
    [ExcludeFromCodeCoverage]
    internal class WebServiceRequest : IWebServiceRequest
    {
        private readonly ISerialization serialization;
        private readonly ISignatureGenerator signatureGenerator;
        private readonly int retryCount;
        private static HttpClient httpClient = new HttpClient();

        public WebServiceRequest(ISerialization serialization, ISignatureGenerator signatureGenerator, int retryCount)
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;

            this.serialization = serialization;
            this.signatureGenerator = signatureGenerator;
            this.retryCount = retryCount;
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            return await MakeRequestAsync(requestMessage);
        }

        public async Task<T> GetDeserializedAsync<T>(Uri requestUri) where T : class
        {
            var result = await GetAsync(requestUri);
            using (var content = await result.Content.ReadAsStreamAsync())
            {
                return serialization.Deserialize<T>(content);
            }
        }

        public async Task<HttpResponseMessage> PutAsync(Uri requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
            return await MakeRequestAsync(requestMessage);
        }

        public async Task<T> PutAsync<T>(Uri requestUri) where T : class
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
            var result = await MakeRequestAsync(requestMessage);
            using (var content = await result.Content.ReadAsStreamAsync())
            {
                return serialization.Deserialize<T>(content);
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(Uri requestUri, T payload) where T : class
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
            var content = new StringContent(serialization.Serialize(payload));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            requestMessage.Content = content;

            return await MakeRequestAsync(requestMessage);
        }

        public async Task<TResponse> PutDeserializedAsync<TRequest, TResponse>(Uri requestUri, TRequest payload)
            where TRequest : class
            where TResponse : class
        {
            var result = await PutAsync<TRequest>(requestUri, payload);
            using (var content = await result.Content.ReadAsStreamAsync())
            {
                return serialization.Deserialize<TResponse>(content);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);

            return await MakeRequestAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(Uri requestUri, T payload) where T : class
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var content = new StringContent(serialization.Serialize(payload));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            requestMessage.Content = content;

            return await MakeRequestAsync(requestMessage);
        }

        public async Task<TResponse> PostDeserializedAsync<TRequest, TResponse>(Uri requestUri, TRequest payload)
            where TRequest : class
            where TResponse : class
        {
            var result = await PostAsync<TRequest>(requestUri, payload);
            using (var content = await result.Content.ReadAsStreamAsync())
            {
                return serialization.Deserialize<TResponse>(content);
            }
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(HttpRequestMessage request)
        {
            var exceptions = new List<Exception>();

            // Minor optimization: skip the loop entirely if we don't need it
            if (retryCount <= 1)
                return await DoRequestAsync(request);

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    // Wait 1 second between additional attempts
                    if (retry > 0)
                        await Task.Delay(1000);

                    return await DoRequestAsync(request);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
        private async Task<HttpResponseMessage> DoRequestAsync(HttpRequestMessage request)
        {
            var authToken = signatureGenerator.GenerateSignature();
            request.Headers.Add("Authorization", authToken);
            request.Headers.Add("Accept", "application/json");
            // Unfortunately the PS Api is incorrectly using the 204 (NO CONTENT) status code. It is returning content with a 204 which causes protocol violations
            // Closing the connection allows this to work, but no more connection reuse for now
            request.Headers.Add("Connection", "close");


            var result = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
                return result;

            using (var content = await result.Content.ReadAsStreamAsync())
            {
                try
                {
                    var errors = serialization.Deserialize<ErrorResult>(content);
                    throw new PaySimpleEndpointException(errors, result.StatusCode);
                }
                catch (Exception e) when (!(e is PaySimpleEndpointException))
                {
                    throw new PaySimpleEndpointException($"Error deserializing response: {GetResponseString(content)}", e);
                }
            }
        }

        private static string GetResponseString(Stream response)
        {
            response.Position = 0;
            StreamReader reader = new StreamReader(response);
            var text = reader.ReadToEnd();

            return text;
        }
    }
}