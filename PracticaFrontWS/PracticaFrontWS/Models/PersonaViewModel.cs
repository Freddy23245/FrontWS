using PracticaWS;
using System.ComponentModel.DataAnnotations;

namespace PracticaFrontWS.Models
{
    public class PersonaViewModel
    {
        public int id { get; set; }
        public string Cuil { get; set; }
        public string ApellidoYNombre { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        public int idzona { get; set; }
        public string zona { get; set; }

        public static explicit operator Personas(PersonaViewModel model)
        {
            return new Personas
            {
                id = model.id,
                Cuil = model.Cuil,
                ApellidoYNombre = model.ApellidoYNombre,
                FechaNacimiento = model.FechaNacimiento,
                idZona = model.idzona,
                Zona = model.zona
            };
        }
    }
}
