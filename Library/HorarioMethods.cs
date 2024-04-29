using Arduino.Models;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Library
{
    public class HorarioMethods : IHorarioMethods
    {
        private readonly CTRLArduinoContext _context;

        public HorarioMethods(CTRLArduinoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Horario>> GetHorarios()
        {
            return await _context.Horarios.ToListAsync();
        }

        public async Task<Horario> GetHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
            {
                return new Horario();
            }

            return horario;
        }

        public async Task<Horario> UpdateHorario(int id, Horario horario)
        {
            if (id != horario.Id)
            {
                return null;
            }

            _context.Entry(horario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return horario;
        }

        public async Task<Horario> CreateHorario(Horario horario)
        {
            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();

            return horario;
        }

        public async Task<bool> DeleteHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return false;
            }

            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.Id == id);
        }
    }
}
