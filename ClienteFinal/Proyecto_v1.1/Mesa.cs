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
using System.Media;

namespace Proyecto_v1._1
{
    public partial class Mesa : Form
    {
        //Constantes
        private const int cdWidth = 240;
        private const int cdHeight = 360;

        //Atributos
        int segundos = 0;
        Socket server;
        Image deck;
        Image back;
        int id; //Id partida
        int j;  //Tu posicion en el vector jugadores
        string[] jugadores;
        int turno = 0;
        int sentido = 1;
        List<PictureBox> ListBoxes = new List<PictureBox>();
        PictureBox cartaEnMesa;
        bool cerrar = false;
        
        //Inicializador
        public Mesa(Socket server, int id, int j, string[] jugadores)
        {
            InitializeComponent();
            this.timer1.Start();
            this.deck = Image.FromFile("deck.png");
            this.back = Image.FromFile("uno.png");
            this.server = server;
            this.id = id;
            this.jugadores = jugadores;
            this.j = j;
            this.JugadorAbajo.Text = jugadores[j];
            this.JB.Text = "Jugador " + (j + 1).ToString();

            PictureBox PicBx = new PictureBox();
            PicBx.Size = new Size(cdWidth / 2, cdHeight / 2);
            PicBx.Location = new Point(this.Width / 2 + cdWidth / 4, this.Height / 2 - cdHeight / 4);
            PicBx.Image = this.back;
            PicBx.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBx.Name = "200";
            PicBx.Click += new EventHandler(PicBx_Click);
            this.Controls.Add(PicBx);

            this.ChatBox.Items.Add("Es el turno de " + this.jugadores[this.turno]);
            this.ChatBox.TopIndex = this.ChatBox.Items.Count - 1;

            if (jugadores.Length == 2)
            {
                this.JugadorDerecha.Visible = false;
                this.JD.Visible = false;
                this.C2Lbl.Visible = false;
                this.C2Pbx.Visible = false;
                this.JugadorIzquierda.Visible = false;
                this.JI.Visible = false;
                this.C3Lbl.Visible = false;
                this.C3Pbx.Visible = false;
                this.JugadorArriba.Text = jugadores[(j + 1) % 2];
                this.JA.Text = "Jugador " + ((j + 1) % 2 + 1).ToString();

            }
            else if (jugadores.Length == 3)
            {
                this.JugadorDerecha.Visible = false;
                this.JD.Visible = false;
                this.C3Lbl.Visible = false;
                this.C3Pbx.Visible = false;
                this.JugadorIzquierda.Text = jugadores[(j + 1) % 3];
                this.JI.Text = "Jugador " + ((j + 1) % 3 + 1).ToString();
                this.JugadorArriba.Text = jugadores[(j + 2) % 3];
                this.JA.Text = "Jugador " + ((j + 2) % 3 + 1).ToString();

            }
            else if (jugadores.Length == 4)
            {
                this.JugadorIzquierda.Text = jugadores[(j + 1) % 4];
                this.JI.Text = "Jugador " + ((j + 1) % 4 + 1).ToString();
                this.JugadorArriba.Text = jugadores[(j + 2) % 4];
                this.JA.Text = "Jugador " + ((j + 2) % 4 + 1).ToString();
                this.JugadorDerecha.Text = jugadores[(j + 3) % 4];
                this.JD.Text = "Jugador " + ((j + 3) % 4 + 1).ToString();
            }
            

            this.timer1.Tick += new EventHandler(timer1_Tick);
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.segundos++;
        }

        //Metodos
        public void Cerrar(string ganador)
        {
            this.timer1.Stop();
            if (ganador == this.JugadorAbajo.Text)
            {
                string consulta = "20/" + this.id + "/" + this.segundos;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(consulta);
                server.Send(msg);
            }
            this.cerrar = true;
            this.Close();
        }

        public void MensajeRecibido(string mensaje, string jugador)
        {         
            this.ChatBox.Items.Add(jugador + ": " + mensaje);
            this.ChatBox.TopIndex = this.ChatBox.Items.Count - 1;
        }

        public void CancelarPartida()
        {
            this.cerrar = true;
            this.Close();
        }
        public void SetTurno(int turno)
        {
            this.turno = turno;
            this.ChatBox.Items.Add("Es el turno de " + this.jugadores[this.turno]);
            this.ChatBox.TopIndex = this.ChatBox.Items.Count - 1;
        }
        public void SetSentido(int sentido)
        {
            this.sentido = sentido;
        }
        public void SetNumerosC(string num)
        {
            string[] numeros = num.Split(',');
            if (jugadores.Length == 2)
                this.C1Lbl.Text = numeros[(j + 1) % 2];
            if (jugadores.Length == 3)
            {
                this.C1Lbl.Text = numeros[(j + 2) % 3];
                this.C2Lbl.Text = numeros[(j + 1) % 3];
            }
            if (jugadores.Length == 4)
            {
                this.C1Lbl.Text = numeros[(j + 2) % 4];
                this.C2Lbl.Text = numeros[(j + 1) % 4];
                this.C3Lbl.Text = numeros[(j + 3) % 4];
            }
        }

        public void SetColor()
        {
            PaletaColores paleta = new PaletaColores();
            paleta.ShowDialog();

            string mensaje = "17/" + this.id + "/" + paleta.GetColor();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        public void AnuncioColor(string color, bool anunciar)
        {
            this.ColorLbl.Text = color;
            this.panel1.Visible = anunciar;
        }

        //Añade una carta al centro de la mesa
        public void HaveCard(int num)
        {
            try { cartaEnMesa.Dispose(); }
            catch { };

            PictureBox PicBx = new PictureBox();
            PicBx.Size = new Size(cdWidth / 2, cdHeight / 2);
            PicBx.Location = new Point(this.Width / 2 - cdWidth / 2, this.Height / 2 - cdHeight / 4);

            Rectangle rect = new Rectangle(1 + cdWidth * (num % 14), 1 + cdHeight * (int)Math.Floor((double)num / 14), cdWidth, cdHeight);
            Image card = cropImage(this.deck, rect);
            PicBx.Image = card;
            PicBx.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBx.Name = num.ToString();

            cartaEnMesa = PicBx;
            this.Controls.Add(PicBx);

        }

        //Remplaza la mano del jugador
        public void HaveHand(int[] hand)
        {
            Rectangle rect;
            Image card;
            PictureBox PicBx;

            while (this.ListBoxes.Count != 0)
            {
                this.ListBoxes[0].Dispose();
                this.ListBoxes.Remove(this.ListBoxes[0]);
            }


            for (int i = 0; i < hand.Length; i++)
            {
                PicBx = new PictureBox();
                PicBx.Size = new Size(cdWidth / 2, cdHeight / 2);
                PicBx.Location = new Point((hand.Length / 112) * (cdWidth / 3) + (this.Width / (2 + hand.Length - 1)) * (i + 1) - (cdWidth / 4), this.Height - cdHeight / 2 - 50);

                rect = new Rectangle(1 + cdWidth * (hand[i] % 14), 1 + cdHeight * (int)Math.Floor((double)hand[i] / 14), cdWidth, cdHeight);
                card = cropImage(deck, rect);
                PicBx.Image = card;
                PicBx.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBx.Name = hand[i].ToString();
                PicBx.Click += new EventHandler(PicBx_Click);
                this.ListBoxes.Add(PicBx);
                this.Controls.Add(PicBx);
            }
        }

        //Funcion recortar imagen
        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        private void PicBx_Click(object sender, EventArgs e)
        {
            PictureBox target = (PictureBox)sender;
            //this.MensajeRecibido("Carta nº" + target.Name, "(consola)");

            string mensaje = "14/" + this.id + "/" + target.Name;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void EnviarBtn_Click(object sender, EventArgs e)
        {
            if (this.MensajeTxt.Text != "")
            {
                string mensaje = "12/" + this.id + "|" + this.MensajeTxt.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            this.MensajeTxt.Clear();
        }

        private void MensajeTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                this.EnviarBtn.PerformClick();
        }

        private void Mesa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.cerrar == false)
            {
                DialogResult res = MessageBox.Show("¿Estás seguro que quieres salir de la partida?", "Salir", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    string cancelar = "13/" + this.id;
                    byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(cancelar);
                    server.Send(msg2);
                }
                e.Cancel = true;
            }
        }

        private void PasoBtn_Click(object sender, EventArgs e)
        {
            string paso = "16/" + this.id;
            byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(paso);
            server.Send(msg2);
        }

        private void UnoBtn_Click(object sender, EventArgs e)
        {
            string mensaje = "18/" + this.id;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            SoundPlayer unoaudio = new SoundPlayer("unonino.wav");
            unoaudio.Play();
        }
    }
}
