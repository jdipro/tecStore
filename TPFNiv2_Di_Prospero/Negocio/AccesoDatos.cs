using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true");
            comando = new SqlCommand(); 
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura() 
        {
            comando.Connection = conexion; 
            try                        
            {
                conexion.Open(); 
                lector = comando.ExecuteReader(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Método para inserción de artículos a la DB.
        public void ejecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Seteo de parametros para el método Agregar() en la clase ArticuloNegocio.
        public void setearParametro(string nombre, object valor )
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void cerrarConexion() //Tengo que agregar el cierre de la conexión y cierro el lector, si es q alguno conectánose.
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }

    }
}
