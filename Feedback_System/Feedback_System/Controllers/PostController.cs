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
    [Authorize(Roles = "User,Admin")] // User ve Admin bu işlemi yapabilir
    public IActionResult GetAllPosts()
    {
        var posts = _readRepository.GetAll();

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
    [Authorize(Roles = "User,Admin")] // User ve Admin bu işlemi yapabilir
    public IActionResult GetPostById(int id)
    {
        var post = _readRepository.GetById(id);
        if (post == null)
        {
            return NotFound("Post not found.");
        }

        var postDto = new PostDto
        {
            Id = post.Id,
            Comment = post.Comment,
            ParkId = post.ParkId
        };

        return Ok(postDto);
    }

    // POST: api/Post
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin post oluşturabilir
    public IActionResult CreatePost([FromBody] PostDto postDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

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
    [Authorize(Roles = "Admin")] // Sadece Admin postları güncelleyebilir
    public IActionResult UpdatePost(int id, [FromBody] PostDto updatedPostDto)
    {
        if (!ModelState.IsValid || updatedPostDto.Id != id)
        {
            return BadRequest(ModelState);
        }

        var existingPost = _readRepository.GetById(id);
        if (existingPost == null)
        {
            return NotFound("Post not found.");
        }

        existingPost.Comment = updatedPostDto.Comment;
        existingPost.ParkId = updatedPostDto.ParkId ?? 0;

        _writeRepository.Update(existingPost);
        return NoContent();
    }

    // DELETE: api/Post/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin postları silebilir
    public IActionResult DeletePost(int id)
    {
        var post = _readRepository.GetById(id);
        if (post == null)
        {
            return NotFound("Post not found.");
        }

        _writeRepository.Delete(post);
        return NoContent();
    }
}
