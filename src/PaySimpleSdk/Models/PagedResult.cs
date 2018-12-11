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
        public int TotalPages { get; set; }

        // PagedResult items will not always be IEnumerable.  Some PagesResult items will
        // be an object like PaymentScheduleList where the POCO is made up of two Enumerable
        // lists
        public T Items { get; set; }        
    }
    
    internal static class PagedResult
    {
        public static PagedResult<T2> ConvertToPagedResult<T1, T2>(Result<T1> results, T2 items)
            where T1 : class
            where T2 : class
        {         
            var pagedResult = new PagedResult<T2>
            {            
                Page = 1,
                TotalPages = 1,
                Items = items
            };

            if (results.ResultData.PagingDetails == null)
                return pagedResult;

            pagedResult.Page = results.ResultData.PagingDetails.Page;
            pagedResult.ItemsPerPage = results.ResultData.PagingDetails.ItemsPerPage;
            pagedResult.TotalItems = results.ResultData.PagingDetails.TotalItems;

            if (pagedResult.ItemsPerPage > 0)
                pagedResult.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(pagedResult.TotalItems) / Convert.ToDecimal(pagedResult.ItemsPerPage)));            

            return pagedResult;
        }

        public static PagedResult<T> ConvertToPagedResult<T>(Result<T> results)       
            where T : class
        {
            var pagedResult = new PagedResult<T>
            {
                Page = 1,
                TotalPages = 1,
                Items = results.Response
            };

            if (results.ResultData.PagingDetails == null)
                return pagedResult;

            pagedResult.Page = results.ResultData.PagingDetails.Page;
            pagedResult.ItemsPerPage = results.ResultData.PagingDetails.ItemsPerPage;
            pagedResult.TotalItems = results.ResultData.PagingDetails.TotalItems;

            if (pagedResult.ItemsPerPage > 0)            
                pagedResult.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(pagedResult.TotalItems ) / Convert.ToDecimal(pagedResult.ItemsPerPage)));            

            return pagedResult;
        }
    }
}