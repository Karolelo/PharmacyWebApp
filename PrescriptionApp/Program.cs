using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Context;
using PrescriptionApp.Middleware;
using PrescriptionApp.Repositories;
using PrescriptionApp.Service;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PharmacyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddScoped<IPharmacyRepository, PharmacyRepository>();
builder.Services.AddScoped<IPharmacyService, PharmacyService>();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CustomExceptionHandler>();
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthorization();
app.Run();