
using Microsoft.AspNetCore.Mvc;
using PracticaFrontWS.Core.Session;
using PracticaFrontWS.Models;
using PracticaFrontWS.Services.Interfaces;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Hosting.Server;
using PracticaFrontWS.Services.Implementaciones;

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
            //FacturaA.GenerarFactura(factura);
            //return View();
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                Random random = new Random();

                int numeroAleatorio = random.Next(10000, 100000);
                var fechaActual = DateTime.Today;
                Font fontTitulo = new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD);
                Font fontSubTitulo = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
                Font fontDireccion = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);
                Font font = new Font(Font.FontFamily.HELVETICA, 26, Font.BOLD);

                Font fontcod = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);
                var tbl = new PdfPTable(new float[] { 30f,10f ,2f,35f }) { WidthPercentage = 100, PaddingTop = 0,SpacingBefore = -10};

                tbl.AddCell(new PdfPCell(new Phrase("LA IMPRENTA S.A.", fontTitulo)) { Border = 0 , HorizontalAlignment = Element.ALIGN_CENTER});
                tbl.AddCell(new PdfPCell(new Phrase("A", font )){ Rowspan =2, VerticalAlignment =Element.ALIGN_MIDDLE,HorizontalAlignment = Element.ALIGN_CENTER });
               
                tbl.AddCell(new PdfPCell(new Phrase("")) { Border = 0 , Rowspan= 3});
                tbl.AddCell(new PdfPCell(new Phrase("Factura" + Environment.NewLine + "Nº" + numeroAleatorio)));
                tbl.AddCell(new PdfPCell(new Phrase("IMPRENTA Y LIBRERIA", fontSubTitulo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                tbl.AddCell(new PdfPCell(new Phrase("Fecha: " + fechaActual.ToString("dd/MM/yyyy"))));

                tbl.AddCell(new PdfPCell(new Phrase("El Salvador 689 - (1406) Capital Federal " + Environment.NewLine + "Tel. 4616-1112 / 4639-0048", fontDireccion)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                tbl.AddCell(new PdfPCell(new Phrase("Codigo Nº 01",fontcod)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER});
                tbl.AddCell(new PdfPCell(new Phrase("CUIT:30-68914583-0 " + Environment.NewLine + "INGRESOS BRUTOS:CM. 901-11111-0" + Environment.NewLine + "Inicio de Actividades:01/04/1994")));

                document.Add(tbl);

                document.Add(new Paragraph("             IVA:Responsable Inscripto",fontDireccion));

                //Parte Cliente
                PdfPTable talbleCliente = new PdfPTable(1);
                talbleCliente.WidthPercentage = 100;
                talbleCliente.DefaultCell.Border = Rectangle.NO_BORDER;
                talbleCliente.SpacingBefore = 5f;//espacio antes
                talbleCliente.SpacingAfter = 5f;//espacio despues
                PdfPCell cellCliente = new PdfPCell(new Phrase(" Señores:  " + factura.clientes +
                    Environment.NewLine +
                    " Direccion: " + factura.direccion +
                    Environment.NewLine +
                    " L.V.A: Responsable Inscripto                                       " + " C.U.I.T: " + factura.CUIT));

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
                tableCondicion.SpacingAfter = 10f;
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
                table.AddCell(new PdfPCell(new Phrase(factura.codigo)) {BorderWidthRight = 0,BorderWidthBottom = 0, FixedHeight = 300 });
                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(factura.cantidad))) { Border = 0 });
                table.AddCell(new PdfPCell(new Phrase(factura.detalle)) { Border = 0 });
                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(factura.precioUnitario))) { Border = 0 });

                factura.total = factura.precioUnitario * factura.cantidad;
                factura.impuesto = 2;
                factura.Iva = 21;
                var totalImpuesto = factura.total * (factura.impuesto / 100);
                var TotalIncImpuesto = factura.total + totalImpuesto;
                var TotalIncIva = TotalIncImpuesto * (factura.Iva / 100);
                var TotalFinal = factura.total + TotalIncIva + totalImpuesto;
                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(factura.total))) { BorderWidthLeft = 0, BorderWidthBottom = 0, FixedHeight = 300 });

                document.Add(table);

                PdfPTable tableSubtotal = new PdfPTable(6);
                tableSubtotal.WidthPercentage = 100;

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

                PdfPTable tablePie = new PdfPTable(2);
                tablePie.WidthPercentage = 100;

                PdfPCell cellPie = new PdfPCell();
                cellPie.BorderWidthRight = 0;
                cellPie.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellPie.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cellPie2 = new PdfPCell();
                cellPie2.BorderWidthLeft = 0;
                cellPie2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellPie2.HorizontalAlignment = Element.ALIGN_MIDDLE;

                BarcodeQRCode barcodeQRCode = new BarcodeQRCode(Convert.ToString(numeroAleatorio), 100, 100, null);
                Image codeQRImage = barcodeQRCode.GetImage();
                codeQRImage.ScaleAbsolute(100, 100);

                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(codeQRImage);
                imagen.ScaleToFit(100f, 100f);
                Phrase desc = new Phrase("            C.A.I. Nº:25064106537080 " + Environment.NewLine +
                      "            Fecha de Vto.: 13-06-2024");
                cellPie.AddElement(imagen);
                cellPie2.AddElement(desc);

                tablePie.AddCell(cellPie);
                tablePie.AddCell(cellPie2);

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
        public IActionResult CrearZonas(ZonasViewModel nuevo)
        {
            _service.RegistrarZona(nuevo);
            return RedirectToAction(nameof(Index));
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