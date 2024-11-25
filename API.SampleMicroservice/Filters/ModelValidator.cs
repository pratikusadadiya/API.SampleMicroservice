using API.SampleMicroservice.DataModels.Shared;
using FluentValidation;
using System.Net;

namespace API.SampleMicroservice.Filters
{
    public class ModelValidator<TModel>(IValidator<TModel> validator) : IEndpointFilter where TModel : class
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            var contextObj = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(TModel));

            if (contextObj == null)
            {
                return Results.BadRequest(response);
            }

            var result = await validator.ValidateAsync((TModel)contextObj);

            if (!result.IsValid)
            {
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    response.ErrorMessages.Add(result.Errors[0].ToString());
                }
                
                return Results.BadRequest(response);
            }

            return await next(context);
        }
    }
}
