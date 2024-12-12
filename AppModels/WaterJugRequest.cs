using System.ComponentModel.DataAnnotations;

namespace AppModels
{
    public class WaterJugRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Value must be a positive integer.")]
        public int XCapacity { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be a positive integer.")]
        public int YCapacity { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be a positive integer.")]
        public int ZAmountWanted { get; set; }
    }
}