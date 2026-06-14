using GreenSolutions.Enums;

namespace GreenSolutions.Services
{
    public class PricingService
    {
        public decimal Calculate(decimal basePrice, string state)
        {
            return state == "SP" ? basePrice: basePrice * 1.18m;
        }
    }
}
