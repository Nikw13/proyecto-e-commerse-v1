using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class ClienteController : ApiController
    {
        public List<Cliente> Get()
        {
            return ClienteData.Listar();
        }

        public Cliente Get(long id)
        {
            return ClienteData.Consultar(id);
        }

        [Route("api/Cliente/PorEmail/{email}")]
        public Cliente GetPorEmail(string email)
        {
            return ClienteData.ObtenerPorEmail(email);
        }

        public long Post([FromBody] Cliente oCliente)
        {
            return ClienteData.Registrar(oCliente);
        }

        public bool Put([FromBody] Cliente oCliente)
        {
            return ClienteData.Actualizar(oCliente);
        }

        [Route("api/Cliente/Password/{id}")]
        public bool PutPassword(long id, [FromBody] string nuevaContrasena)
        {
            return ClienteData.ActualizarPassword(id, nuevaContrasena);
        }

        public bool Delete(long id)
        {
            return ClienteData.Eliminar(id);
        }
    }
}