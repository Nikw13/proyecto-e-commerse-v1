using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class Pago
    {
        public long IdPago { get; set; }
        public string MetodoPago { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public string Referencia_externa { get; set; }
        public DateTime FechaPago { get; set; }
        public long IdFactura { get; set; }
    }
}