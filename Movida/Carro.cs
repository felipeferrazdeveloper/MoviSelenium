using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movida
{
    class Carro
    {
        public string modelo { get; set; }
        public double preco { get; set; }

        public Carro(string modelo, double preco)
        {
            this.modelo = modelo;
            this.preco = preco;
        }
    }
}