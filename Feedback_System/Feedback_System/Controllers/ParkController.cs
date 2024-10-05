

namespace Feedback_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkController : ControllerBase
    {
        private readonly IReadRepository<ParkEntities> _readRepository;
        private readonly IWriteRepository<ParkEntities> _writeRepository;

        public ParkController(IReadRepository<ParkEntities> readRepository, IWriteRepository<ParkEntities> writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        // GET: api/Park
        [HttpGet]
        [Authorize(Roles = "User,Admin")] // Hem User hem Admin GET işlemini yapabilir
        public IActionResult GetAllParks()
        {
            var parks = _readRepository.GetAll();

            var parkDtos = parks.Select(park => new ParkDto
            {
                Id = park.Id,
                Name = park.Name,
                Description = park.Description,
                Posts = park.Posts?.Select(post => new PostDto
                {
                    Id = post.Id,
                    Comment = post.Comment,
                    ParkId = post.ParkId
                }).ToList()
            }).ToList();

            return Ok(parkDtos);
        }

        // GET: api/Park/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")] // Hem User hem Admin GET by ID yapabilir
        public IActionResult GetParkById(int id)
        {
            try
            {
                var park = _readRepository.GetById(id);

                var parkDto = new ParkDto
                {
                    Id = park.Id,
                    Name = park.Name,
                    Description = park.Description,
                    Posts = park.Posts?.Select(post => new PostDto
                    {
                        Id = post.Id,
                        Comment = post.Comment,
                        ParkId = post.ParkId
                    }).ToList()
                };

                return Ok(parkDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Park
        [HttpPost]
        [Authorize(Roles = "Admin")] // Sadece Admin park oluşturabilir
        public IActionResult CreatePark([FromBody] ParkDto parkDto)
        {
            if (parkDto == null)
            {
                return BadRequest("Park DTO cannot be null.");
            }

            var park = new ParkEntities
            {
                Name = parkDto.Name,
                Description = parkDto.Description,
                Posts = parkDto.Posts?.Select(post => new PostModel
                {
                    Comment = post.Comment,
                    ParkId = post.ParkId ?? 0
                }).ToList()
            };

            _writeRepository.Create(park);
            return CreatedAtAction(nameof(GetParkById), new { id = park.Id }, park);
        }

        // PUT: api/Park/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Sadece Admin parkları güncelleyebilir
        public IActionResult UpdatePark(int id, [FromBody] ParkDto updatedParkDto)
        {
            if (updatedParkDto == null || updatedParkDto.Id != id)
            {
                return BadRequest("Park DTO is invalid.");
            }

            try
            {
                var existingPark = _readRepository.GetById(id);

                existingPark.Name = updatedParkDto.Name;
                existingPark.Description = updatedParkDto.Description;

                existingPark.Posts = updatedParkDto.Posts?.Select(postDto => new PostModel
                {
                    Id = postDto.Id ?? 0,
                    Comment = postDto.Comment,
                    ParkId = postDto.ParkId ?? 0
                }).ToList();

                _writeRepository.Update(existingPark);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Park/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Sadece Admin parkları silebilir
        public IActionResult DeletePark(int id)
        {
            try
            {
                var park = _readRepository.GetById(id);
                _writeRepository.Delete(park);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
