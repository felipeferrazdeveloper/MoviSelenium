using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movida
{
    class Parceiro
    {
        public List<Carro> carros { get; set; }
        public string nome { get; set; }
        public double media { get; set; }

        public Parceiro()
        {
            this.carros = new List<Carro>();
            this.nome = null;
        }

        public void adicionarCarro(Carro carro)
        {
            if (carro != null)
            {
                this.carros.Add(carro);
                this.media = calcularMediaPreco();
            }
        }

        public double calcularMediaPreco()
        {
            double soma = 0;
            foreach (var carro in carros)
            {

                soma = carro.preco + soma;
            }
            return media = soma / carros.Count;
        }
    }
}