using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class FacturaData
    {
        public static long Registrar(Factura oFactura)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Factura_Registrar '{oFactura.NroFactura}',{oFactura.Subtotal},{oFactura.Iva},{oFactura.IdPedido}";

            if (objConex.ConsultarValorUnico(sentencia, false))
            {
                long idFactura = Convert.ToInt64(objConex.ValorUnico);
                objConex.CerrarConexion();
                objConex = null;
                return idFactura;
            }
            objConex.CerrarConexion();
            objConex = null;
            return 0;
        }

        public static bool Actualizar(Factura oFactura)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"UPDATE Factura SET nro_factura = '{oFactura.NroFactura}', subtotal = {oFactura.Subtotal}, iva = {oFactura.Iva} WHERE id_factura = {oFactura.IdFactura}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idFactura)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"DELETE FROM Factura WHERE id_factura = {idFactura}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static Factura Consultar(long idFactura)
        {
            Factura oFactura = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"SELECT id_factura, nro_factura, subtotal, iva, total, fecha_emision, id_pedido FROM Factura WHERE id_factura = {idFactura}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oFactura = new Factura()
                    {
                        IdFactura = Convert.ToInt64(dr["id_factura"]),
                        NroFactura = dr["nro_factura"].ToString(),
                        Subtotal = Convert.ToDecimal(dr["subtotal"]),
                        Iva = Convert.ToDecimal(dr["iva"]),
                        Total = Convert.ToDecimal(dr["total"]),
                        FechaEmision = Convert.ToDateTime(dr["fecha_emision"]),
                        IdPedido = Convert.ToInt64(dr["id_pedido"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oFactura;
        }

        public static List<Factura> Listar()
        {
            List<Factura> oLista = new List<Factura>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_factura, nro_factura, subtotal, iva, total, fecha_emision, id_pedido FROM Factura";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Factura()
                    {
                        IdFactura = Convert.ToInt64(dr["id_factura"]),
                        NroFactura = dr["nro_factura"].ToString(),
                        Subtotal = Convert.ToDecimal(dr["subtotal"]),
                        Iva = Convert.ToDecimal(dr["iva"]),
                        Total = Convert.ToDecimal(dr["total"]),
                        FechaEmision = Convert.ToDateTime(dr["fecha_emision"]),
                        IdPedido = Convert.ToInt64(dr["id_pedido"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }

        public static List<Factura> ListarPorCliente(long idCliente)
        {
            List<Factura> oLista = new List<Factura>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Factura_ListarPorCliente {idCliente}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Factura()
                    {
                        IdFactura = Convert.ToInt64(dr["id_factura"]),
                        NroFactura = dr["nro_factura"].ToString(),
                        Total = Convert.ToDecimal(dr["total"]),
                        FechaEmision = Convert.ToDateTime(dr["fecha_emision"]),
                        IdPedido = Convert.ToInt64(dr["id_pedido"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}