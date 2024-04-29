using Arduino.Library;
using Arduino.Models;
using Arduino.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Arduino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArduinoController : ControllerBase
    {
        private IArduinoService _iArduinoService;
        private ILogActionsMethods _logActionsMethods;

        public ArduinoController(IArduinoService iArduinoService)
        {
            _iArduinoService = iArduinoService;
        }
        [HttpPost]
        [Route("Send")]
        public IActionResult Send(Ardcommand ardcommand)
        {
            string result= _iArduinoService.Send(ardcommand);
            if (result=="Ok")
                return Ok();
            else
                return BadRequest(result);
        }

        [HttpPost]
        [Route("OnThenOff")]
        public IActionResult OnThenOff(Ardcommand ardcommand)
        {
            _iArduinoService.OnThenOff(ardcommand);
            return Ok();
        }
    }
}
