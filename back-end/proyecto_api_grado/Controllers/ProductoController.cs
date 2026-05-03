using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class ProductoController : ApiController
    {
        public List<Producto> Get()
        {
            return ProductoData.Listar();
        }

        public Producto Get(long id)
        {
            return ProductoData.Consultar(id);
        }

        public long Post([FromBody] Producto oProducto)
        {
            return ProductoData.Registrar(oProducto);
        }

        public bool Put([FromBody] Producto oProducto)
        {
            return ProductoData.Actualizar(oProducto);
        }

        [Route("api/Producto/Stock/{id}")]
        public bool PutStock(long id, [FromBody] int cantidad)
        {
            return ProductoData.ActualizarStock(id, cantidad);
        }

        public bool Delete(long id)
        {
            return ProductoData.Eliminar(id);
        }
    }
}