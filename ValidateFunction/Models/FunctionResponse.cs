namespace ValidateFunction.Models;

public class FunctionResponse<TResponse> : FunctionResponse {
    public FunctionResponse(TResponse payload) {
        Payload = payload;
    }

    /// <summary>
    /// The actual value of the response
    /// </summary>
    public TResponse Payload { get; set; }
}

public class FunctionResponse {
    public static FunctionResponse<TType> Create<TType>(TType input) => new FunctionResponse<TType>(input);
}