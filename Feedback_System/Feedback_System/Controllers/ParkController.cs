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
        return Ok(parks);
    }

    // GET: api/Park/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "User,Admin")]
    public IActionResult GetParkById(int id)
    {
        var park = _readRepository.GetById(id);
        if (park == null)
        {
            return NotFound("Park not found.");
        }
        return Ok(park);
    }

    // POST: api/Park
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin park oluşturabilir
    public IActionResult CreatePark([FromBody] ParkDto parkDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // DTO'dan Model'e manuel dönüşüm
        var park = new ParkEntities
        {
            Name = parkDto.Name,
            Description = parkDto.Description,
            Posts = parkDto.Posts?.Select(postDto => new PostModel
            {
                Comment = postDto.Comment
            }).ToList()
        };

        _writeRepository.Create(park);

        // Oluşturulan parkın postlarına parkId atanması
        if (park.Posts != null)
        {
            foreach (var post in park.Posts)
            {
                post.ParkId = park.Id; // ParkId'yi manuel olarak set ediyoruz
            }
            _writeRepository.Update(park); // Güncelleme yaparak ParkId'yi kaydediyoruz
        }

        return CreatedAtAction(nameof(GetParkById), new { id = park.Id }, park);
    }

    // PUT: api/Park/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin parkları güncelleyebilir
    public IActionResult UpdatePark(int id, [FromBody] ParkDto updatedParkDto)
    {
        if (!ModelState.IsValid || updatedParkDto.Id != id)
        {
            return BadRequest(ModelState);
        }

        var existingPark = _readRepository.GetById(id);
        if (existingPark == null)
        {
            return NotFound("Park not found.");
        }

        existingPark.Name = updatedParkDto.Name;
        existingPark.Description = updatedParkDto.Description;

        _writeRepository.Update(existingPark);
        return NoContent();
    }

    // DELETE: api/Park/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin parkları silebilir
    public IActionResult DeletePark(int id)
    {
        var park = _readRepository.GetById(id);
        if (park == null)
        {
            return NotFound("Park not found.");
        }

        _writeRepository.Delete(park);
        return NoContent();
    }
}
