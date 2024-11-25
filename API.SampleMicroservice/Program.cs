using API.SampleMicroservice.Configuration;
using API.SampleMicroservice.Middlewares;
using API.SampleMicroservice.Profiles;

namespace API.SampleMicroservice
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.ConnectDatabase(builder.Configuration);
			builder.Services.AddHttpContext();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwagger("SampleMicroservice");
			builder.Services.AddAutoMapper(typeof(MappingProfile));
			builder.Services.RegisterValidations();
			builder.Services.RegisterServices();
			builder.Services.RegisterRepositories();
			builder.Services.RegisterMiddleWares();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/V1/swagger.json", "SampleMicroservice WebAPI");
			});

			app.RegisterAPIs();
			app.UseMiddleware<ExceptionMiddleWare>();
			app.Run();
		}
	}
}

