using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class DetallePedidoData
    {
        public static bool AgregarItem(long idPedido, long idProducto, int cantidad)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_DetallePedido_AgregarItem {idPedido},{idProducto},{cantidad}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool EliminarItem(long idDetalle)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_DetallePedido_EliminarItem {idDetalle}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static List<DetallePedido> ListarPorPedido(long idPedido)
        {
            List<DetallePedido> oLista = new List<DetallePedido>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_DetallePedido_ListarPorPedido {idPedido}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new DetallePedido()
                    {
                        IdDetalle = Convert.ToInt64(dr["id_detalle"]),
                        Cantidad = Convert.ToInt32(dr["cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(dr["precio_unitario"]),
                        Subtotal = Convert.ToDecimal(dr["subtotal"]),
                        IdPedido = idPedido,
                        IdProducto = 0
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}