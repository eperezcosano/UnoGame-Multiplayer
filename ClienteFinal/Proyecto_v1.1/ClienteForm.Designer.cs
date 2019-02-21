namespace Proyecto_v1._1
{
    partial class ClienteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClienteForm));
            this.IpTxt = new System.Windows.Forms.TextBox();
            this.IpLbl = new System.Windows.Forms.Label();
            this.PortLbl = new System.Windows.Forms.Label();
            this.PortTxt = new System.Windows.Forms.TextBox();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.UserLbl = new System.Windows.Forms.Label();
            this.PassLbl = new System.Windows.Forms.Label();
            this.UserTxt = new System.Windows.Forms.TextBox();
            this.PassTxt = new System.Windows.Forms.TextBox();
            this.EntrarBtn = new System.Windows.Forms.Button();
            this.RegBtn = new System.Windows.Forms.Button();
            this.ServerGrp = new System.Windows.Forms.GroupBox();
            this.DescBtn = new System.Windows.Forms.Button();
            this.LogGrp = new System.Windows.Forms.GroupBox();
            this.ConsultGrp = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.BajaBtn = new System.Windows.Forms.Button();
            this.ConsultNum = new System.Windows.Forms.NumericUpDown();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ConsultTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ConsultBtn = new System.Windows.Forms.Button();
            this.StatusPnl = new System.Windows.Forms.Panel();
            this.DescGrp = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ConectadosGrp = new System.Windows.Forms.GroupBox();
            this.JugarBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerGrp.SuspendLayout();
            this.LogGrp.SuspendLayout();
            this.ConsultGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsultNum)).BeginInit();
            this.DescGrp.SuspendLayout();
            this.ConectadosGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // IpTxt
            // 
            this.IpTxt.Location = new System.Drawing.Point(104, 19);
            this.IpTxt.Name = "IpTxt";
            this.IpTxt.Size = new System.Drawing.Size(150, 20);
            this.IpTxt.TabIndex = 0;
            this.IpTxt.Text = "192.168.1.10";
            // 
            // IpLbl
            // 
            this.IpLbl.AutoSize = true;
            this.IpLbl.Location = new System.Drawing.Point(17, 22);
            this.IpLbl.Name = "IpLbl";
            this.IpLbl.Size = new System.Drawing.Size(68, 13);
            this.IpLbl.TabIndex = 1;
            this.IpLbl.Text = "Dirección IP:";
            // 
            // PortLbl
            // 
            this.PortLbl.AutoSize = true;
            this.PortLbl.Location = new System.Drawing.Point(17, 46);
            this.PortLbl.Name = "PortLbl";
            this.PortLbl.Size = new System.Drawing.Size(41, 13);
            this.PortLbl.TabIndex = 2;
            this.PortLbl.Text = "Puerto:";
            // 
            // PortTxt
            // 
            this.PortTxt.Location = new System.Drawing.Point(104, 43);
            this.PortTxt.Name = "PortTxt";
            this.PortTxt.Size = new System.Drawing.Size(50, 20);
            this.PortTxt.TabIndex = 3;
            this.PortTxt.Text = "9050";
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Location = new System.Drawing.Point(160, 42);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(94, 21);
            this.ConnectBtn.TabIndex = 4;
            this.ConnectBtn.Text = "Conectar";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // UserLbl
            // 
            this.UserLbl.AutoSize = true;
            this.UserLbl.Location = new System.Drawing.Point(7, 22);
            this.UserLbl.Name = "UserLbl";
            this.UserLbl.Size = new System.Drawing.Size(58, 13);
            this.UserLbl.TabIndex = 5;
            this.UserLbl.Text = "Username:";
            // 
            // PassLbl
            // 
            this.PassLbl.AutoSize = true;
            this.PassLbl.Location = new System.Drawing.Point(7, 48);
            this.PassLbl.Name = "PassLbl";
            this.PassLbl.Size = new System.Drawing.Size(56, 13);
            this.PassLbl.TabIndex = 6;
            this.PassLbl.Text = "Password:";
            // 
            // UserTxt
            // 
            this.UserTxt.Location = new System.Drawing.Point(98, 19);
            this.UserTxt.Name = "UserTxt";
            this.UserTxt.Size = new System.Drawing.Size(156, 20);
            this.UserTxt.TabIndex = 7;
            this.UserTxt.Text = "Izan";
            // 
            // PassTxt
            // 
            this.PassTxt.Location = new System.Drawing.Point(98, 45);
            this.PassTxt.Name = "PassTxt";
            this.PassTxt.Size = new System.Drawing.Size(156, 20);
            this.PassTxt.TabIndex = 8;
            this.PassTxt.Text = "III";
            this.PassTxt.UseSystemPasswordChar = true;
            // 
            // EntrarBtn
            // 
            this.EntrarBtn.Location = new System.Drawing.Point(179, 71);
            this.EntrarBtn.Name = "EntrarBtn";
            this.EntrarBtn.Size = new System.Drawing.Size(75, 23);
            this.EntrarBtn.TabIndex = 9;
            this.EntrarBtn.Text = "Entrar";
            this.EntrarBtn.UseVisualStyleBackColor = true;
            this.EntrarBtn.Click += new System.EventHandler(this.EntrarBtn_Click);
            // 
            // RegBtn
            // 
            this.RegBtn.Location = new System.Drawing.Point(98, 71);
            this.RegBtn.Name = "RegBtn";
            this.RegBtn.Size = new System.Drawing.Size(75, 23);
            this.RegBtn.TabIndex = 10;
            this.RegBtn.Text = "Registrar";
            this.RegBtn.UseVisualStyleBackColor = true;
            this.RegBtn.Click += new System.EventHandler(this.RegBtn_Click);
            // 
            // ServerGrp
            // 
            this.ServerGrp.Controls.Add(this.IpLbl);
            this.ServerGrp.Controls.Add(this.IpTxt);
            this.ServerGrp.Controls.Add(this.PortLbl);
            this.ServerGrp.Controls.Add(this.PortTxt);
            this.ServerGrp.Controls.Add(this.ConnectBtn);
            this.ServerGrp.Location = new System.Drawing.Point(12, 12);
            this.ServerGrp.Name = "ServerGrp";
            this.ServerGrp.Size = new System.Drawing.Size(260, 74);
            this.ServerGrp.TabIndex = 11;
            this.ServerGrp.TabStop = false;
            this.ServerGrp.Text = "Servidor";
            // 
            // DescBtn
            // 
            this.DescBtn.Location = new System.Drawing.Point(104, 13);
            this.DescBtn.Name = "DescBtn";
            this.DescBtn.Size = new System.Drawing.Size(150, 21);
            this.DescBtn.TabIndex = 5;
            this.DescBtn.Text = "Desconectar";
            this.DescBtn.UseVisualStyleBackColor = true;
            this.DescBtn.Click += new System.EventHandler(this.DescBtn_Click);
            // 
            // LogGrp
            // 
            this.LogGrp.Controls.Add(this.UserTxt);
            this.LogGrp.Controls.Add(this.UserLbl);
            this.LogGrp.Controls.Add(this.RegBtn);
            this.LogGrp.Controls.Add(this.PassLbl);
            this.LogGrp.Controls.Add(this.EntrarBtn);
            this.LogGrp.Controls.Add(this.PassTxt);
            this.LogGrp.Enabled = false;
            this.LogGrp.Location = new System.Drawing.Point(12, 138);
            this.LogGrp.Name = "LogGrp";
            this.LogGrp.Size = new System.Drawing.Size(260, 102);
            this.LogGrp.TabIndex = 12;
            this.LogGrp.TabStop = false;
            this.LogGrp.Text = "Acceso Usuario";
            // 
            // ConsultGrp
            // 
            this.ConsultGrp.Controls.Add(this.button1);
            this.ConsultGrp.Controls.Add(this.BajaBtn);
            this.ConsultGrp.Controls.Add(this.ConsultNum);
            this.ConsultGrp.Controls.Add(this.radioButton3);
            this.ConsultGrp.Controls.Add(this.radioButton2);
            this.ConsultGrp.Controls.Add(this.radioButton1);
            this.ConsultGrp.Controls.Add(this.label6);
            this.ConsultGrp.Controls.Add(this.label5);
            this.ConsultGrp.Controls.Add(this.label4);
            this.ConsultGrp.Controls.Add(this.ConsultTxt);
            this.ConsultGrp.Controls.Add(this.label3);
            this.ConsultGrp.Controls.Add(this.label2);
            this.ConsultGrp.Controls.Add(this.label1);
            this.ConsultGrp.Controls.Add(this.ConsultBtn);
            this.ConsultGrp.Enabled = false;
            this.ConsultGrp.Location = new System.Drawing.Point(278, 13);
            this.ConsultGrp.Name = "ConsultGrp";
            this.ConsultGrp.Size = new System.Drawing.Size(226, 227);
            this.ConsultGrp.TabIndex = 13;
            this.ConsultGrp.TabStop = false;
            this.ConsultGrp.Text = "Consultar";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Reglas del Juego";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BajaBtn
            // 
            this.BajaBtn.Location = new System.Drawing.Point(17, 195);
            this.BajaBtn.Name = "BajaBtn";
            this.BajaBtn.Size = new System.Drawing.Size(90, 23);
            this.BajaBtn.TabIndex = 11;
            this.BajaBtn.Text = "Darse de Baja";
            this.BajaBtn.UseVisualStyleBackColor = true;
            this.BajaBtn.Click += new System.EventHandler(this.BajaBtn_Click);
            // 
            // ConsultNum
            // 
            this.ConsultNum.Enabled = false;
            this.ConsultNum.Location = new System.Drawing.Point(44, 142);
            this.ConsultNum.Name = "ConsultNum";
            this.ConsultNum.Size = new System.Drawing.Size(63, 20);
            this.ConsultNum.TabIndex = 27;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(17, 124);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(14, 13);
            this.radioButton3.TabIndex = 26;
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(17, 72);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(14, 13);
            this.radioButton2.TabIndex = 25;
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 30);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(14, 13);
            this.radioButton1.TabIndex = 24;
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "que han jugado en la partida ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Dame los nombres de los jugadores";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "máxima puntuación";
            // 
            // ConsultTxt
            // 
            this.ConsultTxt.Enabled = false;
            this.ConsultTxt.Location = new System.Drawing.Point(40, 82);
            this.ConsultTxt.Name = "ConsultTxt";
            this.ConsultTxt.Size = new System.Drawing.Size(67, 20);
            this.ConsultTxt.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "que ha durado más del jugador:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Dame máxima duración de la partida";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Dame el nombre del jugador con";
            // 
            // ConsultBtn
            // 
            this.ConsultBtn.Location = new System.Drawing.Point(17, 168);
            this.ConsultBtn.Name = "ConsultBtn";
            this.ConsultBtn.Size = new System.Drawing.Size(198, 22);
            this.ConsultBtn.TabIndex = 15;
            this.ConsultBtn.Text = "Consultar";
            this.ConsultBtn.UseVisualStyleBackColor = true;
            this.ConsultBtn.Click += new System.EventHandler(this.ConsultBtn_Click);
            // 
            // StatusPnl
            // 
            this.StatusPnl.BackColor = System.Drawing.Color.Red;
            this.StatusPnl.Location = new System.Drawing.Point(78, 14);
            this.StatusPnl.Name = "StatusPnl";
            this.StatusPnl.Size = new System.Drawing.Size(20, 20);
            this.StatusPnl.TabIndex = 14;
            // 
            // DescGrp
            // 
            this.DescGrp.Controls.Add(this.label7);
            this.DescGrp.Controls.Add(this.StatusPnl);
            this.DescGrp.Controls.Add(this.DescBtn);
            this.DescGrp.Enabled = false;
            this.DescGrp.Location = new System.Drawing.Point(12, 89);
            this.DescGrp.Name = "DescGrp";
            this.DescGrp.Size = new System.Drawing.Size(260, 43);
            this.DescGrp.TabIndex = 15;
            this.DescGrp.TabStop = false;
            this.DescGrp.Text = "Desconectar";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Estado:";
            // 
            // ConectadosGrp
            // 
            this.ConectadosGrp.Controls.Add(this.JugarBtn);
            this.ConectadosGrp.Controls.Add(this.dataGridView1);
            this.ConectadosGrp.Enabled = false;
            this.ConectadosGrp.Location = new System.Drawing.Point(510, 13);
            this.ConectadosGrp.Name = "ConectadosGrp";
            this.ConectadosGrp.Size = new System.Drawing.Size(162, 227);
            this.ConectadosGrp.TabIndex = 16;
            this.ConectadosGrp.TabStop = false;
            this.ConectadosGrp.Text = "Lista Conectados";
            // 
            // JugarBtn
            // 
            this.JugarBtn.BackColor = System.Drawing.Color.Transparent;
            this.JugarBtn.Enabled = false;
            this.JugarBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugarBtn.Location = new System.Drawing.Point(6, 177);
            this.JugarBtn.Name = "JugarBtn";
            this.JugarBtn.Size = new System.Drawing.Size(150, 41);
            this.JugarBtn.TabIndex = 3;
            this.JugarBtn.Text = "Invitar a Jugar";
            this.JugarBtn.UseVisualStyleBackColor = false;
            this.JugarBtn.Click += new System.EventHandler(this.JugarBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nombre});
            this.dataGridView1.Location = new System.Drawing.Point(6, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(150, 154);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Nombre
            // 
            this.Nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Nombre.HeaderText = "Nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            this.Nombre.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Nombre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClienteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(677, 245);
            this.Controls.Add(this.ConectadosGrp);
            this.Controls.Add(this.DescGrp);
            this.Controls.Add(this.ConsultGrp);
            this.Controls.Add(this.LogGrp);
            this.Controls.Add(this.ServerGrp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClienteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log In";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClienteForm_FormClosing);
            this.ServerGrp.ResumeLayout(false);
            this.ServerGrp.PerformLayout();
            this.LogGrp.ResumeLayout(false);
            this.LogGrp.PerformLayout();
            this.ConsultGrp.ResumeLayout(false);
            this.ConsultGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsultNum)).EndInit();
            this.DescGrp.ResumeLayout(false);
            this.DescGrp.PerformLayout();
            this.ConectadosGrp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox IpTxt;
        private System.Windows.Forms.Label IpLbl;
        private System.Windows.Forms.Label PortLbl;
        private System.Windows.Forms.TextBox PortTxt;
        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.Label UserLbl;
        private System.Windows.Forms.Label PassLbl;
        private System.Windows.Forms.TextBox UserTxt;
        private System.Windows.Forms.TextBox PassTxt;
        private System.Windows.Forms.Button EntrarBtn;
        private System.Windows.Forms.Button RegBtn;
        private System.Windows.Forms.GroupBox ServerGrp;
        private System.Windows.Forms.Button DescBtn;
        private System.Windows.Forms.GroupBox LogGrp;
        private System.Windows.Forms.GroupBox ConsultGrp;
        private System.Windows.Forms.Button ConsultBtn;
        private System.Windows.Forms.NumericUpDown ConsultNum;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ConsultTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel StatusPnl;
        private System.Windows.Forms.GroupBox DescGrp;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox ConectadosGrp;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.Button JugarBtn;
        private System.Windows.Forms.Button BajaBtn;
        private System.Windows.Forms.Button button1;
    }
}

