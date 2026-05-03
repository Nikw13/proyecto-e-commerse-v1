using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class DepartamentoData
    {
        public static bool Registrar(Departamento oDepartamento)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Departamento_Registrar '{oDepartamento.Nombre}','{oDepartamento.CodigoDane}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            else
            {
                objConex = null;
                return true;
            }
        }

        public static bool Actualizar(Departamento oDepartamento)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Departamento_Actualizar {oDepartamento.IdDepartamento},'{oDepartamento.Nombre}','{oDepartamento.CodigoDane}',{(oDepartamento.Activo ? 1 : 0)}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            else
            {
                objConex = null;
                return true;
            }
        }

        public static bool Eliminar(int idDepartamento)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Departamento_Eliminar {idDepartamento}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            else
            {
                objConex = null;
                return true;
            }
        }

        public static Departamento Consultar(int idDepartamento)
        {
            Departamento oDepartamento = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Departamento_ObtenerPorId {idDepartamento}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oDepartamento = new Departamento()
                    {
                        IdDepartamento = Convert.ToInt32(dr["id_departamento"]),
                        Nombre = dr["nombre"].ToString(),
                        CodigoDane = dr["codigo_dane"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oDepartamento;
        }

        public static List<Departamento> Listar()
        {
            List<Departamento> oLista = new List<Departamento>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "EXEC usp_Departamento_Listar";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Departamento()
                    {
                        IdDepartamento = Convert.ToInt32(dr["id_departamento"]),
                        Nombre = dr["nombre"].ToString(),
                        CodigoDane = dr["codigo_dane"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}