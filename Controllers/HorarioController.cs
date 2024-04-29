using Arduino.Library;
using Arduino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arduino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioMethods _iHorarioMethods;

        public HorarioController(IHorarioMethods iHorarioMethods)
        {
            _iHorarioMethods = iHorarioMethods;
        }

        [HttpGet]
        public async Task<IEnumerable<Horario>> GetHorarios()
        {
            return await _iHorarioMethods.GetHorarios();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Horario>> GetHorario(int id)
        {
            var horario = await _iHorarioMethods.GetHorario(id);

            if (horario == null)
            {
                return NotFound();
            }

            return horario;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorario(int id, Horario horario)
        {
            if (id != horario.Id)
            {
                return BadRequest();
            }


            horario = await _iHorarioMethods.GetHorario(id);
            if (id != horario.Id)
            {
                return NotFound();
            }


            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<LogAction>> PostHorario(Horario horario)
        {
            await _iHorarioMethods.CreateHorario(horario);

            return CreatedAtAction("PostHorario", new { id = horario.Id }, horario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            await _iHorarioMethods.DeleteHorario(id);

            return NoContent();
        }

        private bool HorarioExists(int id)
        {
            return _iHorarioMethods.HorarioExists(id);
        }
    }
}
