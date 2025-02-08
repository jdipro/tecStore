using System;
using System.Collections.Generic;
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

        private void frmArticulo_Load(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulo = negocio.listar();
            dgvArticulos.DataSource = listaArticulo;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            pbArticulo.Load(listaArticulo[0].ImagenUrl);
        }






        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);

            }
        }

        

        private void cargarImagen(string imagen) //metodo privado para cargar imagen.
        {
            try
            {
                pbArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png"); //carga la imagen  que guardé arriba en el pictureBox para que se muestr por pantalla.
            }                                   //Pero si la dejo acá, se compllica con la actualizacion de la db, ya que no se enterará y me romperá todo. Para esto haremos un nuevo metodo privado.
        }

        //Lógica para Agregar Artículo.
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo(); 
            alta.ShowDialog();
             
        }
    }

}
