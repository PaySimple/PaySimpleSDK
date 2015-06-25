#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance, Ethan Tipton
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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PaySimpleSdk.Helpers
{
    [ExcludeFromCodeCoverage]
    // Thanks Jon Skeet - http://stackoverflow.com/questions/255341/getting-key-of-value-of-a-generic-dictionary
    public class BiLookup<TFirst, TSecond> : IEnumerable<KeyValuePair<TFirst, TSecond>>
    {
        IDictionary<TFirst, TSecond> firstToSecond = new Dictionary<TFirst, TSecond>();
        IDictionary<TSecond, TFirst> secondToFirst = new Dictionary<TSecond, TFirst>();

        private static TFirst EmptyFirst = default(TFirst);
        private static TSecond EmptySecond = default(TSecond);

        public void Add(TFirst first, TSecond second)
        {
            TFirst firsts;
            TSecond seconds;
            if (!firstToSecond.TryGetValue(first, out seconds))
            {
                firstToSecond[first] = second;
            }
            if (!secondToFirst.TryGetValue(second, out firsts))
            {
                secondToFirst[second] = first;
            }
        }

        // Note potential ambiguity using indexers (e.g. mapping from int to int)
        // Hence the methods as well...
        public TSecond this[TFirst first]
        {
            get { return GetByFirst(first); }
        }

        public TFirst this[TSecond second]
        {
            get { return GetBySecond(second); }
        }

        public TSecond GetByFirst(TFirst first)
        {
            TSecond value;
            if (!firstToSecond.TryGetValue(first, out value))
            {
                return EmptySecond;
            }
            return value; // Create a copy for sanity
        }

        public TFirst GetBySecond(TSecond second)
        {
            TFirst value;
            if (!secondToFirst.TryGetValue(second, out value))
            {
                return EmptyFirst;
            }
            return value; // Create a copy for sanity
        }

        #region IEnumerable
        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            return firstToSecond.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
