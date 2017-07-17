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
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace PaySimpleSdk.Helpers
{
	[ExcludeFromCodeCoverage]
	internal class Serialization : ISerialization
	{
		public string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		}

		public T Deserialize<T>(string obj) where T : class
		{
			return JsonConvert.DeserializeObject<T>(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });//, NullValueHandling = NullValueHandling.Ignore });
		}

		public T Deserialize<T>(Stream stream) where T : class
		{
			var serializer = new JsonSerializer
			{
				TypeNameHandling = TypeNameHandling.Objects
			};

			var jsonTextReader = new JsonTextReader(new StreamReader(stream));

			var result = serializer.Deserialize<T>(jsonTextReader);
			return result;
		}
	}
}