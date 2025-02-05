using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; //esta biblioteca me permite usar DisplayName -> se conoce como Anotation.

namespace Dominio
{
    public class Articulo //tomara las columnas de la tabla Articulos de la db.
    {
        public int Id { get; set; } 

        [DisplayName("Código")]
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public int IdMarca { get; set; }

        [DisplayName("IdCategoría")]
        public int IdCategoria { get; set; }

        public string ImagenUrl { get; set; }

        public decimal Precio { get; set; }


        //Esta clase se utiliza en ArtículoNegocio.cs para crear al Artículo modelo genérico...es como Schema en JS y Node.
        //Ve la property Numer, Nombre y Descripción.Luego mapea de forma automática cada columna con cada objeto de la lista.
        //No va a llevar los Id de MSQL, acá trabajamos con objetos. No necesariamente se tienen que llamar iguales las clases que
        //los id, en este caso si, tendré la class Pokemon y la class Elemento. De otra forma, los nombre serñian la relación entre las tablas.





    }
}
