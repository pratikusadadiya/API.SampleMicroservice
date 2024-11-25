using API.SampleMicroservice.DataModels.Shared;
using API.SampleMicroservice.Resources;
using System.Net;

namespace API.SampleMicroservice.Filters
{
    public class IdValidator : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(int)) as int?;
            if (id == null || id == 0)
            {
                ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
                response.ErrorMessages.Add(Messages.IdValidationMessage);
                return Results.BadRequest(response);
            }
            return await next(context);
        }
    }
}
