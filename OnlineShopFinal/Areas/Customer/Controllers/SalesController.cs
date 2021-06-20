using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using OnlineShopFinal.PdfProvider;
using OnlineShopFinal.PdfProvider.DataModel;

using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using OnlineShopFinal.Utility;
using OnlineShopFinal.ViewModel;

namespace OnlineShopFinal.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SalesController : Controller
    {
        private readonly IPdfSharpService _pdfService;

        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _db;
        public SalesController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var customerInvoiceDetails = (from x in _db.OrderDetails where (x.Date > DateTime.Today) select x).Include(c => c.Product).ToList();

            //var customerInvoiceDetails = _db.OrderDetails.Include(c => c.Product).ToList();
            return View(customerInvoiceDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Index(DateTime startDate, DateTime endDate, String Search, DateTime date1, DateTime date2, DateTime d1, DateTime d2)
        {
            if (Search == "Search")
            {
                var Date1 = startDate;
                var Date2 = endDate;
                ViewData["startDate"] = Date1;
                ViewData["endDate"] = Date2;
                return View((from x in _db.OrderDetails where (x.Date >= startDate) && (x.Date <= endDate) select x).Include(c => c.Product).ToList());

            }
            else
            {

                try
                {
                    int id =1;
                    var Date1 = date1;
                    var Date2 = date2;
                    ViewData["startDate"] = Date1;
                    ViewData["endDate"] = Date2;
                    List<OrderDetails> result = new List<OrderDetails>();
                    if (date1 == d1 && date2 == d2)
                    {
                        result = (from x in _db.OrderDetails where (x.Date > DateTime.Today) select x).Include(c => c.Product).ToList();


                    }
                    else
                    {
                         result = (from x in _db.OrderDetails where (x.Date >= startDate) && (x.Date <= endDate) select x).Include(c => c.Product).ToList();

                    }


                    var data = new PdfData
                    {
                        DocumentTitle = "Title of the MigraDoc",
                        DocumentName = "MigraDocDocName",
                        CreatedBy = "Damien",
                        Description = "some data description which I have, and want to display in the PDF file..., This is another text, what is happening here, why is this text display...",
                        DisplayListItems = new List<ItemsToDisplay>
                {
                    new ItemsToDisplay{ Id = "Print Servers", Data1= "some data", Data2 = "more data to display"},
                    new ItemsToDisplay{ Id = "Network Stuff", Data1= "IP4", Data2 = "any left"},
                    new ItemsToDisplay{ Id = "Job details", Data1= "too many", Data2 = "say no"},
                    new ItemsToDisplay{ Id = "Firewall", Data1= "what", Data2 = "Let's burn it"}

                }
                    };


                    var path = CreateMigraDocPdf(data, Date1, Date2, d1, d2);
                    var path1 = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "PdfFiles/pdf" + id + ".pdf");
                    //var stream = new FileStream(path, FileMode.Open);
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, "application/pdf", Path.GetFileName(path1));
                }
                catch
                {
                    TempData["Success"] = "Try again!";

                    return View((from x in _db.OrderDetails where (x.Date >= startDate) && (x.Date <= endDate) select x).Include(c => c.Product).ToList());

                }


                return View((from x in _db.OrderDetails where (x.Date >= startDate) && (x.Date <= endDate) select x).Include(c => c.Product).ToList());
            }
        }
        private readonly string _createdDocsPath = ".\\PdfProvider\\Created";
        public string CreateMigraDocPdf(PdfData pdfData , DateTime date1, DateTime date2, DateTime d10, DateTime d20)
        {
            var d1 = date1;
            var d2 = date2;
            var date10 = d10;
            var date20 = d20;
            // Create a MigraDoc document
            Document document = CreateDocument(pdfData, d1, d2, date10, date20);
            string mdddlName = $"{_createdDocsPath}/{pdfData.DocumentName}-{DateTime.UtcNow.ToOADate()}.mdddl";
            string docName = $"{_createdDocsPath}/{pdfData.DocumentName}-{DateTime.UtcNow.ToOADate()}.pdf";

            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, mdddlName);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(docName);

            return docName;
        }
        private Document CreateDocument(PdfData pdfData, DateTime d1, DateTime d2, DateTime date10, DateTime date20)
        {
            var date1 = d1;
            var date2 = d2;
            var date100 = date10;
            var date200 = date20;
            // Create a new MigraDoc document
            Document document = new Document();
            document.Info.Title = pdfData.DocumentTitle;
            document.Info.Subject = pdfData.Description;
            document.Info.Author = pdfData.CreatedBy;

            DefineStyles(document);
            CreatePage(document);
            DemonstrateSimpleTable(document, date1, date2, date100, date200);
            DemonstrateTableAlignment(document);
            //FillContent(document);

            return document;
        }
        private void DefineStyles(Document document)
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }
        private void CreatePage(Document document)
        {

            // Each MigraDoc document needs at least one section.
            Section section = document.AddSection();




            // Put a logo in the header
            //string imageFilePath = System.Web.HttpContext.Current.Server.MapPath("..") + "/images/graphics/logo.png";

            //Image image = section.Headers.Primary.AddImage(imageFilePath);
            //Image image = section.Headers.Primary.AddImage("/img.png");
            //Image image = section.AddImage("C:\\inetpub\\wwwroot\\site\\logo.png");
            //image.Height = "2.5cm";
            //image.LockAspectRatio = true;
            //image.RelativeVertical = RelativeVertical.Line;
            //image.RelativeHorizontal = RelativeHorizontal.Margin;
            //image.Top = ShapePosition.Top;
            //image.Left = ShapePosition.Right;
            //image.WrapFormat.Style = WrapStyle.Through;



            Paragraph paragraph = section.Headers.Primary.AddParagraph();
            //paragraph.Format.SpaceBefore = "5cm";
            //paragraph.Style = "Reference";
            //paragraph.AddFormattedText("LOGO", TextFormat.Bold);
            //paragraph.Format.Font.Name = "Times New Roman";
            //paragraph.Format.Font.Size = 20;
            paragraph = section.AddParagraph("Swapnochura\n");
            paragraph.Format.Font.Size = 20;
            paragraph.Format.Font.Color = Colors.DarkRed;
            paragraph.Format.SpaceBefore = "0.5cm";
            paragraph.Format.SpaceAfter = "0.5cm";
            paragraph = section.AddParagraph("123 Main Street, New York, NY 10030");
            paragraph = section.AddParagraph(" Date: ");
            paragraph.AddDateField("dd.MM.yyyy");

            // Create footer
            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Swapnochura Inc · Sample Street 42 · 56789 Cologne · New York");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the text frame for the address
            TextFrame addressFrame = section.AddTextFrame();

            addressFrame.Height = "3.0cm";
            addressFrame.Width = "7.0cm";
            addressFrame.Left = ShapePosition.Left;
            addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            addressFrame.Top = "5.0cm";
            addressFrame.RelativeVertical = RelativeVertical.Page;

            //// Put sender in address frame
            //paragraph = addressFrame.AddParagraph("PowerBooks Inc · Sample Street 42 · 56789 Cologne");
            //paragraph.Format.Font.Name = "Times New Roman";
            //paragraph.Format.Font.Size = 7;
            //paragraph.Format.SpaceAfter = 3;

            // Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("INVOICE", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddText("Cologne, ");
            paragraph.AddDateField("dd.MM.yyyy");
        }
        private void DemonstrateTableAlignment(Document document)
        {
            document.LastSection.AddParagraph();

            Table table = new Table();
            table.Format.Alignment = ParagraphAlignment.Center;
            table.Borders.Width = 0.75;
            table.Borders.Visible = false;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Column column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
           
            
           

            document.LastSection.Add(table);

        }
        public void DemonstrateSimpleTable(Document document, DateTime startDate, DateTime endDate, DateTime d1, DateTime d2)
        {
            Table table = document.LastSection.AddTable();
            table.Borders.Visible = true;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Column column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.VerticalAlignment = VerticalAlignment.Top;
            List<OrderDetails> result = new List<OrderDetails>();
            if(startDate == d1 && endDate == d2)
            {
                result = (from x in _db.OrderDetails where (x.Date > DateTime.Today) select x).Include(c => c.Product).ToList();

            }
            else
            {
                result = (from x in _db.OrderDetails where (x.Date >= startDate) && (x.Date <= endDate) select x).Include(c => c.Product).ToList();

            }
            row.Cells[0].AddParagraph("Name");
            row.Cells[1].AddParagraph("Date");
            row.Cells[2].AddParagraph("Price");
            row.Cells[3].AddParagraph("quantity");
            row.Cells[4].AddParagraph("SubTotal");


            decimal subtotal=0;
            foreach (var product in result)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(product.Product.Name.ToString());
                row.Cells[1].AddParagraph(product.Date.ToString());
                row.Cells[2].AddParagraph(product.Price.ToString());
                row.Cells[3].AddParagraph(product.Quantity.ToString());
                row.Cells[4].AddParagraph((product.Price*product.Quantity).ToString());
                subtotal = subtotal + (product.Price * product.Quantity);
            }
            row = table.AddRow();
            row.Borders.Visible = false;
            row.Cells[4].AddParagraph(subtotal.ToString());

        }



    }
}