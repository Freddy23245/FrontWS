using PracticaFrontWS.Models;

namespace PracticaFrontWS.Services.Mappers
{
    public class PracticaServiceMapper
    {
        public static PersonaViewModel MapearPersona(PracticaWS.Personas data)
        {
            return new PersonaViewModel
            {
                id = data.id,
                Cuil = data.Cuil,
                ApellidoYNombre = data.ApellidoYNombre,
                FechaNacimiento = data.FechaNacimiento,
                idzona = data.idZona,
                zona = data.Zona
            };
        }
    }
}
