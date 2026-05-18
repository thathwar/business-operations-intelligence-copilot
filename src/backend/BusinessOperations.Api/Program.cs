using BusinessOperations.Api.Application;
using BusinessOperations.Api.Application.Agents;
using BusinessOperations.Api.Application.AI;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();// Add services to the container.
builder.Services.AddSingleton<IncidentService>();
builder.Services.AddScoped<IIncidentAgent, RootCauseAgent>();
builder.Services.AddScoped<IIncidentAgent, RootCauseAgent>();
builder.Services.AddScoped<IIncidentAgent, BusinessImpactAgent>();
builder.Services.AddScoped<IIncidentAgent, RecommendationAgent>();
builder.Services.AddSingleton<OpenAiService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors("Frontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
