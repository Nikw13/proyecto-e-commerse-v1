using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class PedidoController : ApiController
    {
        public List<Pedido> Get()
        {
            return PedidoData.Listar();
        }

        public Pedido Get(long id)
        {
            return PedidoData.Consultar(id);
        }

        public long Post([FromBody] Pedido oPedido)
        {
            return PedidoData.Registrar(oPedido);
        }

        [Route("api/Pedido/Estado/{id}")]
        public bool PutEstado(long id, [FromBody] string estado)
        {
            return PedidoData.ActualizarEstado(id, estado);
        }

        public bool Put([FromBody] Pedido oPedido)
        {
            return PedidoData.ActualizarEstado(oPedido.IdPedido, oPedido.Estado);
        }

        public bool Delete(long id)
        {
            return PedidoData.Eliminar(id);
        }
    }
}