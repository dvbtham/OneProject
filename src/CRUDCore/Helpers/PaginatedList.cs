using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCore.Helpers
{
    public class PaginatedList<T> 
    {
        public int Page { get; set; }

        public int Count
        {
            get
            {
                return (Items != null) ? Items.Count() : 0;
            }
        }

        public int TotalCount { get; set; }

        public int MaxPage { set; get; }

        public int TotalPages { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
