namespace SpareParts.API.Services.PdfService
{
    public interface IPdfService
    {
        byte[] GeneratePdfFromHtmlString(ReportName reportName, string html);
    }
}
