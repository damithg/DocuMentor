using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System.ComponentModel.DataAnnotations;

namespace DocuMentor.Api.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IComputerVisionClient _computerVisionClient;

        public DocumentService(IComputerVisionClient computerVisionClient)
        {
            _computerVisionClient = computerVisionClient;
        }

        public async Task<string> ExtractTextAsync(IFormFile document)
        {
            // Use OCR to extract text
            // Validate against predefined criteria
            // Return results
            using (var stream = document.OpenReadStream())
            {
                var result = await _computerVisionClient.RecognizePrintedTextInStreamAsync(true, stream);
                return result.Regions.SelectMany(region => region.Lines)
                                     .SelectMany(line => line.Words)
                                     .Select(word => word.Text)
                                     .Aggregate((current, next) => $"{current} {next}");
            }
        }
    }
}
