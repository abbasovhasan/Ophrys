﻿namespace Feedback_System.Dtos;

public class ParkDto
{
    public int? Id { get; set; }  // Güncellemeler için Id
    public string Name { get; set; }
    public string Description { get; set; }

    // Park ile ilişkili postlar
    public List<PostDto> Posts { get; set; }
}


    public class PostDto
    {
        public int? Id { get; set; }  // Güncellemeler için Id
        public string Comment { get; set; }

        // Post'un hangi parka ait olduğunu belirtmek için ParkId
        public int? ParkId { get; set; }
    }
