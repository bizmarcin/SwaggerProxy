using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Expressions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddXmlSerializerFormatters(); //dodanie obsługi XML

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/getstatus", (int index) =>
{
    var datetime =  DateTime.Now.ToString("yyyy-MM-dd");
    return datetime;
})
.WithName("GetStatus")
.WithOpenApi();

app.MapPost("/postXI", (HttpRequest r) =>
{
    string path = Path.Combine(System.Environment.CurrentDirectory,"SwaggerOutput.txt");
    string createText="";
    var reader = new StreamReader(r.Body);
    var xml = reader.ReadToEndAsync().Result;

    try{
        createText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + xml + "\n";          
    }catch(Exception ex){
        createText=ex.ToString();
    }

    File.AppendAllText(path, createText); 
})
.WithName("PostXI")
.Accepts<HttpRequest>("application/xml","application/json")
.Produces(200)
.ProducesProblem(400)
.ProducesValidationProblem(403)
.WithOpenApi();


app.Run();