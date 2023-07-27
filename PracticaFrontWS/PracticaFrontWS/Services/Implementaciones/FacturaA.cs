using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using PracticaFrontWS.Models;

namespace PracticaFrontWS.Services.Implementaciones
{
    public class FacturaA
    {
        public static void GenerarFactura(FacturaViewModel factura)
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
                document.Add(new Paragraph("    " + Chunk.NEWLINE));
                var fechaActual = DateTime.Today;
                var tbl = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100, PaddingTop = 10 };
                tbl.AddCell(new PdfPCell(new Phrase("LA IMPRENTA S.A." + Environment.NewLine +
                   "IMPRENTA Y LIBRERIA " + Environment.NewLine +
                   "El Salvador 689 -(1406) Capital Federal " + Environment.NewLine +
                   "Tel. 4616-1112 / 4639-0048"))
                { Border = 0, Rowspan = 3 });

                tbl.AddCell(new PdfPCell(new Phrase("Factura" + Environment.NewLine + "Nº 000001")));
                tbl.AddCell(new PdfPCell(new Phrase("Fecha" + fechaActual.ToString("dd/MM/yyyy"))));
                tbl.AddCell(new PdfPCell(new Phrase("CUIT:45466546  " + Environment.NewLine
                    + " ING BRUTOS:65466663"))
                { Padding = 2 });

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
                table.AddCell(new PdfPCell(new Phrase(factura.codigo)) { Border = 0, FixedHeight = 150 });
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
                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(factura.total))) { Border = 0 });

                document.Add(table);

                document.Add(Chunk.NEWLINE);
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

                PdfPTable tablePie = new PdfPTable(1);
                tablePie.WidthPercentage = 100;

                PdfPCell cellPie = new PdfPCell();

                cellPie.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellPie.HorizontalAlignment = Element.ALIGN_CENTER;

                var ImagenRuta = @"c:\Users\Cristian\Desktop\practicaWS\FrontWS\PracticaFrontWS\PracticaFrontWS\wwwroot\img\codBarra.png";

                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(ImagenRuta);
                imagen.ScaleToFit(100f, 100f);

                Phrase desc = new Phrase("C.A.I. Nº:25064106537080 " + Environment.NewLine +
                      "Fecha de Vto.: 13-06-2024");

                cellPie.AddElement(imagen);
                cellPie.AddElement(desc);

                tablePie.AddCell(cellPie);

                document.Add(tablePie);
                document.Close();
                ms.ToArray();
            }
        }
    }
}
