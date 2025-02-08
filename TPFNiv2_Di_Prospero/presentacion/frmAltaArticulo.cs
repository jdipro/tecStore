using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;

namespace presentacion
{
    public partial class frmAltaArticulo : Form
    {
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Articulo arti = new Articulo();
            ArticuloNegocio negocio = new ArticuloNegocio();



            try
            {
                arti.Codigo = txtCodArt.Text;
                arti.Nombre = txtNombre.Text;
                arti.Descripcion = txtDescripcion.Text;
                arti.Empresa = (Marca)cbbMarca.SelectedItem;
                arti.Clasificacion = (Categoria)cbbCategoria.SelectedItem;
                arti.ImagenUrl = txtImagen.Text;
                arti.Precio = numUDPrecio.Value;

                negocio.Agregar(arti);
                MessageBox.Show("Agregado exitosamente.");
                Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            //Cargo el ComboBox del label Marca.
            MarcaNegocio marNegocio = new MarcaNegocio();
            try
            {
                cbbMarca.DataSource = marNegocio.listar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            //Cargo el ComboBox del label Categoría..
            CategoriaNegocio catNegocio = new CategoriaNegocio();
            try
            {
                cbbCategoria.DataSource = catNegocio.listar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
