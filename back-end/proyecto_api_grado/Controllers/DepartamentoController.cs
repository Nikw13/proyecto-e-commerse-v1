using proyecto_api_grado.Data;
using proyecto_api_grado.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace proyecto_api_grado.Controllers
{
    public class DepartamentoController : ApiController
    {
        public List<Departamento> Get()
        {
            return DepartamentoData.Listar();
        }

        public Departamento Get(int id)
        {
            return DepartamentoData.Consultar(id);
        }

        public bool Post([FromBody] Departamento oDepartamento)
        {
            return DepartamentoData.Registrar(oDepartamento);
        }

        public bool Put([FromBody] Departamento oDepartamento)
        {
            return DepartamentoData.Actualizar(oDepartamento);
        }

        public bool Delete(int id)
        {
            return DepartamentoData.Eliminar(id);
        }
    }
}