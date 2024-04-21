using Azure.AI.FormRecognizer;
using Azure;
using DocuMentor.Api.Services;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Azure.AI.FormRecognizer.DocumentAnalysis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Azure Cognitive Services
builder.Services.AddSingleton<IComputerVisionClient, ComputerVisionClient>(provider => new ComputerVisionClient(new ApiKeyServiceClientCredentials(builder.Configuration["CognitiveServices:Key"]))
{
    Endpoint = builder.Configuration["CognitiveServices:Endpoint"]
});

builder.Services.AddSingleton<IDocumentService, DocumentService>();

// Configure FormRecognizerClient
builder.Services.AddSingleton(new FormRecognizerClient(new Uri(builder.Configuration["FormRecognizer:Endpoint"]), new AzureKeyCredential(builder.Configuration["FormRecognizer:ApiKey"])));
builder.Services.AddSingleton(builder.Configuration["FormRecognizer:ModelId"]);

// Configure Document Analysis Client
var credential = new AzureKeyCredential(builder.Configuration["FormRecognizer:ApiKey"]);
var client = new DocumentAnalysisClient(new Uri(builder.Configuration["FormRecognizer:Endpoint"]), credential);
builder.Services.AddSingleton(client);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
