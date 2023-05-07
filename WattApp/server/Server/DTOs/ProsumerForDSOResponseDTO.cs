using Server.DTOs.Responses;

namespace Server.DTOs
{
    public class ProsumerForDSOResponseDTO
    {
        public UserDetailsDTO userDetailsDTO { get; set; }
        public double CurrentConsumption { get; set; } = 0.0;
        public double CurrentProduction { get; set; } = 0.0;
    }
}
