using Dominio;
using Negocio;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;


namespace presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;  //usado en "agregar ".

        private OpenFileDialog archivo = null; //usado en Filtro avanzado.
        public frmAltaArticulo() //usado en " Agregar ", el constructor vacío.
        {
            InitializeComponent();
        }
        public frmAltaArticulo(Articulo articulo) //usado en "modificar", el contructor ira cargado con un artículo.
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Artículo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {  
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if(articulo == null)
                {
                    articulo = new Articulo();
                }
                articulo.Codigo = txtCodArt.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Empresa = (Marca)cbbMarca.SelectedItem;
                articulo.Clasificacion = (Categoria)cbbCategoria.SelectedItem;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = numUDPrecio.Value; 
                

                if (articulo.Id != 0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("Modificado exitosamente.");
                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Agregado exitosamente.");
                }

                //Guardo imagen si la levantó localmente:
                if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                }

                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error para agregar o modificar arrtículo en método btnAceptarClick() de frmAltaArticulo: {ex.Message}");
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            //Cargo el ComboBox del lbMarca.
            MarcaNegocio marNegocio = new MarcaNegocio();
            try
            {
                cbbMarca.DataSource = marNegocio.listar();
                cbbMarca.ValueMember = "Id";  //marco cual será el elemento preseleccionado.
                cbbMarca.DisplayMember = "Descripcion";

                ingresoModificacion();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cbbMarca en método frmAltaArticulo_Load() de frmAltaArticulo: {ex.Message}");
                MessageBox.Show(ex.ToString());
            }

            //Cargo el ComboBox del lbCategoría..
            CategoriaNegocio catNegocio = new CategoriaNegocio();
            try
            {
                cbbCategoria.DataSource = catNegocio.listar();
                cbbCategoria.ValueMember = "Id";  //marco cual será el elemento preseleccionado.
                cbbCategoria.DisplayMember = "Descripcion";

                ingresoModificacion();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cbbCategoria  en método frmAltaArticulo_Load() de frmAltaArticulo: {ex.Message}");
                MessageBox.Show(ex.ToString());
            }

          
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

        private void ingresoModificacion() //Creo esta función para compartir en los dos Try/Catch de frmAltaLoad. Así puedo preseleccioanr los valores que elija.
        {
            if (articulo != null)   //validacion a la hora de "modificar"
            {
                txtCodArt.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                txtImagenUrl.Text = articulo.ImagenUrl;
                cargarImagen(articulo.ImagenUrl);
                cbbMarca.SelectedValue = articulo.Empresa.Id;
                cbbCategoria.SelectedValue = articulo.Clasificacion.Id;

            }
        }

  
        public void cargarImagen(string imagen) //metodo privado para cargar imagen.
        {
            try
            {
                pbArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la imagen en pbArticulo de método cargarImagen() de frmAltaArticulo: {ex.Message}");
                MessageBox.Show(ex.ToString());
                pbArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }

        }

        //Lógica Botón "+" Agregar Imagen a DB.. :
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg| * .jpg; |png| *.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

            }
        }
       
    }
}
