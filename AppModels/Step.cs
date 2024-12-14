using System.Text.Json.Serialization;

namespace AppModels
{
    // Represents each step in the water jug process
    public class Step
    {
        [JsonPropertyName("step")]  // Using the correct JSON property name for "step"
        public int StepNumber { get; set; } // This is renamed to avoid conflict

        public int BucketX { get; set; }

        public int BucketY { get; set; }

        public string Action { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
