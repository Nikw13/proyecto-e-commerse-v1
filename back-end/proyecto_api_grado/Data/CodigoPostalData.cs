using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class CodigoPostalData
    {
        public static bool Registrar(CodigoPostal oCodigoPostal)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_CodigoPostal_Registrar '{oCodigoPostal.Codigo}',{oCodigoPostal.IdCiudad}";

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

        public static bool Actualizar(CodigoPostal oCodigoPostal)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_CodigoPostal_Actualizar {oCodigoPostal.IdCodigoPostal},'{oCodigoPostal.Codigo}',{(oCodigoPostal.Activo ? 1 : 0)},{oCodigoPostal.IdCiudad}";

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

        public static bool Eliminar(int idCodigoPostal)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"DELETE FROM CodigoPostal WHERE id_codigo_postal = {idCodigoPostal}";

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

        public static CodigoPostal Consultar(int idCodigoPostal)
        {
            CodigoPostal oCodigoPostal = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"SELECT id_codigo_postal, codigo, activo, id_ciudad FROM CodigoPostal WHERE id_codigo_postal = {idCodigoPostal}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oCodigoPostal = new CodigoPostal()
                    {
                        IdCodigoPostal = Convert.ToInt32(dr["id_codigo_postal"]),
                        Codigo = dr["codigo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdCiudad = Convert.ToInt32(dr["id_ciudad"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oCodigoPostal;
        }

        public static List<CodigoPostal> Listar()
        {
            List<CodigoPostal> oLista = new List<CodigoPostal>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_codigo_postal, codigo, activo, id_ciudad FROM CodigoPostal";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new CodigoPostal()
                    {
                        IdCodigoPostal = Convert.ToInt32(dr["id_codigo_postal"]),
                        Codigo = dr["codigo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdCiudad = Convert.ToInt32(dr["id_ciudad"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }

        public static List<CodigoPostal> ListarPorCiudad(int idCiudad)
        {
            List<CodigoPostal> oLista = new List<CodigoPostal>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_CodigoPostal_ListarPorCiudad {idCiudad}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new CodigoPostal()
                    {
                        IdCodigoPostal = Convert.ToInt32(dr["id_codigo_postal"]),
                        Codigo = dr["codigo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdCiudad = idCiudad
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}