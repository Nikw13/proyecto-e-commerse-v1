using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class PedidoData
    {
        public static long Registrar(Pedido oPedido)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pedido_Registrar {oPedido.IdCliente},{oPedido.IdDireccion}";

            if (objConex.ConsultarValorUnico(sentencia, false))
            {
                long idPedido = Convert.ToInt64(objConex.ValorUnico);
                objConex.CerrarConexion();
                objConex = null;
                return idPedido;
            }
            objConex.CerrarConexion();
            objConex = null;
            return 0;
        }

        public static bool ActualizarEstado(long idPedido, string nuevoEstado)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pedido_ActualizarEstado {idPedido},'{nuevoEstado}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idPedido)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pedido_ActualizarEstado {idPedido},'Cancelado'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static Pedido Consultar(long idPedido)
        {
            Pedido oPedido = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pedido_ObtenerPorId {idPedido}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oPedido = new Pedido()
                    {
                        IdPedido = Convert.ToInt64(dr["id_pedido"]),
                        Estado = dr["estado"].ToString(),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        IdCliente = 0,
                        IdDireccion = 0
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oPedido;
        }

        public static List<Pedido> Listar()
        {
            List<Pedido> oLista = new List<Pedido>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_pedido, estado, created_at, updated_at, id_cliente, id_direccion FROM Pedido";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Pedido()
                    {
                        IdPedido = Convert.ToInt64(dr["id_pedido"]),
                        Estado = dr["estado"].ToString(),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        UpdatedAt = dr["updated_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_at"]),
                        IdCliente = Convert.ToInt64(dr["id_cliente"]),
                        IdDireccion = Convert.ToInt64(dr["id_direccion"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}