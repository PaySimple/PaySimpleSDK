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
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PaySimpleSdk.Helpers
{
    [ExcludeFromCodeCoverage]
    internal class TypeEnumConverter<T1, T2> : JsonConverter
        where T1 : struct
        where T2 : BiLookup<T1, string>, new()
    {
        private BiLookup<T1, string> lookup;

        public TypeEnumConverter()
        {
            this.lookup = EnumStrings.GetEnumMappings<T1>() as BiLookup<T1, string>;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Serializing this is of type List<T1>
            if (value.GetType() == typeof(List<T1>))
            {
                IEnumerable<T1> enumValues = value as IEnumerable<T1>;
                List<string> newValues = new List<string>();

                foreach (var e in enumValues)
                    newValues.Add(lookup[e]);

                serializer.Serialize(writer, newValues);
            }
            else
            {
                // If this is not a List, then write the value out
                T1 enumValue = (T1)value;
                var retVal = lookup[enumValue];

                if (!string.IsNullOrWhiteSpace(retVal))
                    writer.WriteValue(retVal);
                else
                    writer.WriteValue((string)null);

            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Deserializing, this is of type IEnumerable<T1>
            if (objectType == typeof(IEnumerable<T1>))
            {
                var jsonObj = serializer.Deserialize<List<string>>(reader);
                List<T1> newValues = new List<T1>();

                foreach (string e in jsonObj)
                    newValues.Add(lookup[e]);

                return newValues;
            }
            else
            {
                // If this is not a Enumeration, then write the value out                
                var enumString = (string)reader.Value ?? "";
                return lookup[enumString];
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
