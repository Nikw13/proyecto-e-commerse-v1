using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class DireccionCliente
    {
        public long IdDireccion { get; set; }
        public string Descripcion { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }
        public long IdCliente { get; set; }
        public int IdCodigoPostal { get; set; }
    }
}