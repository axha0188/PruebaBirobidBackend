using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PruebaBirobidBackend.Business;
using PruebaBirobidBackend.Models;

var builder = WebApplication.CreateBuilder(args);


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// ConfigureServices: Agregar servicios necesarios
builder.Services.AddCors(options =>
{
    options.AddPolicy("PolicyADM",
        policy =>
        {
            policy.WithOrigins("http://localhost:7212", "*").WithMethods("POST", "PUT", "DELETE", "GET");
        });
});

var app = builder.Build();
// Configurar políticas CORS
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure para manejar solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/listarcliente", async () => {
    var clientes = new Cliente();
    var listaClientes = await clientes.ListarCliente();
    return listaClientes;
});

app.MapPost("/api/grabarcliente", async ([FromBody] ClienteModel cliente) => {
    var clientes = new Cliente();
    var res = await clientes.GrabarCliente(cliente);
    return res;
});

app.MapPost("/api/editarcliente", async ([FromBody] ClienteModel cliente) => {
    var clientes = new Cliente();
    var res = await clientes.EditarCliente(cliente);
    return res;
});

app.MapPost("/api/eliminarcliente", async ([FromBody] ClienteModel cliente) => {
    var clientes = new Cliente();
    var res = await clientes.EliminarCliente(cliente);
    return res;
});

// Configure para ejecutar la aplicación
app.Run();
