using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductFiltrationForCountSpecification:BaseSpecification<Product>
    {
        public ProductFiltrationForCountSpecification(ProductSpecParams productspec) : base(
            p => (string.IsNullOrEmpty(productspec.Search) || p.Name.ToLower().Contains(productspec.Search)) &&
            (!productspec.BrandId.HasValue || p.ProductBrandId == productspec.BrandId) &&
            (!productspec.TypeId.HasValue || p.ProductTypeId == productspec.TypeId))
        {

        }

    }
}
