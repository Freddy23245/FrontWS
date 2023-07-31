using PracticaFrontWS.Models;

namespace PracticaFrontWS.Services.Interfaces
{
    public interface IPracticaService
    {
        List<PersonaViewModel> ObtenerPersonas(string ApellidoYNombre);
        List<ZonasViewModel> ObtenerZonas();
        PersonaViewModel RegistrarPersonas(PersonaViewModel nuevo);
        PersonaViewModel ModificarPersona(PersonaViewModel modifica);
        PersonaViewModel EliminarPersona(PersonaViewModel elimina);

        ZonasViewModel RegistrarZona(ZonasViewModel nueva);
        ZonasViewModel ModificarZonas(ZonasViewModel modifica);
        ZonasViewModel EliminarZona(ZonasViewModel elimina);
    }
}
