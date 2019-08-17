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

namespace OperacionesConArchivos
{
    public partial class OperadorDeArchivos : Form
    {
        private List<Registro> registros;
        private Registro registro;
        private BinaryWriter writer;
        private BinaryReader reader;
        private ControlOperaciones controlOperaciones;
        private string directorio;
        private string nombreArchivo;

        public OperadorDeArchivos()
        {
            registros = new List<Registro>();
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.directorio = Environment.CurrentDirectory;
            this.nombreArchivo = "";
            openFileDialog.AddExtension = true;//Habilita la opcion para añadir la extension 
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = this.directorio;
        }

        private void operaciones_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.AccessibleName)
            {
                case "Crear":
                    #region Crear
                    this.controlOperaciones = new ControlOperaciones("Crear Archivo");
                    if (controlOperaciones.ShowDialog().Equals(DialogResult.OK))
                    {
                        this.nombreArchivo = this.directorio + "\\" + controlOperaciones.Nombre + ".tarea";
                        if(!File.Exists(this.nombreArchivo))
                        {
                            FileStream file;
                            file = new FileStream(this.nombreArchivo, FileMode.Create);
                            file.Close();
                            button1.Enabled = true;
                        }
                    }
                    this.controlOperaciones.Dispose();
                    #endregion
                break;
                case "Abrir":
                    #region Abrir
                    if (this.openFileDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        this.nombreArchivo = openFileDialog.FileName;
                        if (File.Exists(this.nombreArchivo))
                        {
                            this.leeArchivo();
                            button1.Enabled = true;
                        }
                    }
                    #endregion
                break;
                case "Cambiar":
                    #region Cambiar
                    string original;
                    string nuevo;
                    original = "";
                    nuevo = "";
                    if (this.openFileDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        original = openFileDialog.FileName;
                        if (File.Exists(original))
                        {
                            this.controlOperaciones = new ControlOperaciones("Nuevo Nombre");
                            if (this.controlOperaciones.ShowDialog().Equals(DialogResult.OK))
                            {
                                nuevo = this.directorio + "//" + this.controlOperaciones.Nombre;
                                if (!File.Exists(nuevo))
                                {
                                    File.Move(original, nuevo);
                                }
                            }
                        }
                    }
                    #endregion
                break;
                case "Eliminar":
                    #region Eliminar
                    this.controlOperaciones = new ControlOperaciones("EliminarArchivo");
                    string nombreAuxiliar;
                    if (controlOperaciones.ShowDialog().Equals(DialogResult.OK))
                    {
                        nombreAuxiliar = this.directorio + "\\" + controlOperaciones.Nombre + ".tarea";
                        if (!this.nombreArchivo.Equals(nombreAuxiliar))
                        {
                            if(File.Exists(nombreAuxiliar))
                            {
                                File.Delete(nombreAuxiliar);
                            }
                        }
                    }
                    #endregion
                break;
                case "Cerrar":
                    #region Cerrar
                    this.registros.Clear();
                    this.nombreArchivo = "";
                    this.richTextBox1.Text = "";
                    button1.Enabled = false;
                    #endregion
                break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    richTextBox1.Text += textBox1.Text + "\n";
                    Int32.Parse(textBox1.Text);
                    this.registro = new Registro("int", textBox1.Text);
                    this.registros.Add(this.registro);
                    textBox1.Text = "";
                    this.grabaRegistro();
                }
            }
            catch(FormatException)
            {
                this.registro = new Registro("string", textBox1.Text);
                this.registros.Add(this.registro);
                textBox1.Text = "";
                this.grabaRegistro();
            }
        }

        private void grabaRegistro()
        {
            try
            {
                using (writer = new BinaryWriter(new FileStream(this.nombreArchivo, FileMode.Open)))//Abre el archivo con el BinaryWriter
                {
                    this.writer.Seek(0, SeekOrigin.Begin);//Posiciona el grabado del archivo en la dirección actual
                    this.writer.Write(this.registros.Count);
                    this.writer.Seek(0, SeekOrigin.End);
                    this.writer.Write(this.registro.Tipo);//Graba la dirección Actual
                    if (registro.Tipo.Equals("int"))
                    {
                        this.writer.Write(Int32.Parse(this.registro.Valor));
                    }
                    else if (registro.Tipo.Equals("string"))
                    {
                        this.writer.Write(this.registro.Valor);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void leeArchivo()
        {
            using (reader = new BinaryReader(new FileStream(this.nombreArchivo, FileMode.Open)))//Abre el archivo con el BinaryReader
            {
                int datos;
                string tipo;
                string valor;
                tipo = "";
                valor = "";
                datos = this.reader.ReadInt32();
                for (int i = 0; i < datos; i++)
                {
                    tipo = reader.ReadString();
                    if (tipo.Equals("int"))
                    {
                        valor = reader.ReadInt32().ToString();
                    }
                    else if (tipo.Equals("string"))
                    {
                        valor = reader.ReadString();
                    }
                    richTextBox1.Text += valor + "\n";
                    this.registro = new Registro(tipo, valor);
                    this.registros.Add(registro);
                    valor = "";
                }
            }
        }

    }
}
