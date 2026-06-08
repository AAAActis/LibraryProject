using System;
using Presentation.Middleware;


namespace Presentation.Middleware
{
    public class ApiError
    {
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public int statusCode { get; set; }
    }
}