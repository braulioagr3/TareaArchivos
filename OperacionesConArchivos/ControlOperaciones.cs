using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperacionesConArchivos
{
    public partial class ControlOperaciones : Form
    {
        public ControlOperaciones(string titulo)
        {
            this.Text = titulo;
            InitializeComponent();
        }

        public string Nombre
        {
            get { return this.textBox1.Text; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBox1.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor no deje el campo vacio", "Error");
            }
        }

    }
}
