using Microsoft.AspNetCore.Mvc;

namespace objetosPerdidos.Controllers;

[ApiController]
[Route("[controller]")]
public class LostObjectController : ControllerBase
{
    private static readonly List<LostObject> lostObjects = new()
    {
        new LostObject
        {
            Id = 1,
            Description = "Color azul",
            Category = "Pelota",
            PlaceFound = "Baños",
            DateFound = DateOnly.Parse("2025-12-25"),
            Claimed = false,
            OwnerName = "Pepe"
        }
    };

    private readonly ILogger<LostObjectController> _logger;

    public LostObjectController(ILogger<LostObjectController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Create([FromBody] LostObject newLostObject)
    {
        try
        {
            if (newLostObject.Id <= 0)
            {
                return BadRequest("Ingresa un id valido.");
            }

            bool idExists = lostObjects.Any(x => x.Id == newLostObject.Id);

            if (idExists)
            {
                return Conflict("El ID ya existe");
            }

            if (String.IsNullOrWhiteSpace(newLostObject.Description))
            {
                return BadRequest("El objeto debe contener una descripcion.");
            }

            if (newLostObject.DateFound > DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("La fecha debe ser menor a la actual.");
            }

            lostObjects.Add(newLostObject);
            return Ok("Objeto agregado exitosamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            if (lostObjects.Count <= 0)
            {
                return NotFound("No hay objetos registrados aun.");
            }

            return Ok(lostObjects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpGet("{Id}")]
    public IActionResult GetById(int Id)
    {
        try
        {
            if (Id <= 0)
            {
                return BadRequest("Ingresa un id valido.");
            }

            LostObject result = lostObjects.FirstOrDefault(LO => LO.Id == Id);

            if (result is null)
            {
                return BadRequest("Ingresa un id valido.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] LostObject updatedLostObject)
    {
        try
        {
            if (updatedLostObject.Id <= 0)
            {
                return BadRequest("Ingresa un id valido.");
            }

            if (String.IsNullOrWhiteSpace(updatedLostObject.Description))
            {
                return BadRequest("El objeto debe contener una descripcion.");
            }

            if (updatedLostObject.DateFound > DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("La fecha debe ser menor a la actual.");
            }

            var result = lostObjects.FirstOrDefault(LO => LO.Id == updatedLostObject.Id);

            if (result is null)
            {
                return BadRequest("Ingresa un id valido.");
            }

            result.Id = updatedLostObject.Id;
            result.Description = updatedLostObject.Description;
            result.Category = updatedLostObject.Category;
            result.PlaceFound = updatedLostObject.PlaceFound;
            result.DateFound = updatedLostObject.DateFound;
            result.Claimed = updatedLostObject.Claimed;
            result.OwnerName = updatedLostObject.OwnerName;

            return Ok("Objeto modificado correctamente");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpDelete]
    public IActionResult Delete(int Id)
    {
        try
        {
            LostObject result = lostObjects.FirstOrDefault(LO => LO.Id == Id);

            if (result is null)
            {
                return NotFound("Ingresa un id valido.");
            }

            lostObjects.Remove(result);

            return Ok("El objeto fue borrado con exito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpGet("Description")]
    public IActionResult GetByDescription(string Description)
    {
        try
        {
            LostObject result = lostObjects.FirstOrDefault(LO => LO.Description == Description);

            if (result is null)
            {
                return NotFound("No existe un objeto con esa descripcion.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpGet("Category")]
    public IActionResult GetByCategory(string Category)
    {
        try
        {
            var result = lostObjects.Where(LO => LO.Category == Category).ToList();

            if (result is null)
            {
                return NotFound("No existen objetos con esa categoria.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpGet("NotClaimed")]
    public IActionResult GetNotClaimed()
    {
        try
        {
            var NotClaimed = lostObjects.Where(LO => LO.Claimed == false).ToList();

            if (NotClaimed.Count == 0)
            {
                return NotFound("No hay objetos sin reclamar.");
            }

            return Ok(NotClaimed);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpGet("ClaimedDate")]
    public IActionResult GetClaimed(string claimedDate)
    {
        try
        {
            var claimed = lostObjects.Where(LO => LO.Claimed == true)
                                .Where(LO => LO.DateFound >= DateOnly.Parse(claimedDate))
                                .OrderDescending()
                                .ToList();
            
            if (claimed.Count == 0)
            {
                return NotFound("No hay reclamados luego de esa fecha.");
            }

            return Ok(claimed);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }

    [HttpPatch("Claimed")]
    public IActionResult ChangeClaimedStatus(int id)
    {
        try
        {
            var claimedObject = lostObjects.Where(LO => LO.Claimed == false)
                                        .FirstOrDefault(LO => LO.Id == id);
        
            if (claimedObject is null)
            {
                return NotFound("No hay objetos sin reclamar con ese id.");
            }

            claimedObject.Claimed = true;

            return Ok("Objeto marcado como reclamado");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrio un problema en el servidor {ex.Message}.");
        }
    }
}