using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class Departamento
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; }
        public string CodigoDane { get; set; }
        public bool Activo { get; set; }
    }
}