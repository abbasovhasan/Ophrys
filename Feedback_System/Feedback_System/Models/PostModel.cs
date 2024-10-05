using System.Text.Json.Serialization;

namespace Feedback_System.Models;

public class PostModel : BaseEntities
{
    public string Comment { get; set; }

    // Foreign key to ParkEntities
    public int ParkId { get; set; }

    // Navigation property for Park
    [JsonIgnore]
    public ParkEntities Park { get; set; }
}
