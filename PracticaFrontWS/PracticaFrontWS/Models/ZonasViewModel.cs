using PracticaWS;

namespace PracticaFrontWS.Models
{
    public class ZonasViewModel
    {
        public int idzona { get; set; }
        public string zona { get; set; }
        public static explicit operator Zonas(ZonasViewModel model)
        {
            return new Zonas
            {
                idzona = model.idzona,
                zona = model.zona,
            };
        }
    }
}
