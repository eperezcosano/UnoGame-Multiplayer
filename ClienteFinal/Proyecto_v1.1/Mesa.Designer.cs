namespace Proyecto_v1._1
{
    partial class Mesa
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mesa));
            this.MensajeTxt = new System.Windows.Forms.TextBox();
            this.EnviarBtn = new System.Windows.Forms.Button();
            this.ChatBox = new System.Windows.Forms.ListBox();
            this.ChatGrp = new System.Windows.Forms.GroupBox();
            this.JugadorArriba = new System.Windows.Forms.Label();
            this.JugadorAbajo = new System.Windows.Forms.Label();
            this.JugadorDerecha = new System.Windows.Forms.Label();
            this.JugadorIzquierda = new System.Windows.Forms.Label();
            this.JA = new System.Windows.Forms.Label();
            this.JD = new System.Windows.Forms.Label();
            this.JB = new System.Windows.Forms.Label();
            this.JI = new System.Windows.Forms.Label();
            this.PasoBtn = new System.Windows.Forms.Button();
            this.C1Pbx = new System.Windows.Forms.PictureBox();
            this.C1Lbl = new System.Windows.Forms.Label();
            this.C2Lbl = new System.Windows.Forms.Label();
            this.C2Pbx = new System.Windows.Forms.PictureBox();
            this.C3Pbx = new System.Windows.Forms.PictureBox();
            this.C3Lbl = new System.Windows.Forms.Label();
            this.UnoBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.ColorLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ChatGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.C1Pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.C2Pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.C3Pbx)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MensajeTxt
            // 
            this.MensajeTxt.Location = new System.Drawing.Point(12, 104);
            this.MensajeTxt.Name = "MensajeTxt";
            this.MensajeTxt.Size = new System.Drawing.Size(163, 20);
            this.MensajeTxt.TabIndex = 0;
            this.MensajeTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MensajeTxt_KeyPress);
            // 
            // EnviarBtn
            // 
            this.EnviarBtn.Location = new System.Drawing.Point(180, 104);
            this.EnviarBtn.Name = "EnviarBtn";
            this.EnviarBtn.Size = new System.Drawing.Size(75, 20);
            this.EnviarBtn.TabIndex = 1;
            this.EnviarBtn.Text = "Enviar";
            this.EnviarBtn.UseVisualStyleBackColor = true;
            this.EnviarBtn.Click += new System.EventHandler(this.EnviarBtn_Click);
            // 
            // ChatBox
            // 
            this.ChatBox.BackColor = System.Drawing.Color.White;
            this.ChatBox.FormattingEnabled = true;
            this.ChatBox.Location = new System.Drawing.Point(12, 16);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(243, 82);
            this.ChatBox.TabIndex = 2;
            // 
            // ChatGrp
            // 
            this.ChatGrp.BackColor = System.Drawing.Color.Transparent;
            this.ChatGrp.Controls.Add(this.ChatBox);
            this.ChatGrp.Controls.Add(this.MensajeTxt);
            this.ChatGrp.Controls.Add(this.EnviarBtn);
            this.ChatGrp.Location = new System.Drawing.Point(922, 12);
            this.ChatGrp.Name = "ChatGrp";
            this.ChatGrp.Size = new System.Drawing.Size(261, 133);
            this.ChatGrp.TabIndex = 3;
            this.ChatGrp.TabStop = false;
            // 
            // JugadorArriba
            // 
            this.JugadorArriba.AutoSize = true;
            this.JugadorArriba.BackColor = System.Drawing.Color.Transparent;
            this.JugadorArriba.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorArriba.ForeColor = System.Drawing.Color.White;
            this.JugadorArriba.Location = new System.Drawing.Point(329, 67);
            this.JugadorArriba.Name = "JugadorArriba";
            this.JugadorArriba.Size = new System.Drawing.Size(97, 16);
            this.JugadorArriba.TabIndex = 5;
            this.JugadorArriba.Text = "JugadorArriba";
            // 
            // JugadorAbajo
            // 
            this.JugadorAbajo.AutoSize = true;
            this.JugadorAbajo.BackColor = System.Drawing.Color.Transparent;
            this.JugadorAbajo.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorAbajo.ForeColor = System.Drawing.Color.White;
            this.JugadorAbajo.Location = new System.Drawing.Point(407, 431);
            this.JugadorAbajo.Name = "JugadorAbajo";
            this.JugadorAbajo.Size = new System.Drawing.Size(96, 16);
            this.JugadorAbajo.TabIndex = 6;
            this.JugadorAbajo.Text = "JugadorAbajo";
            // 
            // JugadorDerecha
            // 
            this.JugadorDerecha.AutoSize = true;
            this.JugadorDerecha.BackColor = System.Drawing.Color.Transparent;
            this.JugadorDerecha.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorDerecha.ForeColor = System.Drawing.Color.White;
            this.JugadorDerecha.Location = new System.Drawing.Point(853, 237);
            this.JugadorDerecha.Name = "JugadorDerecha";
            this.JugadorDerecha.Size = new System.Drawing.Size(113, 16);
            this.JugadorDerecha.TabIndex = 8;
            this.JugadorDerecha.Text = "JugadorDerecha";
            // 
            // JugadorIzquierda
            // 
            this.JugadorIzquierda.AutoSize = true;
            this.JugadorIzquierda.BackColor = System.Drawing.Color.Transparent;
            this.JugadorIzquierda.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JugadorIzquierda.ForeColor = System.Drawing.Color.White;
            this.JugadorIzquierda.Location = new System.Drawing.Point(42, 185);
            this.JugadorIzquierda.Name = "JugadorIzquierda";
            this.JugadorIzquierda.Size = new System.Drawing.Size(119, 16);
            this.JugadorIzquierda.TabIndex = 7;
            this.JugadorIzquierda.Text = "JugadorIzquierda";
            // 
            // JA
            // 
            this.JA.AutoSize = true;
            this.JA.BackColor = System.Drawing.Color.Transparent;
            this.JA.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JA.ForeColor = System.Drawing.Color.White;
            this.JA.Location = new System.Drawing.Point(329, 83);
            this.JA.Name = "JA";
            this.JA.Size = new System.Drawing.Size(25, 16);
            this.JA.TabIndex = 10;
            this.JA.Text = "JA";
            // 
            // JD
            // 
            this.JD.AutoSize = true;
            this.JD.BackColor = System.Drawing.Color.Transparent;
            this.JD.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JD.ForeColor = System.Drawing.Color.White;
            this.JD.Location = new System.Drawing.Point(853, 256);
            this.JD.Name = "JD";
            this.JD.Size = new System.Drawing.Size(25, 16);
            this.JD.TabIndex = 11;
            this.JD.Text = "JD";
            // 
            // JB
            // 
            this.JB.AutoSize = true;
            this.JB.BackColor = System.Drawing.Color.Transparent;
            this.JB.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JB.ForeColor = System.Drawing.Color.White;
            this.JB.Location = new System.Drawing.Point(407, 447);
            this.JB.Name = "JB";
            this.JB.Size = new System.Drawing.Size(25, 16);
            this.JB.TabIndex = 12;
            this.JB.Text = "JB";
            // 
            // JI
            // 
            this.JI.AutoSize = true;
            this.JI.BackColor = System.Drawing.Color.Transparent;
            this.JI.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JI.ForeColor = System.Drawing.Color.White;
            this.JI.Location = new System.Drawing.Point(42, 201);
            this.JI.Name = "JI";
            this.JI.Size = new System.Drawing.Size(21, 16);
            this.JI.TabIndex = 13;
            this.JI.Text = "JI";
            // 
            // PasoBtn
            // 
            this.PasoBtn.BackColor = System.Drawing.Color.White;
            this.PasoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PasoBtn.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasoBtn.Location = new System.Drawing.Point(922, 161);
            this.PasoBtn.Name = "PasoBtn";
            this.PasoBtn.Size = new System.Drawing.Size(96, 47);
            this.PasoBtn.TabIndex = 14;
            this.PasoBtn.Text = "Paso";
            this.PasoBtn.UseVisualStyleBackColor = false;
            this.PasoBtn.Click += new System.EventHandler(this.PasoBtn_Click);
            // 
            // C1Pbx
            // 
            this.C1Pbx.Image = ((System.Drawing.Image)(resources.GetObject("C1Pbx.Image")));
            this.C1Pbx.Location = new System.Drawing.Point(451, 12);
            this.C1Pbx.Name = "C1Pbx";
            this.C1Pbx.Size = new System.Drawing.Size(120, 180);
            this.C1Pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.C1Pbx.TabIndex = 15;
            this.C1Pbx.TabStop = false;
            // 
            // C1Lbl
            // 
            this.C1Lbl.AutoSize = true;
            this.C1Lbl.BackColor = System.Drawing.Color.Black;
            this.C1Lbl.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C1Lbl.ForeColor = System.Drawing.Color.White;
            this.C1Lbl.Location = new System.Drawing.Point(460, 28);
            this.C1Lbl.Name = "C1Lbl";
            this.C1Lbl.Size = new System.Drawing.Size(30, 29);
            this.C1Lbl.TabIndex = 16;
            this.C1Lbl.Text = "7";
            // 
            // C2Lbl
            // 
            this.C2Lbl.AutoSize = true;
            this.C2Lbl.BackColor = System.Drawing.Color.Black;
            this.C2Lbl.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C2Lbl.ForeColor = System.Drawing.Color.White;
            this.C2Lbl.Location = new System.Drawing.Point(181, 241);
            this.C2Lbl.Name = "C2Lbl";
            this.C2Lbl.Size = new System.Drawing.Size(30, 29);
            this.C2Lbl.TabIndex = 18;
            this.C2Lbl.Text = "7";
            // 
            // C2Pbx
            // 
            this.C2Pbx.Image = ((System.Drawing.Image)(resources.GetObject("C2Pbx.Image")));
            this.C2Pbx.Location = new System.Drawing.Point(45, 230);
            this.C2Pbx.Name = "C2Pbx";
            this.C2Pbx.Size = new System.Drawing.Size(180, 120);
            this.C2Pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.C2Pbx.TabIndex = 17;
            this.C2Pbx.TabStop = false;
            // 
            // C3Pbx
            // 
            this.C3Pbx.Image = ((System.Drawing.Image)(resources.GetObject("C3Pbx.Image")));
            this.C3Pbx.Location = new System.Drawing.Point(856, 275);
            this.C3Pbx.Name = "C3Pbx";
            this.C3Pbx.Size = new System.Drawing.Size(180, 120);
            this.C3Pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.C3Pbx.TabIndex = 19;
            this.C3Pbx.TabStop = false;
            // 
            // C3Lbl
            // 
            this.C3Lbl.AutoSize = true;
            this.C3Lbl.BackColor = System.Drawing.Color.Black;
            this.C3Lbl.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C3Lbl.ForeColor = System.Drawing.Color.White;
            this.C3Lbl.Location = new System.Drawing.Point(871, 357);
            this.C3Lbl.Name = "C3Lbl";
            this.C3Lbl.Size = new System.Drawing.Size(30, 29);
            this.C3Lbl.TabIndex = 20;
            this.C3Lbl.Text = "7";
            // 
            // UnoBtn
            // 
            this.UnoBtn.BackColor = System.Drawing.Color.Red;
            this.UnoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnoBtn.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnoBtn.ForeColor = System.Drawing.Color.White;
            this.UnoBtn.Location = new System.Drawing.Point(1024, 161);
            this.UnoBtn.Name = "UnoBtn";
            this.UnoBtn.Size = new System.Drawing.Size(153, 47);
            this.UnoBtn.TabIndex = 21;
            this.UnoBtn.Text = "¡UNO!";
            this.UnoBtn.UseVisualStyleBackColor = false;
            this.UnoBtn.Click += new System.EventHandler(this.UnoBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.ColorLbl);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(243, 275);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(195, 51);
            this.panel1.TabIndex = 22;
            this.panel1.Visible = false;
            // 
            // ColorLbl
            // 
            this.ColorLbl.AutoSize = true;
            this.ColorLbl.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColorLbl.Location = new System.Drawing.Point(84, 13);
            this.ColorLbl.Name = "ColorLbl";
            this.ColorLbl.Size = new System.Drawing.Size(107, 25);
            this.ColorLbl.TabIndex = 1;
            this.ColorLbl.Text = "amarillo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "COLOR:";
            // 
            // Mesa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(172)))), ((int)(((byte)(132)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1195, 671);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.UnoBtn);
            this.Controls.Add(this.C3Lbl);
            this.Controls.Add(this.C3Pbx);
            this.Controls.Add(this.C2Lbl);
            this.Controls.Add(this.C2Pbx);
            this.Controls.Add(this.C1Lbl);
            this.Controls.Add(this.C1Pbx);
            this.Controls.Add(this.PasoBtn);
            this.Controls.Add(this.JI);
            this.Controls.Add(this.JB);
            this.Controls.Add(this.JD);
            this.Controls.Add(this.JA);
            this.Controls.Add(this.JugadorDerecha);
            this.Controls.Add(this.JugadorIzquierda);
            this.Controls.Add(this.JugadorAbajo);
            this.Controls.Add(this.JugadorArriba);
            this.Controls.Add(this.ChatGrp);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Mesa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mesa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Mesa_FormClosing);
            this.ChatGrp.ResumeLayout(false);
            this.ChatGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.C1Pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.C2Pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.C3Pbx)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MensajeTxt;
        private System.Windows.Forms.Button EnviarBtn;
        private System.Windows.Forms.ListBox ChatBox;
        private System.Windows.Forms.GroupBox ChatGrp;
        private System.Windows.Forms.Label JugadorArriba;
        private System.Windows.Forms.Label JugadorAbajo;
        private System.Windows.Forms.Label JugadorDerecha;
        private System.Windows.Forms.Label JugadorIzquierda;
        private System.Windows.Forms.Label JA;
        private System.Windows.Forms.Label JD;
        private System.Windows.Forms.Label JB;
        private System.Windows.Forms.Label JI;
        private System.Windows.Forms.Button PasoBtn;
        private System.Windows.Forms.PictureBox C1Pbx;
        private System.Windows.Forms.Label C1Lbl;
        private System.Windows.Forms.Label C2Lbl;
        private System.Windows.Forms.PictureBox C2Pbx;
        private System.Windows.Forms.PictureBox C3Pbx;
        private System.Windows.Forms.Label C3Lbl;
        private System.Windows.Forms.Button UnoBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ColorLbl;
        private System.Windows.Forms.Label label1;

    }
}