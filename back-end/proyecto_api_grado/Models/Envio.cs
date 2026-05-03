using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class Envio
    {
        public long IdEnvio { get; set; }
        public string NumeroGuia { get; set; }
        public string EmpresaTransporte { get; set; }
        public string MetodoEnvio { get; set; }
        public decimal CostoEnvio { get; set; }
        public string EstadoEnvio { get; set; }
        public DateTime? FechaDespacho { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime CreatedAt { get; set; }
        public long IdPedido { get; set; }
    }
}