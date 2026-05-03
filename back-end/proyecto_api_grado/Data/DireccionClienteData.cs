using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class DireccionClienteData
    {
        public static long Registrar(DireccionCliente oDireccion)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_DireccionCliente_Registrar '{oDireccion.Descripcion}',{(oDireccion.EsPrincipal ? 1 : 0)},{oDireccion.IdCliente},{oDireccion.IdCodigoPostal}";

            if (objConex.ConsultarValorUnico(sentencia, false))
            {
                long idDireccion = Convert.ToInt64(objConex.ValorUnico);
                objConex.CerrarConexion();
                objConex = null;
                return idDireccion;
            }
            objConex.CerrarConexion();
            objConex = null;
            return 0;
        }

        public static bool Actualizar(DireccionCliente oDireccion)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_DireccionCliente_Actualizar {oDireccion.IdDireccion},'{oDireccion.Descripcion}',{(oDireccion.EsPrincipal ? 1 : 0)},{(oDireccion.Activo ? 1 : 0)},{oDireccion.IdCodigoPostal}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idDireccion)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_DireccionCliente_Desactivar {idDireccion}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static DireccionCliente Consultar(long idDireccion)
        {
            DireccionCliente oDireccion = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"SELECT id_direccion, descripcion, es_principal, activo, id_cliente, id_codigo_postal FROM DireccionCliente WHERE id_direccion = {idDireccion}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oDireccion = new DireccionCliente()
                    {
                        IdDireccion = Convert.ToInt64(dr["id_direccion"]),
                        Descripcion = dr["descripcion"].ToString(),
                        EsPrincipal = Convert.ToBoolean(dr["es_principal"]),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdCliente = Convert.ToInt64(dr["id_cliente"]),
                        IdCodigoPostal = Convert.ToInt32(dr["id_codigo_postal"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oDireccion;
        }

        public static List<DireccionCliente> ListarPorCliente(long idCliente)
        {
            List<DireccionCliente> oLista = new List<DireccionCliente>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_DireccionCliente_ListarPorCliente {idCliente}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new DireccionCliente()
                    {
                        IdDireccion = Convert.ToInt64(dr["id_direccion"]),
                        Descripcion = dr["descripcion"].ToString(),
                        EsPrincipal = Convert.ToBoolean(dr["es_principal"]),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        IdCliente = idCliente,
                        IdCodigoPostal = 0
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}