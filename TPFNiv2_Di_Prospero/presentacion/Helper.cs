using System;
using System.Windows.Forms;

namespace Negocio
{
    //Esta clase tendrá métodos comunes a ser llamados por otros métodos en proyectos previamnete referenciados.
    public static class Helper                      
    {
        public static void CargarImagen(PictureBox pictureBox, string imagen)
        {
            try
            {
                pictureBox.Load(imagen);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                
                pictureBox.Load(@".\Media\image_not_found.png");
            }
        }
    }
}
