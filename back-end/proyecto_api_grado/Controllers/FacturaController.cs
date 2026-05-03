using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class FacturaController : ApiController
    {
        public List<Factura> Get()
        {
            return FacturaData.Listar();
        }

        public Factura Get(long id)
        {
            return FacturaData.Consultar(id);
        }

        [Route("api/Factura/PorCliente/{idCliente}")]
        public List<Factura> GetPorCliente(long idCliente)
        {
            return FacturaData.ListarPorCliente(idCliente);
        }

        public long Post([FromBody] Factura oFactura)
        {
            return FacturaData.Registrar(oFactura);
        }

        public bool Put([FromBody] Factura oFactura)
        {
            return FacturaData.Actualizar(oFactura);
        }

        public bool Delete(long id)
        {
            return FacturaData.Eliminar(id);
        }
    }
}