using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.FormRecognizer.Models;
using DocuMentor.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocuMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly FormRecognizerClient _formRecognizerClient;
        private readonly DocumentAnalysisClient _documentAnalysisClient;

        private readonly string _modelId; // The ID of your custom model

        public DocumentController(
            IDocumentService documentService, 
            FormRecognizerClient formRecognizerClient, 
            DocumentAnalysisClient documentAnalysisClient)
        {
            _documentService = documentService;
            _formRecognizerClient = formRecognizerClient;
            _documentAnalysisClient = documentAnalysisClient;
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocument([FromForm] IFormFile document)
        {
            if (document == null || document.Length == 0)
            {
                return BadRequest("No document uploaded.");
            }

            var text = await _documentService.ExtractTextAsync(document);
            return Ok(new { Text = text });
        }

        [HttpPost("upload-document-for-analysis")]
        public async Task<IActionResult> UploadDocumentForAnalysis(IFormFile document)
        {
            if (document == null || document.Length == 0)
            {
                return BadRequest("No document uploaded.");
            }

            using (var stream = document.OpenReadStream())
            {
                var options = new RecognizeCustomFormsOptions() { IncludeFieldElements = true };
                var recognizeFormsOperation = await _formRecognizerClient.StartRecognizeIdentityDocumentsAsync( stream);
                await recognizeFormsOperation.WaitForCompletionAsync();
                var formPages = recognizeFormsOperation.Value;

                // Process results
                var result = ProcessFormPages(formPages);
                return Ok(result);
            }
        }

        [HttpPost("analyze-id-document")]
        public async Task<IActionResult> AnalyzeIdDocument(IFormFile document)
        {
            if (document == null || document.Length == 0)
            {
                return BadRequest("No document uploaded.");
            }

            using var stream = document.OpenReadStream();
            AnalyzeDocumentOperation operation = await _documentAnalysisClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", stream);
            AnalyzeResult identityDocuments = operation.Value;

            return Ok(ProcessDocumentFields(identityDocuments));
        }

        private object ProcessDocumentFields(AnalyzeResult identityDocuments)
        {
            var results = new List<object>();
            foreach (var doc in identityDocuments.Documents)
            {
                var fields = new Dictionary<string, object>();

                foreach (var field in doc.Fields)
                {
                    fields.Add(field.Key, new
                    {
                        Value = field.Value.Content,
                        Confidence = field.Value.Confidence
                    });
                }

                results.Add(new
                {
                    ModelId = identityDocuments.ModelId,
                    Fields = fields
                });
            }

            return results;
        }
        private object ProcessFormPages(RecognizedFormCollection formPages)
        {
            var results = new List<object>();

            foreach (var page in formPages)
            {
                var fields = page.Fields.Select(f => new
                {
                    FieldName = f.Key,
                    FieldValue = f.Value.ValueData.Text,
                    Confidence = f.Value.Confidence
                }).ToList();

                results.Add(new
                {
                    FormType = page.FormType,
                    Fields = fields
                });
            }

            return results;
        }
    }
}
