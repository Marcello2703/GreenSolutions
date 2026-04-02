using GreenSolutions.Enums;

namespace GreenSolutions.Services
{
    public class PricingService
    {
        public decimal Calculate(decimal basePrice, ClientType clientType)
        {
            switch (clientType)
            {
                case ClientType.FiliatedLocal: return basePrice * 0.9m;
                case ClientType.FiliatedNational: return basePrice * 0.85m;
                default : return basePrice;
            }
        }
    }
}
