using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class DetallePedidoController : ApiController
    {
        [Route("api/DetallePedido/PorPedido/{idPedido}")]
        public List<DetallePedido> GetPorPedido(long idPedido)
        {
            return DetallePedidoData.ListarPorPedido(idPedido);
        }

        public bool Post([FromBody] DetallePedido oDetalle)
        {
            return DetallePedidoData.AgregarItem(oDetalle.IdPedido, oDetalle.IdProducto, oDetalle.Cantidad);
        }

        public bool Delete(long id)
        {
            return DetallePedidoData.EliminarItem(id);
        }
    }
}