using PracticaFrontWS.Models;

namespace PracticaFrontWS.Core.Session
{
    public interface ISessionData
    {
      
            List<PersonaViewModel> Personas { get; set; }
            string ValorString { get; set; }
            PersonaViewModel PersonaSeleccionada { get; set; }
        List<ZonasViewModel> Zonas { get; set; }

    }
}
