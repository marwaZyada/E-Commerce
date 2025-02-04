using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
      public static async Task SeedAsync(StoreContext dbcontext)
        {
            //productBrand
            if (!dbcontext.ProductBrands.Any()) {
                var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                if (brands?.Count > 0)
                {
                    foreach (var data in brands)
                    {
                        await dbcontext.ProductBrands.AddAsync(data);
                        await dbcontext.SaveChangesAsync();
                    }
                }
            }
            //ProductType
            if (!dbcontext.ProductTypes.Any())
            {
                var TypeData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                if (types?.Count > 0)
                {
                    foreach (var data in types)
                    {
                        await dbcontext.ProductTypes.AddAsync(data);
                        await dbcontext.SaveChangesAsync();
                    }
                }
            }
            //Products
            if (!dbcontext.Products.Any())
            {
                var productdata = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products=JsonSerializer.Deserialize<List<Product>>(productdata);
                if (products?.Count>0)
                {
                    foreach(var data in products)
                    {
                        await dbcontext.Products.AddAsync(data);
                        await dbcontext.SaveChangesAsync();
                    }
                }
            }

            //DeliverMethod
            if (!dbcontext.DeliveryMethods.Any())
            {
                var deliverymethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliverymethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverymethodData);
                if (deliverymethods?.Count > 0)
                {
                    foreach (var data in deliverymethods)
                    {
                        await dbcontext.DeliveryMethods.AddAsync(data);
                        await dbcontext.SaveChangesAsync();
                    }
                }
            }
            //DeliverMethod
        }
    }
}
