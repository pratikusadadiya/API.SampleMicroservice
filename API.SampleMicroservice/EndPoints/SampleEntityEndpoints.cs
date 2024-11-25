using API.SampleMicroservice.DataModels.Request;
using API.SampleMicroservice.DataModels.Shared;
using API.SampleMicroservice.Filters;
using API.SampleMicroservice.Interfaces.Services;
using API.SampleMicroservice.Resources;
using Shared.Helpers;
using System.Net;

namespace API.SampleMicroservice.Endpoints
{
    public static class SampleEntityEndpoints
    {
        public static void RegisterSampleEntityAPIs(this IEndpointRouteBuilder app)
        {
            var sampleEntityApiGroup = app.MapGroup("api/sample-entity");

            sampleEntityApiGroup.MapPost("/search", GetAll)
                .Produces<ApiResponse>((int)HttpStatusCode.OK)
                .Accepts<SampleEntitySearchPageRequest>("application/json");

            sampleEntityApiGroup.MapGet("/{id:int}", GetById)
                .Produces<ApiResponse>((int)HttpStatusCode.OK)
                .AddEndpointFilter<IdValidator>();

            sampleEntityApiGroup.MapPost("/", Create)
                .Accepts<SampleEntityCreateUpdateCommand>("application/json")
                .Produces<ApiResponse>((int)HttpStatusCode.OK)
                .Produces((int)HttpStatusCode.BadRequest)
                .AddEndpointFilter<ModelValidator<SampleEntityCreateUpdateCommand>>();

            sampleEntityApiGroup.MapPut("/{id:int}", Update)
                .Accepts<SampleEntityCreateUpdateCommand>("application/json")
                .Produces<ApiResponse>((int)HttpStatusCode.OK)
                .Produces((int)HttpStatusCode.BadRequest)
                .AddEndpointFilter<IdValidator>()
                .AddEndpointFilter<ModelValidator<SampleEntityCreateUpdateCommand>>();

            sampleEntityApiGroup.MapPut("/{id:int}/{status:bool}", UpdateStatus)
                .Produces<ApiResponse>((int)HttpStatusCode.OK)
                .Produces((int)HttpStatusCode.BadRequest);
        }

        private async static Task<IResult> GetAll(SampleEntitySearchPageRequest request, ISampleEntityService _service)
        {
            var result = await _service.GetSampleEntityAsync(request);
            return Results.Ok(ResponseHelper.SuccessResponse(result));
        }

        private async static Task<IResult> GetById(int id, ISampleEntityService _service)
        {
            var result = await _service.GetSampleEntityByIdAsync(id);
            return Results.Ok(ResponseHelper.SuccessResponse(result));
        }

        private async static Task<IResult> Create(SampleEntityCreateUpdateCommand dto, ISampleEntityService _service)
        {
            var result = await _service.CreateSampleEntityAsync(dto);
            return Results.Ok(ResponseHelper.CreateResponse(result, string.Format(Messages.CreatedSuccessfully, Messages.SampleEntity)));
        }

        private async static Task<IResult> Update(int id, SampleEntityCreateUpdateCommand dto, ISampleEntityService _service)
        {
            var result = await _service.UpdateSampleEntityAsync(id, dto);
            return Results.Ok(ResponseHelper.SuccessResponse(result, string.Format(Messages.UpdatedSuccessfully, Messages.SampleEntity)));
        }

        private async static Task<IResult> UpdateStatus(int id, bool status, ISampleEntityService _service)
        {
            var result = await _service.UpdateStatusAsync(id, status);
            return Results.Ok(ResponseHelper.SuccessResponse(result, string.Format(Messages.UpdatedSuccessfully, Messages.SampleEntity)));
        }

        private async static Task<IResult> Delete(int id, ISampleEntityService _service)
        {
            await _service.RemoveAsync(id);
            return Results.Ok(ResponseHelper.SuccessResponse(new(), string.Format(Messages.DeletedSuccessfully, Messages.SampleEntity)));
        }
    }
}
