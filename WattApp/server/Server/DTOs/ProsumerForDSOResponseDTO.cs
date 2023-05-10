using Server.DTOs.Responses;

namespace Server.DTOs
{
    public class ProsumerForDSOResponseDTO
    {
        public UserDetailsDTO userDetailsDTO { get; set; }
        public double CurrentConsumption { get; set; }
        public double CurrentProduction { get; set; }

        public ProsumerForDSOResponseDTO(UserDetailsDTO userDetailsDTO, double currentConsumption, double currentProduction)
        {
            this.userDetailsDTO = userDetailsDTO;
            CurrentConsumption = currentConsumption;
            CurrentProduction = currentProduction;
        }
    }
}
