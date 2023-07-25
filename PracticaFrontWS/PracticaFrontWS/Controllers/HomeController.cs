
using Microsoft.AspNetCore.Mvc;
using PracticaFrontWS.Core.Session;
using PracticaFrontWS.Models;
using PracticaFrontWS.Services.Interfaces;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace PracticaFrontWS.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPracticaService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionData _session;
        public HomeController(IPracticaService service, IHttpContextAccessor httpContextAccessor,ISessionData session)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _session = session;
        }

        public IActionResult Index()
        {
            //ViewBag.zonas = _service.ObtenerZonas();
            //var personas = _service.ObtenerPersonas(ApellidoYNombre);
            //_session.Personas = personas;
            return View();
        }
        public IActionResult Filtrar(string ApellidoYNombre)
        {
            //ViewBag.zonas = _service.ObtenerZonas();
            var personas = _service.ObtenerPersonas(ApellidoYNombre);
            _session.Personas = personas;
            return Json(personas);
        }
        public IActionResult Create()
        {
            ViewBag.zonas = _service.ObtenerZonas();
     
            return View();
        }
        public IActionResult Crear(PersonaViewModel nuevo)
        {
            if(string.IsNullOrEmpty(nuevo.Cuil) || string.IsNullOrEmpty(nuevo.ApellidoYNombre) || nuevo.FechaNacimiento == DateTime.MinValue)
            {
                return RedirectToAction("Create");
            }
            else
            {
                _service.RegistrarPersonas(nuevo);
                return RedirectToAction(nameof(Index));
            }
     
        }
        public IActionResult Detalle( int id)
        {
            ViewBag.zonas = _service.ObtenerZonas();

            var personaSeleccionada = _session.Personas.FirstOrDefault(x => x.id == id);
            _session.PersonaSeleccionada = personaSeleccionada;
            return View(personaSeleccionada);
        }
        public IActionResult ModificarPersona(PersonaViewModel modifica)
        {
            modifica.id = _session.PersonaSeleccionada.id;
            _service.ModificarPersona(modifica);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult EliminarPersona(PersonaViewModel elimina)
        {
            elimina.id = _session.PersonaSeleccionada.id;
            _service.EliminarPersona(elimina);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult GenerarPDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var fill = _session.Personas;
              
            
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
       
                PdfPTable table = new PdfPTable(4);
                table.AddCell("Cuil");
                table.AddCell("Apellido y Nombre");
                table.AddCell("Fecha de Nacimiento");
                table.AddCell("Zona de Residencia");
                if (fill != null)
                {
                    foreach (var item in fill)
                    {
                        table.AddCell(item.Cuil);
                        table.AddCell(item.ApellidoYNombre);
                        table.AddCell(item.FechaNacimiento.ToString("dd/MM/yyyy"));
                        table.AddCell(item.zona);
                        //Paragraph paragraph = new Paragraph(item.Cuil +" - " + item.ApellidoYNombre + " - " + item.FechaNacimiento.ToString("dd/MM/yyyy") + " - " + item.zona);
                    }
                }
                document.Add(table);
                document.Close();
                return File(ms.ToArray(), "application/pdf", "ejemplo.pdf");
            }
        }
        public IActionResult Visualizador()
        {
            var pdfFilePath = @"c:\Users\Cristian\Downloads\ejemplo.pdf"; 
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            return File(pdfBytes, "application/pdf");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Zonas()
        {
            return View();
        }
        public IActionResult Resultados()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}