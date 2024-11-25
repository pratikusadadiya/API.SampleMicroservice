using API.SampleMicroservice.Constants;
using API.SampleMicroservice.DataModels.Shared;
using API.SampleMicroservice.Entities;
using API.SampleMicroservice.Exceptions;
using API.SampleMicroservice.Resources;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Security.Claims;
using System.Text.Json;

namespace API.SampleMicroservice.Middlewares
{
    public class ExceptionMiddleWare(SampleMicroserviceContext dbContext, ClaimsPrincipal principal) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            ApiResponse errorResponse = new()
            {
                IsSuccess = false
            };
            switch (ex)
            {
                case NotFoundException _:
                    errorResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    errorResponse.ErrorMessages.Add(ex.Message);
                    break;
                case BadRequestException _:
                case EntityNullException _:
                    errorResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    errorResponse.ErrorMessages.Add(ex.Message);
                    break;
                case UnauthorizedException _:
                    errorResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    errorResponse.ErrorMessages.Add(ex.Message);
                    break;
                case DbUpdateException _:
                    NpgsqlException? postgresExceptions = ((DbUpdateException)ex).InnerException as NpgsqlException;
                    if (postgresExceptions != null && postgresExceptions.SqlState == SystemConstants.DuplicateRecordCode)
                    {
                        errorResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
                        errorResponse.ErrorMessages.Add(Messages.RecordAlreadyExists);
                    }
                    else if (postgresExceptions != null && postgresExceptions.SqlState == SystemConstants.ForeignKeyViolation)
                    {
                        errorResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
                        errorResponse.ErrorMessages.Add(Messages.RecordUsedInSystem);
                    }
                    else
                    {
                        errorResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
                        errorResponse.ErrorMessages.Add(Messages.GenericErrorMessage);
                    }
                    break;
                case DuplicateRecordException _:
                    errorResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
                    errorResponse.ErrorMessages.Add(ex.Message);
                    break;
                case NotEditableException _:
                    errorResponse.StatusCode = System.Net.HttpStatusCode.PartialContent;
                    errorResponse.ErrorMessages.Add(ex.Message);
                    break;
                case ForbiddenException _:
                    errorResponse.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    errorResponse.ErrorMessages.Add(ex.Message);
                    break;
                default:
                    errorResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    errorResponse.ErrorMessages.Add(ex.Message);
                    break;
            }

            // Unchange all entities
            if (errorResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                dbContext.ChangeTracker.Entries().ToList().ForEach(entity => entity.State = EntityState.Detached);
                await dbContext.SaveChangesAsync();
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)errorResponse.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}