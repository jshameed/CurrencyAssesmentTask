
using AutoMapper;
using System.Globalization;
using System.Reflection;

namespace DemoCurrency.Profiles
{

    
    public class RateProfile : Profile
    {
        public RateProfile()
        {
            CreateMap<Entities.RateEntitties, Model.CurrencyRates>();
              
        }
      
    }
}
