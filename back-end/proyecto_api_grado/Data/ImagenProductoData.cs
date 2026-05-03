using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class ImagenProductoData
    {
        public static bool Registrar(ImagenProducto oImagen)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_ImagenProducto_Registrar '{oImagen.UrlImagen}',{(oImagen.EsPrincipal ? 1 : 0)},{oImagen.Orden},{oImagen.IdProducto}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(int idImagen)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_ImagenProducto_Eliminar {idImagen}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static List<ImagenProducto> ListarPorProducto(long idProducto)
        {
            List<ImagenProducto> oLista = new List<ImagenProducto>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_ImagenProducto_ListarPorProducto {idProducto}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new ImagenProducto()
                    {
                        IdImagen = Convert.ToInt32(dr["id_imagen"]),
                        UrlImagen = dr["url_imagen"].ToString(),
                        EsPrincipal = Convert.ToBoolean(dr["es_principal"]),
                        Orden = Convert.ToByte(dr["orden"]),
                        IdProducto = idProducto
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}