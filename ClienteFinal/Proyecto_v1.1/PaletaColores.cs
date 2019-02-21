using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Proyecto_v1._1
{
    public partial class PaletaColores : Form
    {
        string color;

        public PaletaColores()
        {
            InitializeComponent();
        }
        
        public string GetColor()
        {
            return this.color;
        }

        private void RojoBtn_Click(object sender, EventArgs e)
        {
            this.color = "rojo";
            this.Close();
        }

        private void AzulBtn_Click(object sender, EventArgs e)
        {
            this.color = "azul";
            this.Close();
        }

        private void AmarilloBtn_Click(object sender, EventArgs e)
        {
            this.color = "amarillo";
            this.Close();
        }

        private void VerdeBtn_Click(object sender, EventArgs e)
        {
            this.color = "verde";
            this.Close();
        }
    }
}
