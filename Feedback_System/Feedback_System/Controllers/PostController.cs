using Feedback_System.Abstractions;
using Feedback_System.Dtos;
using Feedback_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Feedback_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IReadRepository<PostModel> _readRepository;
        private readonly IWriteRepository<PostModel> _writeRepository;

        public PostController(IReadRepository<PostModel> readRepository, IWriteRepository<PostModel> writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        // GET: api/Post
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var posts = _readRepository.GetAll();

            // Entity'den DTO'ya manuel dönüşüm
            var postDtos = posts.Select(post => new PostDto
            {
                Id = post.Id,
                Comment = post.Comment,
                ParkId = post.ParkId
            }).ToList();

            return Ok(postDtos);
        }

        // GET: api/Post/{id}
        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            try
            {
                var post = _readRepository.GetById(id);

                // Entity'den DTO'ya manuel dönüşüm
                var postDto = new PostDto
                {
                    Id = post.Id,
                    Comment = post.Comment,
                    ParkId = post.ParkId
                };

                return Ok(postDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Post
        [HttpPost]
        public IActionResult CreatePost([FromBody] PostDto postDto)
        {
            if (postDto == null)
            {
                return BadRequest("Post DTO cannot be null.");
            }

            // DTO'dan Entity'ye manuel dönüşüm
            var post = new PostModel
            {
                Comment = postDto.Comment,
                ParkId = postDto.ParkId ?? 0
            };

            _writeRepository.Create(post);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        // PUT: api/Post/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] PostDto updatedPostDto)
        {
            if (updatedPostDto == null || updatedPostDto.Id != id)
            {
                return BadRequest("Post DTO is invalid.");
            }

            try
            {
                var existingPost = _readRepository.GetById(id);

                // DTO'dan Entity'ye manuel dönüşüm
                existingPost.Comment = updatedPostDto.Comment;
                existingPost.ParkId = updatedPostDto.ParkId ?? 0;

                _writeRepository.Update(existingPost);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Post/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var post = _readRepository.GetById(id);
                _writeRepository.Delete(post);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
