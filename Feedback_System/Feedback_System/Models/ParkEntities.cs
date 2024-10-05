namespace Feedback_System.Models;

public class ParkEntities : BaseEntities
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Parkın birden fazla postu olabilir (one-to-many relationship)
    public List<PostModel> Posts { get; set; }
}