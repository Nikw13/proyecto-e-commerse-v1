using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class ImagenProductoController : ApiController
    {
        [Route("api/ImagenProducto/PorProducto/{idProducto}")]
        public List<ImagenProducto> GetPorProducto(long idProducto)
        {
            return ImagenProductoData.ListarPorProducto(idProducto);
        }

        public bool Post([FromBody] ImagenProducto oImagen)
        {
            return ImagenProductoData.Registrar(oImagen);
        }

        public bool Delete(int id)
        {
            return ImagenProductoData.Eliminar(id);
        }
    }
}