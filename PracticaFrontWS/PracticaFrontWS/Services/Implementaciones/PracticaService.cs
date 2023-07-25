using PracticaFrontWS.Core;
using PracticaFrontWS.Models;
using PracticaFrontWS.Services.Interfaces;
using PracticaFrontWS.Services.Mappers;
using PracticaWS;

namespace PracticaFrontWS.Services.Implementaciones
{
    public class PracticaService : IPracticaService
    {

        public List<PersonaViewModel> ObtenerPersonas(string ApellidoYNombre)
        {
            var srv = new BaseService().ObtenerPracticaWS();
            var persona = srv.ObtenerPersonas(ApellidoYNombre);
            return persona.Select(x => PracticaServiceMapper.MapearPersona(x)).ToList();
        }
        public List<ZonasViewModel> ObtenerZonas()
        {
            var srv = new BaseService().ObtenerPracticaWS();
            var zona = srv.ObtenerZonas();
            return zona.Select(x => PracticaService2Mapper.MapearPersona(x)).ToList();

        }
        public PersonaViewModel RegistrarPersonas(PersonaViewModel nuevo)
        {
            var srv = new BaseService().ObtenerPracticaWS();
            var NuevaPersona = srv.RegistrarPersonas((Personas)nuevo);
            return PracticaServiceMapper.MapearPersona(NuevaPersona);

        }
        public PersonaViewModel ModificarPersona(PersonaViewModel modifica)
        {
            var srv = new BaseService().ObtenerPracticaWS();
            var ModPersona = srv.ModificarPersonas((Personas)modifica);
            return PracticaServiceMapper.MapearPersona(ModPersona);
        }
        public PersonaViewModel EliminarPersona(PersonaViewModel elimina)
        {
            var srv = new BaseService().ObtenerPracticaWS();
            var PerElimina = srv.EliminarPersonas((Personas)elimina);
            return PracticaServiceMapper.MapearPersona(PerElimina);
        }
    }
}
