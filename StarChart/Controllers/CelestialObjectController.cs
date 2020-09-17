using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [ActionName("GetById")]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = _context.CelestialObjects.Where(o => o.Id == id).SingleOrDefault();

                if (result == null)
                {
                    return NotFound();
                }

                var orbitingBodies = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id)?.ToList();
                result.Satellites = orbitingBodies;

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database internal error!");
            }
        }

        [HttpGet("{name}")]
        public  IActionResult GetByName(string name)
        {
            try
            {
                if (_context.CelestialObjects.Any(o => o.Name == name))
                {
                    var results = _context.CelestialObjects.Where(o => o.Name == name).ToList();

                    foreach (var item in results)
                    {
                        var orbitingBodies = _context.CelestialObjects.Where(o => o.OrbitedObjectId == item.Id)?.ToList();
                        item.Satellites = orbitingBodies;
                    }

                    return Ok(results);
                }
                else
                {
                    return NotFound();
                } 
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database internal error!");
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var results = _context.CelestialObjects?.ToList();

                if (results == null)
                {
                    return NotFound();
                }

                foreach (var item in results)
                {
                    var orbitingBodies = _context.CelestialObjects.Where(o => o.OrbitedObjectId == item.Id)?.ToList();
                    item.Satellites = orbitingBodies;
                }

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database internal error!");
            }
        }
    }
}
