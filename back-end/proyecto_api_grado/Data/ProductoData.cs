using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class ProductoData
    {
        public static long Registrar(Producto oProducto)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Producto_Registrar '{oProducto.Nombre}','{oProducto.Descripcion}','{oProducto.Sku}',{oProducto.Precio},{oProducto.StockCantidad},{oProducto.IdCategoria}";

            if (objConex.ConsultarValorUnico(sentencia, false))
            {
                long idProducto = Convert.ToInt64(objConex.ValorUnico);
                objConex.CerrarConexion();
                objConex = null;
                return idProducto;
            }
            objConex.CerrarConexion();
            objConex = null;
            return 0;
        }

        public static bool Actualizar(Producto oProducto)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Producto_Actualizar {oProducto.IdProducto},'{oProducto.Nombre}','{oProducto.Descripcion}','{oProducto.Sku}',{oProducto.Precio},{oProducto.StockCantidad},{(oProducto.Activo ? 1 : 0)},{oProducto.IdCategoria}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool ActualizarStock(long idProducto, int cantidadCambio)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Producto_ActualizarStock {idProducto},{cantidadCambio}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idProducto)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"UPDATE Producto SET activo = 0 WHERE id_producto = {idProducto}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static Producto Consultar(long idProducto)
        {
            Producto oProducto = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Producto_ObtenerPorId {idProducto}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oProducto = new Producto()
                    {
                        IdProducto = Convert.ToInt64(dr["id_producto"]),
                        Nombre = dr["nombre"].ToString(),
                        Descripcion = dr["descripcion"].ToString(),
                        Sku = dr["sku"].ToString(),
                        Precio = Convert.ToDecimal(dr["precio"]),
                        StockCantidad = Convert.ToInt32(dr["stock_cantidad"]),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        UpdatedAt = dr["updated_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_at"]),
                        IdCategoria = Convert.ToInt32(dr["id_categoria"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oProducto;
        }

        public static List<Producto> Listar()
        {
            List<Producto> oLista = new List<Producto>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_producto, nombre, descripcion, sku, precio, stock_cantidad, activo, created_at, updated_at, id_categoria FROM Producto";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Producto()
                    {
                        IdProducto = Convert.ToInt64(dr["id_producto"]),
                        Nombre = dr["nombre"].ToString(),
                        Descripcion = dr["descripcion"].ToString(),
                        Sku = dr["sku"].ToString(),
                        Precio = Convert.ToDecimal(dr["precio"]),
                        StockCantidad = Convert.ToInt32(dr["stock_cantidad"]),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        UpdatedAt = dr["updated_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_at"]),
                        IdCategoria = Convert.ToInt32(dr["id_categoria"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}