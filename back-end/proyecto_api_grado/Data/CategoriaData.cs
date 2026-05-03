using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using proyecto_api_grado.Models;

namespace proyecto_api_grado.Data
{
    public class CategoriaData
    {
        public static bool Registrar(Categoria oCategoria)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Categoria_Registrar '{oCategoria.Nombre}','{oCategoria.Descripcion}'";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Actualizar(Categoria oCategoria)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"EXEC usp_Categoria_Actualizar {oCategoria.IdCategoria},'{oCategoria.Nombre}','{oCategoria.Descripcion}',{(oCategoria.Activo ? 1 : 0)}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static bool Eliminar(int idCategoria)
        {
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"UPDATE Categoria SET activo = 0 WHERE id_categoria = {idCategoria}";

            if (!objConex.EjecutarSentencia(sentencia, false))
            {
                objConex = null;
                return false;
            }
            objConex = null;
            return true;
        }

        public static Categoria Consultar(int idCategoria)
        {
            Categoria oCategoria = null;
            ConexionBD objConex = new ConexionBD();
            string sentencia = $"SELECT id_categoria, nombre, descripcion, activo FROM Categoria WHERE id_categoria = {idCategoria}";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                if (dr.Read())
                {
                    oCategoria = new Categoria()
                    {
                        IdCategoria = Convert.ToInt32(dr["id_categoria"]),
                        Nombre = dr["nombre"].ToString(),
                        Descripcion = dr["descripcion"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"])
                    };
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oCategoria;
        }

        public static List<Categoria> Listar()
        {
            List<Categoria> oLista = new List<Categoria>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "SELECT id_categoria, nombre, descripcion, activo FROM Categoria";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Categoria()
                    {
                        IdCategoria = Convert.ToInt32(dr["id_categoria"]),
                        Nombre = dr["nombre"].ToString(),
                        Descripcion = dr["descripcion"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"])
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }

        public static List<Categoria> ListarActivas()
        {
            List<Categoria> oLista = new List<Categoria>();
            ConexionBD objConex = new ConexionBD();
            string sentencia = "EXEC usp_Categoria_ListarActivas";

            if (objConex.Consultar(sentencia, false))
            {
                SqlDataReader dr = objConex.Reader;
                while (dr.Read())
                {
                    oLista.Add(new Categoria()
                    {
                        IdCategoria = Convert.ToInt32(dr["id_categoria"]),
                        Nombre = dr["nombre"].ToString(),
                        Descripcion = dr["descripcion"].ToString(),
                        Activo = true
                    });
                }
            }
            objConex.CerrarConexion();
            objConex = null;
            return oLista;
        }
    }
}