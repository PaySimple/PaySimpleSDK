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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace PaySimpleSdk.Models
{
    internal class Result<T>
        where T : class
    {
        [JsonProperty("Meta")]
        public Meta ResultData { get; set; }
        [JsonProperty("Response")]
        public T Response { get; set; }
    }

    public class Meta
    {
        [JsonProperty("Errors")]
        public Error Errors { get; set; }
        [JsonProperty("HttpStatusCode")]
        public int HttpStatusCode { get; set; }
        [JsonProperty("PagingDetails")]
        public PagingDetail PagingDetails { get; set; }
		public override string ToString()
		{
			return $"StatusCode: {HttpStatusCode} {Errors}";
		}
    }

    public class Error
    {
        [JsonProperty("ErrorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("ErrorMessages")]
        public IEnumerable<ErrorMessage> ErrorMessages { get; set; }
        [JsonProperty("TraceCode")]
        public string TraceCode { get; set; }

	    public override string ToString()
	    {
		    return $"ErrorCode: {ErrorCode} TraceCode: {TraceCode} ErrorMessages: {string.Join(", ", ErrorMessages)}";
	    }
    }

    public class ErrorMessage
    {
        [JsonProperty("Field")]
        public string Field { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }

	    public override string ToString()
	    {
		    return $"{Field} - {Message}";
	    }
    }

    public class PagingDetail
    {
        [JsonProperty("TotalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("Page")]
        public int Page { get; set; }
        [JsonProperty("ItemsPerPage")]
        public int ItemsPerPage { get; set; }
    }
}