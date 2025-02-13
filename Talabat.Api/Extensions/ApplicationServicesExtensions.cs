using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Errors;
using Talabat.Api.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
           Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           Services.AddScoped<IUnitOfWork,UnitOfWork>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddAutoMapper(typeof(MappingProfile));


            // start handle validation error
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors)
                    .Select(err => err.ErrorMessage).ToArray();
                    var validationerrorresponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationerrorresponse);
                };
            });
            //end validation Error
            return Services;
        }
    }
}
