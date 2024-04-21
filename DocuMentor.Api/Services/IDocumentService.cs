namespace DocuMentor.Api.Services
{
    public interface IDocumentService
    {
        Task<string> ExtractTextAsync(IFormFile documentFile);
    }
}