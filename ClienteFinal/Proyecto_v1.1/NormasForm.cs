using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Proyecto_v1._1
{
    public partial class NormasForm : Form
    {
        int pagina = 1;

        public NormasForm()
        {
            InitializeComponent();
            this.pictureBox2.Visible = false;
            this.pictureBox3.Visible = false;
            this.pictureBox4.Visible = false;
            this.button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.pagina++;
            this.label1.Text = this.pagina + "/4";
            this.button1.Enabled = true;
            if (this.pagina == 4)
                this.button2.Enabled = false;
            switch (this.pagina)
            {
                case 2:
                    this.pictureBox1.Visible = false;
                    this.pictureBox2.Visible = true;
                    break;
                case 3:
                    this.pictureBox2.Visible = false;
                    this.pictureBox3.Visible = true;
                    break;
                case 4:
                    this.pictureBox3.Visible = false;
                    this.pictureBox4.Visible = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pagina--;
            this.label1.Text = this.pagina + "/4";
            this.button2.Enabled = true;
            if (this.pagina == 1)
                this.button1.Enabled = false;
            switch (this.pagina)
            {
                case 1:
                    this.pictureBox1.Visible = true;
                    this.pictureBox2.Visible = false;
                    break;
                case 2:
                    this.pictureBox2.Visible = true;
                    this.pictureBox3.Visible = false;
                    break;
                case 3:
                    this.pictureBox3.Visible = true;
                    this.pictureBox4.Visible = false;
                    break;
            }
        }

    }
}
