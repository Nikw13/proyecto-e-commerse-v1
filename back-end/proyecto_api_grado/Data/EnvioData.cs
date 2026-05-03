using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class EnvioData
    {
        public static long Registrar(Envio oEnvio)
        {
            ConexionBD objConex = new ConexionBD();
            string empresa = string.IsNullOrEmpty(oEnvio.EmpresaTransporte) ? "NULL" : $"'{oEnvio.EmpresaTransporte}'";
            string guia = string.IsNullOrEmpty(oEnvio.NumeroGuia) ? "NULL" : $"'{oEnvio.NumeroGuia}'";
            string sentencia = $"EXEC usp_Envio_Registrar '{oEnvio.MetodoEnvio}',{oEnvio.CostoEnvio},{oEnvio.IdPedido},{empresa},{guia}";

            if (objConex.ConsultarValorUnico(sentencia, false))
            {
                long idEnvio = Convert.ToInt64(objConex.ValorUnico);
                objConex.CerrarConexion();
                objConex = null;
                return idEnvio;
            }
            objConex.CerrarConexion();
            objConex = null;
            return 0;
        }

        public static bool ActualizarRastreo(long idEnvio, string empresaTransporte, string numeroGuia)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Envio_ActualizarRastreo {idEnvio},'{empresaTransporte}','{numeroGuia}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool ActualizarEstado(long idEnvio, string nuevoEstado)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Envio_ActualizarEstado {idEnvio},'{nuevoEstado}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idEnvio)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Envio_Eliminar {idEnvio}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static Envio Consultar(long idEnvio)
        {
            Envio oEnvio = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"SELECT id_envio, numero_guia, empresa_transporte, metodo_envio, costo_envio, estado_envio, fecha_despacho, fecha_entrega, created_at, id_pedido FROM Envio WHERE id_envio = {idEnvio}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oEnvio = new Envio()
                    {
                        IdEnvio = Convert.ToInt64(dr["id_envio"]),
                        NumeroGuia = dr["numero_guia"].ToString(),
                        EmpresaTransporte = dr["empresa_transporte"].ToString(),
                        MetodoEnvio = dr["metodo_envio"].ToString(),
                        CostoEnvio = Convert.ToDecimal(dr["costo_envio"]),
                        EstadoEnvio = dr["estado_envio"].ToString(),
                        FechaDespacho = dr["fecha_despacho"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_despacho"]),
                        FechaEntrega = dr["fecha_entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_entrega"]),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        IdPedido = Convert.ToInt64(dr["id_pedido"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oEnvio;
        }

        public static List<Envio> Listar()
        {
            List<Envio> oLista = new List<Envio>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_envio, numero_guia, empresa_transporte, metodo_envio, costo_envio, estado_envio, fecha_despacho, fecha_entrega, created_at, id_pedido FROM Envio";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Envio()
                    {
                        IdEnvio = Convert.ToInt64(dr["id_envio"]),
                        NumeroGuia = dr["numero_guia"].ToString(),
                        EmpresaTransporte = dr["empresa_transporte"].ToString(),
                        MetodoEnvio = dr["metodo_envio"].ToString(),
                        CostoEnvio = Convert.ToDecimal(dr["costo_envio"]),
                        EstadoEnvio = dr["estado_envio"].ToString(),
                        FechaDespacho = dr["fecha_despacho"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_despacho"]),
                        FechaEntrega = dr["fecha_entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_entrega"]),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        IdPedido = Convert.ToInt64(dr["id_pedido"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }

        public static Envio ObtenerPorPedido(long idPedido)
        {
            Envio oEnvio = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Envio_ObtenerPorPedido {idPedido}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oEnvio = new Envio()
                    {
                        IdEnvio = Convert.ToInt64(dr["id_envio"]),
                        NumeroGuia = dr["numero_guia"].ToString(),
                        EmpresaTransporte = dr["empresa_transporte"].ToString(),
                        MetodoEnvio = dr["metodo_envio"].ToString(),
                        CostoEnvio = Convert.ToDecimal(dr["costo_envio"]),
                        EstadoEnvio = dr["estado_envio"].ToString(),
                        FechaDespacho = dr["fecha_despacho"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_despacho"]),
                        FechaEntrega = dr["fecha_entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_entrega"]),
                        CreatedAt = DateTime.Now,
                        IdPedido = idPedido
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oEnvio;
        }
    }
}