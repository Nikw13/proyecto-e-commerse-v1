using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class CategoriaController : ApiController
    {
        public List<Categoria> Get()
        {
            return CategoriaData.Listar();
        }

        [Route("api/Categoria/Activas")]
        public List<Categoria> GetActivas()
        {
            return CategoriaData.ListarActivas();
        }

        public Categoria Get(int id)
        {
            return CategoriaData.Consultar(id);
        }

        public bool Post([FromBody] Categoria oCategoria)
        {
            return CategoriaData.Registrar(oCategoria);
        }

        public bool Put([FromBody] Categoria oCategoria)
        {
            return CategoriaData.Actualizar(oCategoria);
        }

        public bool Delete(int id)
        {
            return CategoriaData.Eliminar(id);
        }
    }
}