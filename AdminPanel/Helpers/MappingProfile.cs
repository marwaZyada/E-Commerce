using AdminPanel.ViewModels;
using AutoMapper;
using Talabat.Core.Entities;

namespace AdminPanel.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewForm>().ReverseMap(); 
        }
    }
}
