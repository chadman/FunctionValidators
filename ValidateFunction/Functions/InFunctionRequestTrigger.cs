using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ValidateFunction.Models;

namespace ValidateFunction.Functions;

public class InFunctionRequestTrigger {

    private readonly ILogger _logger;

    public InFunctionRequestTrigger(ILoggerFactory loggerFactory) {
        _logger = loggerFactory.CreateLogger<InFunctionRequestTrigger>();
    }

    [Function(nameof(InFunctionRequestTrigger))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "in-function")] HttpRequestData req, Person person) {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var model = await req.ReadJsonBodyAsync<Person, PersonValidator>();

        if (!model.IsValid) {
            return await model.CreateValidationResponseAsync(req);
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }

    public async Task<HttpResponseData> CreateJsonResponseAsync(HttpRequestData request, object returnData, HttpStatusCode statusCode = HttpStatusCode.OK) {
        var respone = request.CreateResponse(statusCode);

        var serializerOptions = new JsonSerializerOptions {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        await respone.WriteAsJsonAsync(returnData, new JsonObjectSerializer(serializerOptions), statusCode);

        return respone;
    }
}
