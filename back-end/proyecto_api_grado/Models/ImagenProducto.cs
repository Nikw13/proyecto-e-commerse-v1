using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class ImagenProducto
    {
        public int IdImagen { get; set; }
        public string UrlImagen { get; set; }
        public bool EsPrincipal { get; set; }
        public byte Orden { get; set; }
        public long IdProducto { get; set; }
    }
}