using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class ResenaController : ApiController
    {
        public List<Resena> Get()
        {
            return ResenaData.Listar();
        }

        [Route("api/Resena/PorProducto/{idProducto}")]
        public List<Resena> GetPorProducto(long idProducto)
        {
            return ResenaData.ListarPorProducto(idProducto);
        }

        public List<Resena> Get(long id)
        {
            return ResenaData.ListarPorProducto(id);
        }

        public bool Post([FromBody] Resena oResena)
        {
            return ResenaData.Registrar(oResena);
        }

        public bool Put([FromBody] Resena oResena)
        {
            return ResenaData.Actualizar(oResena);
        }

        public bool Delete(long id)
        {
            return ResenaData.Eliminar(id);
        }
    }
}