using OnlineShopFinal.PdfProvider.DataModel;

namespace OnlineShopFinal.PdfProvider
{
    public interface IPdfSharpService
    {
        string CreatePdf(PdfData pdfData);
    }
}
