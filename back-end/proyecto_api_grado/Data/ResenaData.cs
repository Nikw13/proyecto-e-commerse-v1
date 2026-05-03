using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class ResenaData
    {
        public static bool Registrar(Resena oResena)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Resena_Registrar {oResena.Rating},'{oResena.Comentario}',{oResena.IdCliente},{oResena.IdProducto}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Actualizar(Resena oResena)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Resena_Actualizar {oResena.IdResena},{oResena.Rating},'{oResena.Comentario}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(long idResena)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Resena_Eliminar {idResena}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static List<Resena> ListarPorProducto(long idProducto)
        {
            List<Resena> oLista = new List<Resena>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Resena_ListarPorProducto {idProducto}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Resena()
                    {
                        IdResena = Convert.ToInt64(dr["id_resena"]),
                        Rating = Convert.ToByte(dr["rating"]),
                        Comentario = dr["comentario"].ToString(),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        IdCliente = 0,
                        IdProducto = idProducto
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }

        public static List<Resena> Listar()
        {
            List<Resena> oLista = new List<Resena>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_resena, rating, comentario, created_at, id_cliente, id_producto FROM Resena";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Resena()
                    {
                        IdResena = Convert.ToInt64(dr["id_resena"]),
                        Rating = Convert.ToByte(dr["rating"]),
                        Comentario = dr["comentario"].ToString(),
                        CreatedAt = Convert.ToDateTime(dr["created_at"]),
                        IdCliente = Convert.ToInt64(dr["id_cliente"]),
                        IdProducto = Convert.ToInt64(dr["id_producto"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}