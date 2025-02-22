using System;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class frmArticulo : Form
    {

        private List<Articulo>  listaArticulo;
        public frmArticulo()
        {
            InitializeComponent();
        }

        //Método para setear los ComboBox.
        private void frmArticulo_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Empresa");
            cboCampo.Items.Add("Precio");
        }


        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        //Método para cambiar el objeto selecciondo en el dgv.
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void cargar() 
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            {
                try
                {
                    listaArticulo = negocio.listar();
                    dgvArticulos.DataSource = listaArticulo;
                    ocultarColumnas();
                    cargarImagen(listaArticulo[0].ImagenUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la imagen en método cargar() de frmArticulo: {ex.Message}");
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void cargarImagen(string imagen) //metodo privado para cargar imagen.
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagen) || !EsUrlValida(imagen)) //agregado para validar la viabilidad de la url ya que en varias se caía.
                {
                    pbArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                    return;
                }

                pbArticulo.Load(imagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error al cargar la imagen: {ex.Message}\nImagen URL: {imagen}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                pbArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png"); 
            }                              
        }

        // Método para verificar si la URL tiene un formato correcto y si apunta a una imagen (chatgpt)
        private bool EsUrlValida(string url)
        {
            try
            {
                Uri uriResult;
                bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                              (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return result;
            }
            catch
            {
                return false;
            }
        }




        //Lógica para Agregar Artículo.
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo(); 
            alta.ShowDialog();
            cargar();
             
        }

        //Lógica para Modificar Artículo
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        //Lógica para Eliminar artículo de la lista (opto por la eliminación física al no contar con una columna Activo en la DB). 
        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            Eliminar();
        }
        private void Eliminar(bool logico = false) 
        {                                                               
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad querés eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem; 
                    negocio.Eliminar(seleccionado.Id); 
                    cargar(); 
                }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la imagen en método Eliminar() de frmArticulo: {ex.Message}");
                MessageBox.Show(ex.ToString());
            }
        }

         
        //Lógica para validar el btnFiltro_Click.
        private bool validarFiltro()
        {
            //Valido si cbo está cargado.
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex <0)
            {
                MessageBox.Show("Por favor seleccione el criterio para filtrar.");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Por favor cargue el filtro con números ");
                    return true;
                }
                if (!(soloPrecio(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Por favor ingrese sólo números para expresar el precio de un producto.");
                    return true;
                }
            }
            return false;
        }
        
        //Valido que se Numeros.
        private bool soloPrecio(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }
        
        //Lógica para filtrar Avanzado de un artículo de la lista.
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           

        }

        //Lógica para actualización en la lista del filtro de búsqueda.
        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        //Lógica para actualización en la lista del filtro de búsqueda.
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;

            string filtro = txtFiltroRapido.Text;
            if (filtro.Length >=3)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Empresa.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Clasificacion.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        //Lógica para configurar los desplegables.
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }

}
