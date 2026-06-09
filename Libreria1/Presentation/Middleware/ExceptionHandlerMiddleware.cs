using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using Libreria1.Domain.Exceptions;
using Presentation.Middleware;

namespace Libreria1.Presentation.Middleware;
public class ExceptionHandlerMiddLeware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddLeware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //aca se deja que la peticion siga su curso normal hacia el controlador
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        //mapeo limpio usando pattern matching
        var(statusCode, mensaje) = exception switch
        {
            LibroNoEncontradoException e => (StatusCodes.Status404NotFound, e.Message), 
            LibroNoDisponibleException e => (StatusCodes.Status409Conflict, e.Message),
            _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado.")
        };

        context.Response.StatusCode = statusCode;

        var apiError = new ApiError
        {
            statusCode = statusCode,
            Mensaje = mensaje
        };

        //serializamos el error a json 
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(apiError, options);

        return context.Response.WriteAsync(result);
    } 

}