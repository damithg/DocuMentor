# DocuMentor API

DocuMentor API provides advanced document analysis capabilities by integrating Azure's Form Recognizer and Document Analysis services. It allows users to upload documents for  identity document analysis.

## Features

- **Text Extraction**: Extract plain text from uploaded documents.
- **Identity Document Analysis**: Extract and analyze information from identity documents.

## Getting Started

### Prerequisites

- .NET 7.0 SDK or later
- Azure subscription and resources:
  - Azure Form Recognizer resource
  - Azure Cognitive Services resource

### Setting Up Your Environment

Clone the repository:
```bash
  git clone https://yourrepositoryurl.com/DocuMentor.git
```
Navigate to the project directory:
```console
  cd DocuMentor
 ```
Install necessary packages:
```
  dotnet restore
```

### Configuration
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "CognitiveServices": {
    "Endpoint": "YOUR_COGNITIVE_SERVICES_ENDPOINT",
    "Key": "YOUR_COGNITIVE_SERVICES_KEY"
  },
  "FormRecognizer": {
    "Endpoint": "YOUR_FORM_RECOGNIZER_ENDPOINT",
    "ApiKey": "YOUR_FORM_RECOGNIZER_API_KEY",
    "ModelId": "prebuilt-idDocument"
  },
  "AllowedHosts": "*"
}
```

### Detailed Configuration Parameters
Logging: Configures the level of logging for the application and for ASP.NET Core framework activities.
CognitiveServices:
Endpoint: The base URL for your Azure Cognitive Services resource.
Key: The subscription key for your Cognitive Services resource.
FormRecognizer:
Endpoint: The endpoint URL for your Azure Form Recognizer resource.
ApiKey: The API key for your Form Recognizer resource.
ModelId: The model identifier for the specific model you are using; default is set to prebuilt-idDocument for using prebuilt ID document analysis.

### Usage

Running the API
Execute the following command to run the API:
```
dotnet run
```
The API will be available at http://localhost:5000/api.

### Endpoints
#### POST /api/Document/upload-document
Uploads a document and extracts plain text.
##### Form Parameters:
document: The document file.
#### POST /api/Document/upload-document-for-analysis
Uploads a document and performs custom form recognition based on a trained model.
##### Form Parameters:
document: The document file.
#### POST /api/Document/analyze-id-document
Analyzes an uploaded identity document using a prebuilt model.
##### Form Parameters:
document: The identity document file.


### Example Request
```
curl -X POST -F "document=@path_to_your_document.pdf" http://localhost:5000/api/Document/upload-document
```

### Contributing
Contributions are welcome. Please fork the repository and submit a pull request with your features or fixes.

### License
This project is licensed under the MIT License.


