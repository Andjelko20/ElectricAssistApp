using Server.Models;

namespace Server.DTOs
{
    public class ProsumerForDSOFilterDTO
    {
        public UserModel userModel { get; set; }
        public double CurrentConsumption { get; set; }
        public double CurrentProduction { get; set; }

        public ProsumerForDSOFilterDTO(UserModel userModel, double currentConsumption, double currentProduction)
        {
            this.userModel = userModel;
            CurrentConsumption = currentConsumption;
            CurrentProduction = currentProduction;
        }
    }
}
