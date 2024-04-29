using Arduino.Models;

namespace Arduino.Services
{
    public interface IArduinoService
    {
        string Send(Ardcommand ardcommand);
        string OnThenOff(Ardcommand ardcommand);
    }
}
