using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace proyecto_api_grado.Data
{
    public class ConexionBD
    {
        private string strError;
        private bool blnBDAbierta;
        private string strCadenaCnx;
        private SqlConnection objCnnBD;
        private SqlCommand objCmdBD;
        private SqlDataReader objReader;
        private SqlDataAdapter dapGenerico;
        private DataSet dts;
        private string strVrUnico;
        private SqlParameter objParametro;

        public ConexionBD()
        {
            objCnnBD = new SqlConnection();
            objCmdBD = new SqlCommand();
            dapGenerico = new SqlDataAdapter();
            strVrUnico = "";
            objParametro = new SqlParameter();
            strError = "";
        }

        public SqlDataReader Reader
        {
            get { return objReader; }
        }

        public DataSet DataSet_Retornado
        {
            get { return dts; }
        }

        public string Error
        {
            set { strError = value; }
            get { return strError; }
        }

        public string ValorUnico
        {
            get { return strVrUnico; }
        }

        private bool AbrirConexion()
        {
            try
            {
                //strCadenaCnx = "Data Source=.;Initial Catalog=EcommerceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                strCadenaCnx = "workstation id=e-commerse.mssql.somee.com;packet size=4096;user id=Nikw13_SQLLogin_1;pwd=f2iol7ubwu;data source=e-commerse.mssql.somee.com;persist security info=False;initial catalog=e-commerse;TrustServerCertificate=True;";
                objCnnBD.ConnectionString = strCadenaCnx;
                objCnnBD.Open();
                blnBDAbierta = true;
                return true;
            }
            catch (Exception ex)
            {
                blnBDAbierta = false;
                strError = "Error al abrir la conexion -" + ex.Message;
                return false;
            }
        }

        public bool Consultar(string SentenciaSQL, bool blnCon_Parametros)
        {
            if (SentenciaSQL == "")
            {
                strError = "Error en instrucción SQL";
                return false;
            }
            if (blnBDAbierta == false)
            {
                if (AbrirConexion() == false)
                {
                    return false;
                }
            }
            objCmdBD.Connection = objCnnBD;
            if (blnCon_Parametros)
                objCmdBD.CommandType = CommandType.StoredProcedure;
            else
                objCmdBD.CommandType = CommandType.Text;
            objCmdBD.CommandText = SentenciaSQL;
            try
            {
                objReader = objCmdBD.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                strError = "Falla en ejecutar comando -" + ex.Message;
                return false;
            }
        }

        public bool EjecutarSentencia(string SentenciaSQL, bool blnCon_Parametros)
        {
            if (SentenciaSQL == "")
            {
                strError = "No se ha definido la sentencia a ejecutar ";
                return false;
            }
            if (blnBDAbierta == false)
            {
                if (AbrirConexion() == false)
                {
                    return false;
                }
            }
            objCmdBD.Connection = objCnnBD;
            if (blnCon_Parametros)
                objCmdBD.CommandType = CommandType.StoredProcedure;
            else
                objCmdBD.CommandType = CommandType.Text;
            objCmdBD.CommandText = SentenciaSQL;
            try
            {
                objCmdBD.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                strError = "Error al ejecutar la instrucción -" + ex.Message;
                return false;
            }
        }

        public bool ConsultarValorUnico(string SentenciaSQL, bool blnCon_Parametros)
        {
            if (SentenciaSQL == "")
            {
                strError = "No se ha definido la sentencia a ejecutar ";
                return false;
            }
            if (blnBDAbierta == false)
            {
                if (AbrirConexion() == false)
                {
                    return false;
                }
            }
            objCmdBD.Connection = objCnnBD;
            if (blnCon_Parametros)
                objCmdBD.CommandType = CommandType.StoredProcedure;
            else
                objCmdBD.CommandType = CommandType.Text;
            objCmdBD.CommandText = SentenciaSQL;
            try
            {
                strVrUnico = Convert.ToString(objCmdBD.ExecuteScalar());
                return true;
            }
            catch (Exception ex)
            {
                strError = "Error al ejecutar instrucción -" + ex.Message;
                return false;
            }
        }

        public void CerrarConexion()
        {
            try
            {
                objCmdBD = null;
            }
            catch (Exception ex)
            {
                strError = "Falla en cerrar Command -" + ex.Message;
            }
            try
            {
                objCnnBD.Close();
                objCnnBD = null;
            }
            catch (Exception ex)
            {
                strError = "Falla en cerrar conexión -" + ex.Message;
            }
        }

        public bool LlenarDataSet(string NombreTabla, string SentenciaSQL, bool blnCon_Parametros)
        {
            if (blnBDAbierta == false)
            {
                if (AbrirConexion() == false)
                {
                    return false;
                }
            }
            objCmdBD.Connection = objCnnBD;
            if (blnCon_Parametros)
                objCmdBD.CommandType = CommandType.StoredProcedure;
            else
                objCmdBD.CommandType = CommandType.Text;
            objCmdBD.CommandText = SentenciaSQL;
            try
            {
                dts = new DataSet();
                dapGenerico.SelectCommand = objCmdBD;
                dapGenerico.Fill(dts, NombreTabla);
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        public bool AgregarParametro(ParameterDirection Direccion, string Nombre_En_SP,
        SqlDbType TipoDato, Int16 Tamaño, object Valor)
        {
            try
            {
                objParametro.Direction = Direccion;
                objParametro.ParameterName = Nombre_En_SP;
                objParametro.SqlDbType = TipoDato;
                objParametro.Size = Tamaño;
                objParametro.Value = Valor;
                objCmdBD.Parameters.Add(objParametro);
                objParametro = new SqlParameter();
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }
    }
}