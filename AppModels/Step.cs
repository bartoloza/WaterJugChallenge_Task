using System.Text.Json.Serialization;

namespace AppModels
{
    // Represents each step in the water jug process
    public class Step
    {
        [JsonPropertyName("step")]  // Using the correct JSON property name for "step"
        public int StepNumber { get; set; } // This is renamed to avoid conflict

        [JsonPropertyName("bucketX")]  // JSON property name for bucket X
        public int BucketX { get; set; }

        [JsonPropertyName("bucketY")]  // JSON property name for bucket Y
        public int BucketY { get; set; }

        [JsonPropertyName("action")]  // JSON property name for action
        public string Action { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("status")]  // JSON property name for status
        public string Status { get; set; } // Status property added
    }
}
