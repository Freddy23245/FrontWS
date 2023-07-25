using PracticaFrontWS.Models;
using System.Text.Json;

namespace PracticaFrontWS.Core.Session
{
    public class SessionData:ISessionData
    {
        private IHttpContextAccessor _httpContextAccessor;
        public SessionData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public List<PersonaViewModel> Personas
        {
            get
            {
                return ObtenerValor<List<PersonaViewModel>>("_Personas");
            }
            set
            {
                Guardar("_Personas", value);
            }

        }
        public List<ZonasViewModel> Zonas
        {
            get
            {
                return ObtenerValor<List<ZonasViewModel>>("_Zonas");
            }
            set
            {
                Guardar("_Zonas", value);
            }

        }
        public string ValorString
        {
            get
            {
                return ObtenerValor<string>("_ValorString");
            }
            set
            {
                Guardar("_ValorString", value);
            }

        }

        private void Guardar(string clave, object valor)//List<PersonaViewModel> valor
        {
            var strPersonas = JsonSerializer.Serialize(valor);
            _httpContextAccessor.HttpContext.Session.SetString(clave, strPersonas);
        }

        private T ObtenerValor<T>(string clave)//List<PersonaViewModel> ObtenerValor(string clave)
        {
            var valor = _httpContextAccessor.HttpContext.Session.GetString(clave);
            if (string.IsNullOrEmpty(valor))
                return default;
            var objeto = JsonSerializer.Deserialize<T>(valor);
            return objeto;
        }

        public PersonaViewModel PersonaSeleccionada
        {
            get
            {
                return ObtenerValor<PersonaViewModel>("_PersonaSeleccionada");
            }
            set
            {
                Guardar("_PersonaSeleccionada", value);
            }
        }
    }
}
