using EMSI_Corporation.Models;

namespace EMSI_Corporation.ViewModels
{
    public class UsuarioEventosVM
    {
        public Usuario usuario { get; set; }
        public List<EventoCalendario> eventos { get; set; }
    }
}
