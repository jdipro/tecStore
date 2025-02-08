using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //hacer uso de funcionalidades relacionadas con la red (por ejemplo, descargando datos desde una API, enviando correos electrónicos, o similar). 
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()  //preparo conexion a la db.
        {   
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion Empresa, C.Descripcion Clasificacion, A.ImagenUrl, A.Precio from ARTICULOS A, CATEGORIAS C, MARCAS M Where A.IdCategoria = C.Id and A.IdMarca = M.Id";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader(); //realizo un lectura de esos datos.

                while (lector.Read())
                {
                    //Para leerlos hago un while. El .Read() se fijará si hay lectura. De ser así, posicionará el puntero en la primera fila y devuelve true.
                    //Luego bajará al otro y así hasta q no haya más. Cada vez q baje reutilizará la variable. Creará una nueva var en el stack de objetos.

                    Articulo aux = new Articulo(); //Genero objeto Articulo auxiliar y lo empiezo a cargar con los datos del lector de ese registro.


                    aux.Id = lector.GetInt32(0);
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Empresa = new Marca();
                    aux.Empresa.Descripcion = (string)lector["Empresa"];
                    aux.Clasificacion = new Categoria();
                    aux.Clasificacion.Descripcion = (string)lector["Clasificacion"];
                    aux.ImagenUrl = (string)lector["ImagenUrl"];
                    aux.Precio = lector.IsDBNull(lector.GetOrdinal("Precio")) ? 0m : lector.GetDecimal(lector.GetOrdinal("Precio"));


                    lista.Add(aux); // Agrego artículo a la lista.
                }





                conexion.Close(); //Cierro la conección.
                return lista; //al final del contenido del try ira el return


            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los Artículos", ex);
            }

        }

        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {   //seteo los parámetros de esta manera para evitar confusiones.
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) VALUES (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio)");
              
                datos.setearParametro("@Codigo", nuevo.Codigo);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Descripcion", nuevo.Descripcion);
                datos.setearParametro("@IdMarca", nuevo.Empresa.Id);
                datos.setearParametro("@IdCateoria", nuevo.Clasificacion.Id);
                datos.setearParametro("@ImagenUrl", nuevo.ImagenUrl);
                datos.setearParametro("@Precio", nuevo.Precio);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }



    }
}
