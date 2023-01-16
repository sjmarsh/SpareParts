using System.Diagnostics;

namespace SpareParts.API.Services.PdfService
{
    public class LinuxPdfService : IPdfService
    {
        private readonly ILogger<LinuxPdfService> _logger;

        public LinuxPdfService(ILogger<LinuxPdfService> logger)
        {
            _logger = logger;
        }

        public byte[] GeneratePdfFromHtmlString(ReportName reportName, string html)
        {
            byte[] pdfBytes = Array.Empty<byte>();

            string tempHtmlFileName = $"{Guid.NewGuid()}.html";
            File.WriteAllText(tempHtmlFileName, html);

            string tempPdfFileName = $"{Guid.NewGuid()}.pdf";

            // --title {reportName} --footer-left [title] --footer-center [page]/[topage] --footer-right [date] [time]
            // TODO css file path
            var startInfo = new ProcessStartInfo
            {
                FileName = "wkhtmltopdf",
                Arguments = $" -n {tempHtmlFileName} {tempPdfFileName}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = new Process
            {
                StartInfo = startInfo,
            };

            process.OutputDataReceived += (sender, data) =>
            {
                _logger.LogInformation(data.Data);
            };

            process.ErrorDataReceived += (sender, data) =>
            {
                _logger.LogError(data.Data);
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if (File.Exists(tempPdfFileName))
                {
                    pdfBytes = File.ReadAllBytes(tempPdfFileName);
                }
                else
                {
                    _logger.LogWarning($"PDF file was not created at {tempPdfFileName}");
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (File.Exists(tempHtmlFileName))
                {
                    File.Delete(tempHtmlFileName);
                }
                if (File.Exists(tempPdfFileName))
                {
                    File.Delete(tempPdfFileName);
                }
            }

            return pdfBytes;
        }
    }
}
