
using Microsoft.AspNetCore.Mvc;
using PracticaFrontWS.Core.Session;
using PracticaFrontWS.Models;
using PracticaFrontWS.Services.Interfaces;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Routing.Constraints;

namespace PracticaFrontWS.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPracticaService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionData _session;
        public HomeController(IPracticaService service, IHttpContextAccessor httpContextAccessor, ISessionData session)
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
            if (string.IsNullOrEmpty(nuevo.Cuil) || string.IsNullOrEmpty(nuevo.ApellidoYNombre) || nuevo.FechaNacimiento == DateTime.MinValue)
            {
                return RedirectToAction("Create");
            }
            else
            {
                _service.RegistrarPersonas(nuevo);
                return RedirectToAction(nameof(Index));
            }

        }
        public IActionResult Detalle(int id)
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
        [HttpPost]
        public IActionResult GenerarFactura(FacturaViewModel factura)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
          
                PdfPTable tableTop = new PdfPTable(1);
                tableTop.WidthPercentage = 10;
                tableTop.DefaultCell.Border = Rectangle.NO_BORDER;
          

                PdfPCell cell = new PdfPCell(new Phrase("A"));
                cell.Padding = 1;
                cell.BorderWidth = 2f; // Grosor del borde
                cell.BorderColor = BaseColor.BLACK; // Color del borde
                cell.FixedHeight = 40;
    
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tableTop.AddCell(cell);
                document.Add(tableTop);
                document.Add(Chunk.NEWLINE);
                var fechaActual = DateTime.Today;
                var tbl = new PdfPTable(new float[] { 50f,50f }) { WidthPercentage = 100, PaddingTop = 10 };
                tbl.AddCell(new PdfPCell(new Phrase("LA IMPRENTA S.A."  + Environment.NewLine +
                   "IMPRENTA Y LIBRERIA " + Environment.NewLine +
                   "El Salvador 689 -(1406) Capital Federal " + Environment.NewLine +
                   "Tel. 4616-1112 / 4639-0048") ) { Border = 0, Rowspan = 3 });

                tbl.AddCell(new PdfPCell(new Phrase("Factura" + Environment.NewLine + "Nº 000001")));
                tbl.AddCell(new PdfPCell(new Phrase("Fecha" + fechaActual.ToString("dd/MM/yyyy"))));
                tbl.AddCell(new PdfPCell(new Phrase("CUIT:45466546  " + Environment.NewLine
                    + " ING BRUTOS:65466663"))
                { Padding = 2});


                document.Add(tbl);

                document.Add(new Paragraph("    " + Chunk.NEWLINE));
          
                //Parte Cliente
                PdfPTable talbleCliente = new PdfPTable(1);
                talbleCliente.WidthPercentage = 100;
                talbleCliente.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell cellCliente = new PdfPCell(new Phrase(" Señores:  " + factura.clientes +
                    Environment.NewLine +
                    " Direccion: " + factura.direccion +
                    Environment.NewLine +
                    " L.V.A: Responsable Inscripto " +
                    Environment.NewLine +
                    " C.U.I.T: " + factura.CUIT));
                cellCliente.Padding = 5;
                cellCliente.BorderWidth = 1f;
                cellCliente.BorderColor = BaseColor.BLACK;
                cellCliente.FixedHeight = 60;

                cellCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                cellCliente.VerticalAlignment = Element.ALIGN_MIDDLE;
                talbleCliente.AddCell(cellCliente);
                document.Add(talbleCliente);
                //Fin Cliente
                //Parte Condicion Venta
                PdfPTable tableCondicion = new PdfPTable(2);
                tableCondicion.WidthPercentage = 100;

                tableCondicion.AddCell("Condiciones de Venta: " + factura.condicionVenta);
                tableCondicion.AddCell("Remito Nº: " + factura.remito);

                document.Add(tableCondicion);

                //Fin Condicion Venta

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                PdfPCell cell1 = new PdfPCell();
               var cel = cell1.Border = Rectangle.NO_BORDER;
                table.AddCell("Codigo");
                table.AddCell("Cantidad");
                table.AddCell("Detalle");
                table.AddCell("P. Unitario");
                table.AddCell("Total $");
                table.AddCell( new PdfPCell(new Phrase(factura.codigo)) { Border = 0 ,FixedHeight = 150});
                table.AddCell( new PdfPCell(new Phrase(Convert.ToString(factura.cantidad))) { Border = 0 });
                table.AddCell(new PdfPCell(new Phrase(factura.detalle)) { Border = 0 });
                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(factura.precioUnitario))) { Border = 0 });
               
                factura.total = factura.precioUnitario * factura.cantidad;
                factura.impuesto = 2;
                factura.Iva = 21;
                var totalImpuesto = factura.total * (factura.impuesto/100);
                var TotalIncImpuesto = factura.total + totalImpuesto;
                var TotalIncIva = TotalIncImpuesto * (factura.Iva/100);
                var TotalFinal = factura.total + TotalIncIva + totalImpuesto;
                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(factura.total))) { Border = 0 });

                document.Add(table);

                document.Add(Chunk.NEWLINE);
                PdfPTable tableSubtotal = new PdfPTable(6); // Tabla con 3 columnas para el detalle
                tableSubtotal.WidthPercentage = 100; // La tabla ocupa el 100% del ancho de la página
         
                tableSubtotal.AddCell("SubTotal");
                tableSubtotal.AddCell("Impuesto");
                tableSubtotal.AddCell("Subtotal");
                tableSubtotal.AddCell("IVA Insc....");
                tableSubtotal.AddCell("Iva No Inscr...");
                tableSubtotal.AddCell("TOTAL");

                tableSubtotal.AddCell(Convert.ToString(factura.total));
                tableSubtotal.AddCell(Convert.ToString(factura.impuesto) + "%");
                tableSubtotal.AddCell(Convert.ToString(TotalIncImpuesto));
                tableSubtotal.AddCell("21%");
                tableSubtotal.AddCell("0");
                tableSubtotal.AddCell(TotalFinal.ToString("0.00"));

                document.Add(tableSubtotal);
                PdfPTable tablePie = new PdfPTable(1); // Tabla con 3 columnas para el detalle
                tablePie.WidthPercentage = 100; // La tabla ocupa el 100% del ancho de la página

                tablePie.AddCell("Codigo de Barra");

                document.Add(tablePie);
                document.Close();
                return File(ms.ToArray(), "application/pdf", "Factura.pdf");
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
        public IActionResult Factura()
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