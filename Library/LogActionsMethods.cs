using Arduino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Library
{
    public class LogActionsMethods : ILogActionsMethods
    {
        private readonly CTRLArduinoContext _context;

        public LogActionsMethods(CTRLArduinoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<LogAction>> GetLogActions()
        {
            return await _context.LogActions.ToListAsync();
        }

        public async Task<LogAction> GetLogAction(int id)
        {
            var logAction = await _context.LogActions.FindAsync(id);

            if (logAction == null)
            {
                return new LogAction();
            }

            return logAction;
        }

        public async Task<LogAction> UpdateLogAction(int id, LogAction logAction)
        {
            if (id != logAction.Id)
            {
                return null;
            }

            _context.Entry(logAction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogActionExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return logAction;
        }

        public async Task<LogAction> CreateLogAction(LogAction logAction)
        {
            _context.LogActions.Add(logAction);
            await _context.SaveChangesAsync();

            return logAction;
        }

        public async Task<bool> DeleteLogAction(int id)
        {
            var logAction = await _context.LogActions.FindAsync(id);
            if (logAction == null)
            {
                return false;
            }

            _context.LogActions.Remove(logAction);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool LogActionExists(int id)
        {
            return _context.LogActions.Any(e => e.Id == id);
        }
    }
}
