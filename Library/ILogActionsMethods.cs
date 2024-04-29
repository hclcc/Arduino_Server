using Arduino.Models;

namespace Arduino.Library
{
    public interface ILogActionsMethods
    {
        Task<LogAction> CreateLogAction(LogAction logAction);
        Task<bool> DeleteLogAction(int id);
        Task<LogAction> GetLogAction(int id);
        Task<IEnumerable<LogAction>> GetLogActions();
        Task<LogAction> UpdateLogAction(int id, LogAction logAction);

        bool LogActionExists(int id);
    }
}