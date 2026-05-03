using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class DetallePedido
    {
        public long IdDetalle { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public long IdPedido { get; set; }
        public long IdProducto { get; set; }
    }
}