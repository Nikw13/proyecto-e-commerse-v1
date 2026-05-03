using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class Pedido
    {
        public long IdPedido { get; set; }
        public string Estado { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long IdCliente { get; set; }
        public long IdDireccion { get; set; }
    }
}