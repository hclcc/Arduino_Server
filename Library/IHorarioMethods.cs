using Arduino.Models;

namespace Arduino.Library
{
    public interface IHorarioMethods
    {
        Task<Horario> CreateHorario(Horario horario);
        Task<bool> DeleteHorario(int id);
        Task<Horario> GetHorario(int id);
        Task<IEnumerable<Horario>> GetHorarios();
        Task<Horario> UpdateHorario(int id, Horario logAction);

        bool HorarioExists(int id);
    }
}


