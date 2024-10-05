using Feedback_System.Abstractions;
using Feedback_System.Dtos;
using Feedback_System.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllParks()
        {
            var parks = _readRepository.GetAll();

            // Entity'den DTO'ya manuel dönüşüm
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
        public IActionResult GetParkById(int id)
        {
            try
            {
                var park = _readRepository.GetById(id);

                // Entity'den DTO'ya manuel dönüşüm
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
        public IActionResult CreatePark([FromBody] ParkDto parkDto)
        {
            if (parkDto == null)
            {
                return BadRequest("Park DTO cannot be null.");
            }

            // DTO'dan Entity'ye manuel dönüşüm
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
        public IActionResult UpdatePark(int id, [FromBody] ParkDto updatedParkDto)
        {
            if (updatedParkDto == null || updatedParkDto.Id != id)
            {
                return BadRequest("Park DTO is invalid.");
            }

            try
            {
                var existingPark = _readRepository.GetById(id);

                // DTO'dan Entity'ye manuel dönüşüm
                existingPark.Name = updatedParkDto.Name;
                existingPark.Description = updatedParkDto.Description;

                // Post'ların güncellenmesi gerekiyorsa
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
