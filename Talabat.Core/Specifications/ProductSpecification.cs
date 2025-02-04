using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecification:BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams productspec):base(
            p=> (string.IsNullOrEmpty(productspec.Search) ||
            p.Name.ToLower().Contains(productspec.Search)) &&
            (!productspec.BrandId.HasValue||p.ProductBrandId== productspec.BrandId) && 
            (!productspec.TypeId.HasValue || p.ProductTypeId == productspec.TypeId))
        {
            Includes.Add(p=>p.ProductBrand);
            Includes.Add(p=>p.ProductType);

            if (!string.IsNullOrEmpty(productspec.Sort))
            {
                switch (productspec.Sort)
                {
                    case "PriceSort":
                        {
                            AddOrderBy(p => p.Price);
                            break;
                        }
                    
                    case "PriceDesc":
                        {
                            AddOrderByDescending(p => p.Price);
                            break;
                        }
                     default:
                        {
                            AddOrderBy(p => p.Name);
                            break;
                        }
                }
            }
            ApplyPagination(productspec.PageSize*(productspec.PageIndex-1), productspec.PageSize);
        }
        public ProductSpecification(int id):base(p=>p.Id==id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
    }
}
