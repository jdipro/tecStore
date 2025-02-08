﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; //esta biblioteca me permite usar DisplayName -> se conoce como Anotation

namespace Dominio
{
    public class Categoria
    {
       
        public int Id { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return Descripcion;                                 //Sobreescribo el método ToString() para que la columna me devuelva el contenido solicitado
        }


    }
}
