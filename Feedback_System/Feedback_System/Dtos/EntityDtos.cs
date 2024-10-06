namespace Feedback_System.Dtos;

public class ParkDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PostDto> Posts { get; set; }
}



public class PostDto
{
    public int? Id { get; set; }
    public string Comment { get; set; }
    public int? ParkId { get; set; }
}


public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

