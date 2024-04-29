using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arduino.Models;
using Arduino.Library;

namespace Arduino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogActionsController : ControllerBase
    {
        private readonly ILogActionsMethods _logActionsMethods;

        public LogActionsController(ILogActionsMethods logActionsMethods)
        {
            _logActionsMethods = logActionsMethods;
        }

        [HttpGet]
        public async Task<IEnumerable<LogAction>> GetLogActions()
        {
            return await _logActionsMethods.GetLogActions();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LogAction>> GetLogAction(int id)
        {
            var logAction = await _logActionsMethods.GetLogAction(id);

            if (logAction == null)
            {
                return NotFound();
            }

            return logAction;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogAction(int id, LogAction logAction)
        {
            if (id != logAction.Id)
            {
                return BadRequest();
            }


            logAction = await _logActionsMethods.GetLogAction(id);
            if (id != logAction.Id)
            {
                return NotFound();
            }


            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<LogAction>> PostLogAction(LogAction logAction)
        {
            await _logActionsMethods.CreateLogAction(logAction);

            return CreatedAtAction("GetLogAction", new { id = logAction.Id }, logAction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogAction(int id)
        {
            await _logActionsMethods.DeleteLogAction(id);

            return NoContent();
        }

        private bool LogActionExists(int id)
        {
            return _logActionsMethods.LogActionExists(id);
        }
    }
}
