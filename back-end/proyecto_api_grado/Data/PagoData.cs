using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class PagoData
    {
        public static long Registrar(Pago oPago)
        {
            ConexionBD objConex = new ConexionBD();
            string refExt = string.IsNullOrEmpty(oPago.Referencia_externa) ? "NULL" : $"'{oPago.Referencia_externa}'";
            string sentencia = $"EXEC usp_Pago_Registrar '{oPago.MetodoPago}',{oPago.Monto},{refExt},{oPago.IdFactura}";

            if (objConex.ConsultarValorUnico(sentencia, false))
            {
                long idPago = Convert.ToInt64(objConex.ValorUnico);
                objConex.CerrarConexion();
                objConex = null;
                return idPago;
            }
            objConex.CerrarConexion();
            objConex = null;
            return 0;
        }

        public static bool ActualizarEstado(long idPago, string nuevoEstado)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pago_ActualizarEstado {idPago},'{nuevoEstado}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idPago)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pago_Eliminar {idPago}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static Pago Consultar(long idPago)
        {
            Pago oPago = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pago_ObtenerPorId {idPago}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oPago = new Pago()
                    {
                        IdPago = Convert.ToInt64(dr["id_pago"]),
                        MetodoPago = dr["metodo_pago"].ToString(),
                        Monto = Convert.ToDecimal(dr["monto"]),
                        Estado = dr["estado"].ToString(),
                        Referencia_externa = dr["referencia_externa"].ToString(),
                        FechaPago = Convert.ToDateTime(dr["fecha_pago"]),
                        IdFactura = Convert.ToInt64(dr["id_factura"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oPago;
        }

        public static List<Pago> Listar()
        {
            List<Pago> oLista = new List<Pago>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_pago, metodo_pago, monto, estado, referencia_externa, fecha_pago, id_factura FROM Pago";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Pago()
                    {
                        IdPago = Convert.ToInt64(dr["id_pago"]),
                        MetodoPago = dr["metodo_pago"].ToString(),
                        Monto = Convert.ToDecimal(dr["monto"]),
                        Estado = dr["estado"].ToString(),
                        Referencia_externa = dr["referencia_externa"].ToString(),
                        FechaPago = Convert.ToDateTime(dr["fecha_pago"]),
                        IdFactura = Convert.ToInt64(dr["id_factura"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }

        public static List<Pago> ListarPorFactura(long idFactura)
        {
            List<Pago> oLista = new List<Pago>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Pago_ListarPorFactura {idFactura}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Pago()
                    {
                        IdPago = Convert.ToInt64(dr["id_pago"]),
                        MetodoPago = dr["metodo_pago"].ToString(),
                        Monto = Convert.ToDecimal(dr["monto"]),
                        Estado = dr["estado"].ToString(),
                        Referencia_externa = dr["referencia_externa"].ToString(),
                        FechaPago = Convert.ToDateTime(dr["fecha_pago"]),
                        IdFactura = idFactura
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}