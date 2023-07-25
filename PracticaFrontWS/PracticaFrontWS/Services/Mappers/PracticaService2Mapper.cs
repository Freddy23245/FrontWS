using PracticaFrontWS.Models;

namespace PracticaFrontWS.Services.Mappers
{
    public class PracticaService2Mapper
    {
        public static ZonasViewModel MapearPersona(PracticaWS.Zonas data)
        {
            return new ZonasViewModel
            {
                idzona = data.idzona,
                zona = data.zona
            };
        }
    }
}
