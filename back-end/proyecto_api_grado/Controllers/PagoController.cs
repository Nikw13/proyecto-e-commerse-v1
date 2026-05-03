using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class PagoController : ApiController
    {
        public List<Pago> Get()
        {
            return PagoData.Listar();
        }

        public Pago Get(long id)
        {
            return PagoData.Consultar(id);
        }

        [Route("api/Pago/PorFactura/{idFactura}")]
        public List<Pago> GetPorFactura(long idFactura)
        {
            return PagoData.ListarPorFactura(idFactura);
        }

        public long Post([FromBody] Pago oPago)
        {
            return PagoData.Registrar(oPago);
        }

        [Route("api/Pago/Estado/{id}")]
        public bool PutEstado(long id, [FromBody] string estado)
        {
            return PagoData.ActualizarEstado(id, estado);
        }

        public bool Delete(long id)
        {
            return PagoData.Eliminar(id);
        }
    }
}