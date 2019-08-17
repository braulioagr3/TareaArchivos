using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacionesConArchivos
{
    class Registro
    {
        private string tipo;
        private string valor;

        public Registro(string tipo, string valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }
        public string Tipo
        {
            get { return this.tipo; }
        }
        public string Valor
        {
            get { return this.valor; }
        }
    }
}
