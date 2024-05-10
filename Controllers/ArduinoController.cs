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
        private IConfiguration _configuration;

        private bool useCOM3 = false;

        public ArduinoController(IArduinoService iArduinoService, IConfiguration configuration)
        {
            _iArduinoService = iArduinoService;
            _configuration = configuration;

            useCOM3 = _configuration.GetValue<bool>("UseCOM3");
        }
        [HttpPost]
        [Route("Send")]
        public IActionResult Send(Ardcommand ardcommand)
        {
            string result= _iArduinoService.Send(ardcommand, useCOM3); 
            
            if (result=="Ok")
                return Ok();
            else
                return BadRequest(result);
        }

        [HttpPost]
        [Route("OnThenOff")]
        public IActionResult OnThenOff(Ardcommand ardcommand)
        {
            _iArduinoService.OnThenOff(ardcommand, useCOM3);
            return Ok();
        }
    }
}
