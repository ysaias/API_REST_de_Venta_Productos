namespace begywebsapi.DTOs
{
    public class ProductoFiltrarDto
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        public PaginacionDTO PaginacionDTO
        {

            get { return new PaginacionDTO() { Pagina = Pagina, RecordsPorPagina = RecordsPorPagina }; }
        }
        public string? Nombre { get; set; }
        public int CategoriaId { get; set; }

        public int? Precio { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; } = DateTime.Today; // Filtrar hasta hoy por defecto
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
    }
}
