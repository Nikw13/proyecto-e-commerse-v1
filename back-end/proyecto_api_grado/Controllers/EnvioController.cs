using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class EnvioController : ApiController
    {
        public List<Envio> Get()
        {
            return EnvioData.Listar();
        }

        public Envio Get(long id)
        {
            return EnvioData.Consultar(id);
        }

        [Route("api/Envio/PorPedido/{idPedido}")]
        public Envio GetPorPedido(long idPedido)
        {
            return EnvioData.ObtenerPorPedido(idPedido);
        }

        public long Post([FromBody] Envio oEnvio)
        {
            return EnvioData.Registrar(oEnvio);
        }

        [Route("api/Envio/Rastreo/{id}")]
        public bool PutRastreo(long id, [FromBody] Envio oEnvio)
        {
            return EnvioData.ActualizarRastreo(id, oEnvio.EmpresaTransporte, oEnvio.NumeroGuia);
        }

        [Route("api/Envio/Estado/{id}")]
        public bool PutEstado(long id, [FromBody] string estado)
        {
            return EnvioData.ActualizarEstado(id, estado);
        }

        public bool Delete(long id)
        {
            return EnvioData.Eliminar(id);
        }
    }
}