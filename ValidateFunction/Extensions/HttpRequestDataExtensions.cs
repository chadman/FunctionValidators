using FluentValidation;
using ValidateFunction.Models;

namespace Microsoft.Azure.Functions.Worker.Http;
public static class HttpRequestDataExtensions {
    public static async Task<ValidatedModel<TModel, V>> ReadJsonBodyAsync<TModel, V>(this HttpRequestData data)
        where V : AbstractValidator<TModel>, new()
        where TModel : class {
        var model = await data.ReadFromJsonAsync<TModel>();

        return ValidatedModel<TModel, V>.Create(model);
    }
}
