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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySimpleSdk.Models
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }        
    }
    
    internal static class PagedResult
    {
        public static PagedResult<T> ConvertToPagedResult<T>(Result<IEnumerable<T>> results)    
            where T : class
        {

            var pagedResult = new PagedResult<T>
            {
                Page = 1,
                ItemsPerPage = results.Response.Count(),
                TotalItems = results.Response.Count(),
                Items = results.Response
            };

            if (results.ResultData.PagingDetails == null)
                return pagedResult;

            pagedResult.Page = results.ResultData.PagingDetails.Page;
            pagedResult.ItemsPerPage = results.ResultData.PagingDetails.ItemsPerPage;
            pagedResult.TotalItems = results.ResultData.PagingDetails.TotalItems;

            return pagedResult;
        }
    }
}