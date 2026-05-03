using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class ClienteData
    {
        public static long Registrar(Cliente oCliente)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Cliente_Registrar '{oCliente.Nombres}','{oCliente.Apellidos}','{oCliente.Email}','{oCliente.Telefono}','{oCliente.ContrasenaHash}'";

            if (objConex.ConsultarValorUnico(sentencia, false))
            {
                long idCliente = Convert.ToInt64(objConex.ValorUnico);
                objConex.CerrarConexion();
                objConex = null;
                return idCliente;
            }
            objConex.CerrarConexion();
            objConex = null;
            return 0;
        }

        public static bool Actualizar(Cliente oCliente)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Cliente_Actualizar {oCliente.IdCliente},'{oCliente.Nombres}','{oCliente.Apellidos}','{oCliente.Email}','{oCliente.Telefono}','{oCliente.Estado}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool ActualizarPassword(long idCliente, string nuevaContrasenaHash)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Cliente_ActualizarPassword {idCliente},'{nuevaContrasenaHash}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idCliente)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"UPDATE Cliente SET estado = 'Inactivo' WHERE id_cliente = {idCliente}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static Cliente Consultar(long idCliente)
        {
            Cliente oCliente = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"SELECT id_cliente, nombres, apellidos, email, telefono, contrasena_hash, estado, created_at, updated_at FROM Cliente WHERE id_cliente = {idCliente}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oCliente = new Cliente()
                    {
                        IdCliente = Convert.ToInt64(dr["id_cliente"]),
                        Nombres = dr["nombres"].ToString(),
                        Apellidos = dr["apellidos"].ToString(),
                        Email = dr["email"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        ContrasenaHash = dr["contrasena_hash"].ToString(),
                        Estado = dr["estado"].ToString(),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        UpdatedAt = dr["updated_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_at"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oCliente;
        }

        public static Cliente ObtenerPorEmail(string email)
        {
            Cliente oCliente = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Cliente_ObtenerPorEmail '{email}'";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oCliente = new Cliente()
                    {
                        IdCliente = Convert.ToInt64(dr["id_cliente"]),
                        Nombres = dr["nombres"].ToString(),
                        Apellidos = dr["apellidos"].ToString(),
                        Email = dr["email"].ToString(),
                        ContrasenaHash = dr["contrasena_hash"].ToString(),
                        Estado = dr["estado"].ToString()
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oCliente;
        }

        public static List<Cliente> Listar()
        {
            List<Cliente> oLista = new List<Cliente>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "EXEC usp_Cliente_Listar";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Cliente()
                    {
                        IdCliente = Convert.ToInt64(dr["id_cliente"]),
                        Nombres = dr["nombres"].ToString(),
                        Apellidos = dr["apellidos"].ToString(),
                        Email = dr["email"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Estado = dr["estado"].ToString(),
                        CreatedAt = Convert.ToDateTime(dr["created_at"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}