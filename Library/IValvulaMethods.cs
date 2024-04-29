using Arduino.Models;

namespace Arduino.Library
{
    public interface IValvulaMethods
    {
        public Task<IEnumerable<Valvula>> GetValvulas();
        public Task<Valvula> CreateValvula(Valvula valvula);
        public Task<bool> DeleteValvula(int id);
        public Task<Valvula> GetValvula(int id);
        public Task<Valvula> UpdateValvula( Valvula valvula);
    }
}