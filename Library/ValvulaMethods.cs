using Arduino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Library
{
    public class ValvulaMethods : IValvulaMethods
    {
        private readonly CTRLArduinoContext _context;

        public ValvulaMethods(CTRLArduinoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Valvula>> GetValvulas()
        {
            return await _context.Valvulas.ToListAsync();

        }
        public async Task<Valvula> CreateValvula(Valvula valvula)
        {
            _context.Valvulas.Add(valvula);
            await _context.SaveChangesAsync();

            return valvula;
        }

        public async Task<Valvula> UpdateValvula(Valvula valvula)
        {
            if ( valvula.IdValve==0)
            {
                return null;
            }

            _context.Entry(valvula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!valvulaExists(valvula.IdValve))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return valvula;
        }


        public async Task<bool> DeleteValvula(int IdValve)
        {
            var valvula = await _context.Valvulas.FindAsync(IdValve);
            if (valvula == null)
            {
                return false;
            }

            _context.Valvulas.Remove(valvula);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Valvula> GetValvula(int IdValve)
        {
            var valvula = await _context.Valvulas.FindAsync(IdValve);

            if (valvula == null)
            {
                return new Valvula();
            }

            return valvula;
        }
        public bool valvulaExists(int IdValve)
        {
            return _context.Valvulas.Any(e => e.IdValve == IdValve);
        }


    }
}
