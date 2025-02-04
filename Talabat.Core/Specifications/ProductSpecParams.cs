using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxpageSize = 10;
        private int pageSize=5;
        private string search { get; set; }
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
  

        public int PageSize { get { return pageSize; }
            set{ pageSize = value > MaxpageSize ? MaxpageSize : value; } }

        public int PageIndex { get; set; } = 1;


        

        public string? Search {
            get { return search; }
            set { search = value.ToLower(); }
        }
    }
}
