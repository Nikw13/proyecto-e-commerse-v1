using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class DireccionClienteController : ApiController
    {
        public List<DireccionCliente> Get()
        {
            return DireccionClienteData.ListarPorCliente(0);
        }

        [Route("api/DireccionCliente/{idCliente}")]
        public List<DireccionCliente> GetPorCliente(long idCliente)
        {
            return DireccionClienteData.ListarPorCliente(idCliente);
        }

        public DireccionCliente Get(long id)
        {
            return DireccionClienteData.Consultar(id);
        }

        public long Post([FromBody] DireccionCliente oDireccion)
        {
            return DireccionClienteData.Registrar(oDireccion);
        }

        public bool Put([FromBody] DireccionCliente oDireccion)
        {
            return DireccionClienteData.Actualizar(oDireccion);
        }

        public bool Delete(long id)
        {
            return DireccionClienteData.Eliminar(id);
        }
    }
}