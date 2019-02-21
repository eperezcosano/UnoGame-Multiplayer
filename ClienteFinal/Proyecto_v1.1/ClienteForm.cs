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
using System.Threading;

namespace Proyecto_v1._1
{
    public partial class ClienteForm : Form
    {
        //Atributos
        Socket server;
        Thread atender;
        Thread threadMesa;
        Mesa mesa;

        //Delegados
        delegate void DelegadoParaCerrar(string info);
        delegate void DelegadoParaColor();
        delegate void DelegadoParaDesconectar();
        delegate void DelegadoParaSiguienteTurno(string info);
        delegate void DelegadoParaPonerMano(string info);
        delegate void DelegadoParaDataGridView(string info);
        delegate void DelegadoParaHabilitarCuandoConectado();
        delegate void DelegadoParaPonerEnEspera();
        delegate void DelegadoParaPonerEstadoInicial();
        delegate void DelegadoParaRecibirMensaje(string info);
        delegate void DelegadoCancelar();

        private void Cerrar(string mensaje)
        {
            this.mesa.Cerrar(mensaje);
        }

        private void ColorCarta()
        {
            this.mesa.SetColor();
        }

        private void Desconectar()
        {
            this.DescBtn.PerformClick();
        }

        private void SiguienteTurno(string mensaje)
        {
            string[] trozos = mensaje.Split(',');
            int o = 0;
            bool a = false;
            int carta = Convert.ToInt32(trozos[0]);
            if (carta % 14 == 13)
            {
                o = 1;
                a = true;
            }
            this.mesa.HaveCard(carta);
            this.mesa.AnuncioColor(trozos[o], a);
            this.mesa.SetTurno(Convert.ToInt32(trozos[1+o]));
            this.mesa.SetSentido(Convert.ToInt32(trozos[2+o]));
            string cantidadcartas = "";
            for (int i = (3+o); i < trozos.Length; i++)
                cantidadcartas += trozos[i] + ",";
            this.mesa.SetNumerosC(cantidadcartas);
        }

        private void PonerMano(string mensaje)
        {
            int[] hand = Array.ConvertAll(mensaje.Split(','), int.Parse);
            this.mesa.HaveHand(hand);
        }

        private void CancelarPartida()
        {
            this.mesa.CancelarPartida();
            //threadMesa.Abort();
        }

        //Entregar el mensaje al tablero
        private void RecibirMensaje(string mensaje)
        {
            string[] trozos = mensaje.Split('|');
            this.mesa.MensajeRecibido(trozos[1], trozos[0]);
        }

        //Cambiar el boton de invitar a su estado inicial por defecto.
        private void PonerEstadoInicial()
        {
            this.JugarBtn.Text = "Invitar a Jugar";
            this.JugarBtn.BackColor = Color.Transparent;
            this.JugarBtn.Enabled = false;
            this.dataGridView1.Enabled = true;
        }
        
        //Cambiar el boton de invitar al estado de espera.
        private void PonerEnEspera()
        {
            this.JugarBtn.Text = "Esperando...";
            this.JugarBtn.BackColor = Color.Transparent;
            this.JugarBtn.Enabled = false;
            this.dataGridView1.Enabled = false;
        }

        //Habilitar ciertos campos del formulario cuando el usuario ya ha entrado
        private void HabilitarCuandoConectado()
        {
            this.LogGrp.Enabled = false;
            //this.BajaBtn.Enabled = true;
            this.ConsultGrp.Enabled = true;
            this.ConectadosGrp.Enabled = true;
        }

        //Actualizar el dataGridView con los nuevos usuarios conectados
        private void PonDataGridView(string conectados)
        {
            string[] subtrozos = conectados.Split(',');
            int num = Convert.ToInt32(subtrozos[0]);

            this.dataGridView1.Rows.Clear();

            for (int i = 0; i < num; i++)
            {
                this.dataGridView1.Rows.Insert(i, subtrozos[i + 1]);
                if (subtrozos[i + 1] == UserTxt.Text)
                    this.dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
            }
            this.dataGridView1.ClearSelection();
        }

        //Abre el formulario del tablero
        private void AbrirMesa(int id, string[] jugadores, int cartaEnMesa, int[] mano)
        {
            for (int i = 0; i < jugadores.Length; i++)
            {
                if (jugadores[i] == this.UserTxt.Text)
                {
                    this.mesa = new Mesa(this.server, id, i, jugadores);
                    break;
                }
            }
            this.mesa.HaveCard(cartaEnMesa);
            this.mesa.HaveHand(mano);
            this.mesa.ShowDialog();
        }

        public ClienteForm()
        {
            InitializeComponent();
        }

        //Conexión al servidor
        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress dir = IPAddress.Parse(IpTxt.Text);
                IPEndPoint ipep = new IPEndPoint(dir, Convert.ToInt32(PortTxt.Text));

                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ipep);
                MessageBox.Show("Conectado correctamente");
            }
            catch (FormatException)
            {
                MessageBox.Show("Dirección inválida");
                return;
            }
            catch (SocketException)
            {
                MessageBox.Show("Error en la conexión del servidor");
                return;
            }

            //Iniciamos Thread de Recepción
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();

            this.ServerGrp.Enabled = false;
            this.DescGrp.Enabled = true;
            this.StatusPnl.BackColor = Color.Green;
            this.LogGrp.Enabled = true;
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] response = new byte[80];
                server.Receive(response);
                string[] trozos = Encoding.ASCII.GetString(response).Split('/');

                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];
                
                switch (codigo)
                {
                    //Entrar
                    case 1:
                        switch (mensaje)
                        {
                            case "0":
                                MessageBox.Show("Has entrado correctamente");
                                this.Invoke(new DelegadoParaHabilitarCuandoConectado(HabilitarCuandoConectado)); 
                                break;
                            case "-1":
                                MessageBox.Show("Error usuario o contraseña incorrectos");
                                break;
                            default:
                                MessageBox.Show("Error en la base de datos");
                                break;
                        }
                        break;
                    //Registro
                    case 2:
                        switch (mensaje)
                        {
                            case "0":
                                MessageBox.Show("Te has registrado correctamente");
                                break;
                            case "1":
                                MessageBox.Show("Ya estás registrado");
                                break;
                            default:
                                MessageBox.Show("Error en la base de datos");
                                break;
                        }
                        break;
                    //Consulta1
                    case 3:
                        switch (mensaje)
                        {
                            case "-1":
                                MessageBox.Show("No se han obtenido ningún dato");
                                break;
                            case "-2":
                                MessageBox.Show("Error en la base de datos");
                                break;
                            default:
                                MessageBox.Show("El jugador con máxima puntuación es: " + mensaje);
                                break;
                        }
                        break;
                    //Consulta2
                    case 4:
                        switch (mensaje)
                        {
                            case "-1":
                                MessageBox.Show("No se han obtenido ningún dato");
                                break;
                            case "-2":
                                MessageBox.Show("Error en la base de datos");
                                break;
                            default:
                                MessageBox.Show("La máxima duración es: " + mensaje);
                                break;
                        }
                        break;
                    //Consulta3
                    case 5:
                        switch (mensaje)
                        {
                            case "-1":
                                MessageBox.Show("No existe ninguna partida con esa ID");
                                break;
                            case "-2":
                                MessageBox.Show("Error en la base de datos");
                                break;
                            default:
                                MessageBox.Show("Los jugadores son: " + mensaje);
                                break;
                        }
                        break;
                    //Notificacion de lista Conectados
                    case 6:
                        this.dataGridView1.Invoke(new DelegadoParaDataGridView(PonDataGridView), new Object[] { mensaje });
                        break;
                    case 7:
                        if (Convert.ToInt32(mensaje) == 0)
                        {
                            MessageBox.Show("Te has dado de baja.");
                            this.DescBtn.Invoke(new DelegadoParaDesconectar(Desconectar));
                        }
                        else
                            MessageBox.Show("No se ha podido eliminar usuario");
                        break;
                    //Recepcion Invitacion (3)
                    case 10:
                        string[] invitacion = mensaje.Split(',');
                        int idPartida = Convert.ToInt32(invitacion[0]);
                        string anfitrion = invitacion[1];
                        string oponentes = "";
                        for (int i = 2; i < invitacion.Length; i++)
                        {
                            if (invitacion[i] != this.UserTxt.Text)
                            {
                                oponentes += invitacion[i] + ", ";
                            }
                        }
                        string texto = anfitrion + " te ha invitado a jugar.\n";
                        if (oponentes != "")
                            texto += "Junto con: " + oponentes.Remove(oponentes.Length - 2) + ".\n";
                        texto += "¿Deseas aceptar la invitación?";
                        DialogResult res = MessageBox.Show(texto, "Invitación", MessageBoxButtons.YesNo);

                        switch (res) //Respuesta Invitacion (4)
                        {
                            case DialogResult.Yes:
                                string aceptar = "11/" + idPartida + "/1";
                                byte[] msg1 = System.Text.Encoding.ASCII.GetBytes(aceptar);
                                server.Send(msg1);
                                this.Invoke(new DelegadoParaPonerEnEspera(PonerEnEspera));
                                break;
                            case DialogResult.No:
                                string cancelar = "11/" + idPartida + "/0";
                                byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(cancelar);
                                server.Send(msg2);
                                break;
                        }
                        break;
                    case 11: //Partida creada o rechazada
                        string[] fragmentos = mensaje.Split(',');
                        int idP = Convert.ToInt32(fragmentos[0]);
                        int aceptado = Convert.ToInt32(fragmentos[1]);
                        if (aceptado == 1)
                        {
                            int numJ = Convert.ToInt32(fragmentos[2]);
                            int cartaEnMesa = Convert.ToInt32(fragmentos[3]);

                            string[] jugadores = new string[numJ];
                            jugadores[0] = fragmentos[4];
                            if (numJ > 1)
                                jugadores[1] = fragmentos[5];
                            if (numJ > 2)
                                jugadores[2] = fragmentos[6];
                            if (numJ > 3)
                                jugadores[3] = fragmentos[7];

                            int[] mano = new int[7];
                            for (int i = 0; i < 7; i++)
                            {
                                mano[i] = Convert.ToInt32(fragmentos[numJ + i + 4]);
                            }
                            ThreadStart ts = delegate { AbrirMesa(idP, jugadores, cartaEnMesa, mano); };
                            threadMesa = new Thread(ts);
                            threadMesa.Start();
                        }
                        else
                        {
                            MessageBox.Show("Partida cancelada");
                            this.Invoke(new DelegadoParaPonerEstadoInicial(PonerEstadoInicial));
                        }
                        break;
                    case 12: // Recepción del mensaje chat y entrega al tablero
                        this.mesa.Invoke(new DelegadoParaRecibirMensaje(RecibirMensaje), new object[] { mensaje });
                        break;
                    case 13: //Cierre partida repentino
                        this.mesa.Invoke(new DelegadoCancelar(CancelarPartida));
                        MessageBox.Show("Alguno de tus oponentes ha abandonado la partida");
                        this.Invoke(new DelegadoParaPonerEstadoInicial(PonerEstadoInicial));
                        break;
                    case 14: //Notificacion nueva juagda realizada
                        this.mesa.Invoke(new DelegadoParaSiguienteTurno(SiguienteTurno), new object[] { mensaje });
                        break;
                    case 15: //Nueva mano
                        this.mesa.Invoke(new DelegadoParaPonerMano(PonerMano), new object[] { mensaje });
                        break;
                    case 17: //Elegir Color
                        this.mesa.Invoke(new DelegadoParaColor(ColorCarta));
                        break;
                    case 19: //Ganador
                        this.mesa.Invoke(new DelegadoParaCerrar(Cerrar), new object[] { mensaje });
                        MessageBox.Show("Ha ganado " + mensaje);
                        break;
                    default:
                        break;
                }
            }
        }

        //Entrar una vez ya registrado.
        private void EntrarBtn_Click(object sender, EventArgs e)
        {
            //Comprobar datos válidos
            if (UserTxt.TextLength == 0 || UserTxt.TextLength > 20 || PassTxt.TextLength == 0 || PassTxt.TextLength > 20)
            {
                MessageBox.Show("Datos inválidos");
                return;
            }

            //Preparamos la petición
            string mensaje = "1/" + UserTxt.Text + "," + PassTxt.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        //Registro usuario
        private void RegBtn_Click(object sender, EventArgs e)
        {
            //Comprobación datos válidos
            if (UserTxt.TextLength == 0 || UserTxt.TextLength > 20 || UserTxt.Text == "-1" || UserTxt.Text == "-2" ||
                PassTxt.TextLength == 0 || PassTxt.TextLength > 20)
            {
                MessageBox.Show("Datos inválidos");
                return;
            }

            //Preparamos la petición
            string mensaje = "2/" + UserTxt.Text + "," + PassTxt.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        //Deshabilitar campo de texto (Consulta 2)
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
                ConsultTxt.Enabled = true;
            else
                ConsultTxt.Enabled = false;
        }

        //Deshabilitar campo numerico (Consulta 3)
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                ConsultNum.Enabled = true;
            else
                ConsultNum.Enabled = false;
        }

        //Desconectarse del servidor
        private void DescBtn_Click(object sender, EventArgs e)
        {
            string mensaje = "0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Cerramos Thread
            atender.Abort();

            try
            {
                mesa.Close();
                threadMesa.Abort();
            }
            catch (NullReferenceException) { }

            //Cerramos conexión
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            MessageBox.Show("Desconectado correctamente");

            this.ConsultGrp.Enabled = false;
            this.LogGrp.Enabled = false;
            this.DescGrp.Enabled = false;
            this.StatusPnl.BackColor = Color.Red;
            this.ServerGrp.Enabled = true;
            this.ConectadosGrp.Enabled = false;
            this.dataGridView1.Rows.Clear();
            //this.UserTxt.Clear();
            //this.PassTxt.Clear();
            this.JugarBtn.Enabled = false;
            //this.BajaBtn.Enabled = false;
            this.JugarBtn.BackColor = Color.Transparent;
        }

        //Boton consultar. Consultas 1, 2 y 3
        private void ConsultBtn_Click(object sender, EventArgs e)
        {
            //Consulta 1
            if (radioButton1.Checked == true)
            {
                string mensaje = "3/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }

            //Consulta 2
            if (radioButton2.Checked == true)
            {
                if (ConsultTxt.TextLength == 0)
                {
                    MessageBox.Show("Datos inválidos\n");
                    return;
                }

                string mensaje = "4/" + ConsultTxt.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }

            //Consulta 3
            if (radioButton3.Checked == true)
            {
                if (ConsultNum.Value == 0)
                {
                    MessageBox.Show("Datos inválidos\n");
                    return;
                }

                string mensaje = "5/" + ConsultNum.Value;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
        }

        //Desconexión del servidor si se cierra el cliente.
        private void ClienteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DescGrp.Enabled == true)
                this.DescBtn.PerformClick();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == UserTxt.Text)
            {
                this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = false;
            }
            if (this.dataGridView1.SelectedCells.Count > 0 && this.dataGridView1.SelectedCells.Count < 4)
            {
                this.JugarBtn.Enabled = true;
                this.JugarBtn.BackColor = Color.Lime;
            }
            else
            {
                this.JugarBtn.Enabled = false;
                this.JugarBtn.BackColor = Color.Transparent;
            }
        }

        private void JugarBtn_Click(object sender, EventArgs e) //Invitar a Jugar (1)
        {
            //Validar los datos
            int n = this.dataGridView1.SelectedCells.Count;

            if (n != 1 && n != 2 && n != 3)
            {
                MessageBox.Show("Numero de jugadores inválido.\n (Entre 2 y 4 Jugadores)");
                return;
            }

            //Generar string de invitados
            n = this.dataGridView1.Rows.Count;
            string lineaoponentes = "";
            for (int i = 0; i < n; i++)
            {
                if (this.dataGridView1.Rows[i].Cells["Nombre"].Selected == true && this.dataGridView1.Rows[i].Cells["Nombre"].Value.ToString() != this.UserTxt.Text)
                    lineaoponentes += this.dataGridView1.Rows[i].Cells["Nombre"].Value.ToString() + ",";
            }

            lineaoponentes = lineaoponentes.Remove(lineaoponentes.Length - 1);

            //Mensaje hacia el servidor: 10/string de invitados
            string mensaje = "10/" + lineaoponentes;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            this.dataGridView1.ClearSelection();
            this.JugarBtn.Text = "Esperando...";
            this.JugarBtn.BackColor = Color.Transparent;
            this.JugarBtn.Enabled = false;
            this.dataGridView1.Enabled = false;
        }

        private void BajaBtn_Click(object sender, EventArgs e)
        {
            string baja = "7/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(baja);
            server.Send(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NormasForm normas = new NormasForm();
            normas.ShowDialog();
        }
    }
}