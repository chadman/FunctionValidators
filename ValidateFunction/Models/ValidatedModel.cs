using Azure.Core.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ValidateFunction.Models;

public class ValidatedModel<TModel, V>
    where TModel : class
    where V : AbstractValidator<TModel>, new() {

    public static ValidatedModel<TModel, V> Create(TModel model) {
        var validator = new V();
        var validationResult = validator.Validate(model);

        var result = new ValidatedModel<TModel, V> {
            Result = model,
            Errors = validationResult.Errors,
        };

        return result;
    }

    public TModel Result { get; set; }

    public List<ValidationFailure> Errors { get; set; } = new List<ValidationFailure>();
    public bool IsValid => Errors.Count == 0;

    public async Task<HttpResponseData> CreateValidationResponseAsync(HttpRequestData request) {
        var respone = request.CreateResponse(System.Net.HttpStatusCode.BadRequest);

        var serializerOptions = new JsonSerializerOptions {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        await respone.WriteAsJsonAsync(new {
            errors = Errors.Select(x => new {
                x.PropertyName,
                x.ErrorMessage,
            })
        }, new JsonObjectSerializer(serializerOptions), System.Net.HttpStatusCode.BadRequest);

        return respone;
    }
}