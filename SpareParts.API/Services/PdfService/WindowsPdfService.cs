using Humanizer;
using OpenHtmlToPdf;

namespace SpareParts.API.Services.PdfService
{
    public class WindowsPdfService : IPdfService
    {
        public byte[] GeneratePdfFromHtmlString(ReportName reportName, string html)
        {
            var reportNameTitle = reportName.ToString().Humanize(LetterCasing.Title);
            // ref: https://github.com/vilppu/OpenHtmlToPdf
            // ref: https://wkhtmltopdf.org/usage/wkhtmltopdf.txt
            var pdf = Pdf.From(html)
                .WithTitle(reportNameTitle)
                .OfSize(PaperSize.A4)
                .Portrait()
                .WithObjectSetting("web.userStyleSheet", "./Reports/reports.css")
                .WithObjectSetting("footer.left", "[title]")
                .WithObjectSetting("footer.center", "[page]/[topage]")
                .WithObjectSetting("footer.right", "[date] [time]")
                .Comressed()
                .Content();

            return pdf;
        }
    }
}
