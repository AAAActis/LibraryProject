namespace Libreria1.Application.DTOs;

/// <summary>
/// DTO con las métricas agregadas que muestra el dashboard del frontend.
/// </summary>
public class DashboardStatsDto
{
    public int TotalLibros { get; set; }
    public int LibrosDisponibles { get; set; }
    public int PrestamosActivos { get; set; }
    public int MultasPendientes { get; set; }
}
