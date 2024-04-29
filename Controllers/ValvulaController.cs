using Arduino.Library;
using Arduino.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValvulaController : ControllerBase
    {
        private readonly IValvulaMethods _valvulaMethods;

        public ValvulaController(IValvulaMethods valvulaMethods)
        {
            _valvulaMethods = valvulaMethods;
        }

        [HttpGet]
        public async Task<IActionResult> GetValvulas()
        {
            try
            {
                var valvulas = await _valvulaMethods.GetValvulas();
                Serilog.Log.Information($"GetValvulas - Success");
                return Ok(valvulas);

            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"GetValvulas - {ex.Message} - Failed");
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Valvula>> GetValvula(int id)
        {
            try
            {
                var valvula = await _valvulaMethods.GetValvula(id);

                if (valvula == null)
                {
                    Serilog.Log.Error($"GetValvula - NotFound - {id} - Failed");
                    return NotFound();
                }

                Serilog.Log.Information($"GetValvula - Success");
                return valvula;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"GetValvula - {id} - {ex.Message} - Failed");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> PutValvula( Valvula valvula)
        {
            if ( valvula.IdValve==0)
            {
                Serilog.Log.Error($"GetValvula - id != valvula.IdValve - Failed");
                return BadRequest();
            }

            try
            {
                valvula = await _valvulaMethods.UpdateValvula(valvula);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"PutValvula - {valvula.IdValve} - {ex.Message} - Failed");
                return BadRequest(ex.Message);
            }
            Serilog.Log.Information($"PutValvula - {valvula.IdValve} - Success");
            return NoContent();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Valvula>> PostValvula(Valvula valvula)
        {
            try
            {
                await _valvulaMethods.CreateValvula(valvula);
                return CreatedAtAction("PostValvula", new { id = valvula.IdValve }, valvula);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"PostValvula - {ex.Message} - Failed");
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteValvula(Valvula valvula)
        {
            try
            {
                await _valvulaMethods.DeleteValvula(valvula.IdValve);
                Serilog.Log.Information($"DeleteValvula - {valvula.IdValve} - Success");
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"DeleteValvula - {valvula.IdValve} - {ex.Message} - Failed");
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}

