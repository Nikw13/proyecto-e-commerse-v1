using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class CiudadData
    {
        public static bool Registrar(Ciudad oCiudad)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Ciudad_Registrar '{oCiudad.Nombre}','{oCiudad.CodigoDane}',{oCiudad.IdDepartamento}";

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

        public static bool Actualizar(Ciudad oCiudad)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Ciudad_Actualizar {oCiudad.IdCiudad},'{oCiudad.Nombre}','{oCiudad.CodigoDane}',{(oCiudad.Activo ? 1 : 0)},{oCiudad.IdDepartamento}";

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

        public static bool Eliminar(int idCiudad)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Ciudad_Eliminar {idCiudad}";

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

        public static Ciudad Consultar(int idCiudad)
        {
            Ciudad oCiudad = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"SELECT id_ciudad, nombre, codigo_dane, activo, id_departamento FROM Ciudad WHERE id_ciudad = {idCiudad}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oCiudad = new Ciudad()
                    {
                        IdCiudad = Convert.ToInt32(dr["id_ciudad"]),
                        Nombre = dr["nombre"].ToString(),
                        CodigoDane = dr["codigo_dane"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdDepartamento = Convert.ToInt32(dr["id_departamento"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oCiudad;
        }

        public static List<Ciudad> Listar()
        {
            List<Ciudad> oLista = new List<Ciudad>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_ciudad, nombre, codigo_dane, activo, id_departamento FROM Ciudad";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Ciudad()
                    {
                        IdCiudad = Convert.ToInt32(dr["id_ciudad"]),
                        Nombre = dr["nombre"].ToString(),
                        CodigoDane = dr["codigo_dane"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdDepartamento = Convert.ToInt32(dr["id_departamento"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }

        public static List<Ciudad> ListarPorDepartamento(int idDepartamento)
        {
            List<Ciudad> oLista = new List<Ciudad>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Ciudad_ListarPorDepartamento {idDepartamento}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Ciudad()
                    {
                        IdCiudad = Convert.ToInt32(dr["id_ciudad"]),
                        Nombre = dr["nombre"].ToString(),
                        CodigoDane = dr["codigo_dane"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdDepartamento = idDepartamento
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}