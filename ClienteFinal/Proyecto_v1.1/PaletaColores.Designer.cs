namespace Proyecto_v1._1
{
    partial class PaletaColores
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
            this.RojoBtn = new System.Windows.Forms.Button();
            this.AzulBtn = new System.Windows.Forms.Button();
            this.AmarilloBtn = new System.Windows.Forms.Button();
            this.VerdeBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RojoBtn
            // 
            this.RojoBtn.BackColor = System.Drawing.Color.Red;
            this.RojoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RojoBtn.ForeColor = System.Drawing.Color.Black;
            this.RojoBtn.Location = new System.Drawing.Point(12, 48);
            this.RojoBtn.Name = "RojoBtn";
            this.RojoBtn.Size = new System.Drawing.Size(100, 100);
            this.RojoBtn.TabIndex = 0;
            this.RojoBtn.UseVisualStyleBackColor = false;
            this.RojoBtn.Click += new System.EventHandler(this.RojoBtn_Click);
            // 
            // AzulBtn
            // 
            this.AzulBtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.AzulBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AzulBtn.ForeColor = System.Drawing.Color.Black;
            this.AzulBtn.Location = new System.Drawing.Point(118, 48);
            this.AzulBtn.Name = "AzulBtn";
            this.AzulBtn.Size = new System.Drawing.Size(100, 100);
            this.AzulBtn.TabIndex = 1;
            this.AzulBtn.UseVisualStyleBackColor = false;
            this.AzulBtn.Click += new System.EventHandler(this.AzulBtn_Click);
            // 
            // AmarilloBtn
            // 
            this.AmarilloBtn.BackColor = System.Drawing.Color.Yellow;
            this.AmarilloBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AmarilloBtn.ForeColor = System.Drawing.Color.Black;
            this.AmarilloBtn.Location = new System.Drawing.Point(12, 154);
            this.AmarilloBtn.Name = "AmarilloBtn";
            this.AmarilloBtn.Size = new System.Drawing.Size(100, 100);
            this.AmarilloBtn.TabIndex = 2;
            this.AmarilloBtn.UseVisualStyleBackColor = false;
            this.AmarilloBtn.Click += new System.EventHandler(this.AmarilloBtn_Click);
            // 
            // VerdeBtn
            // 
            this.VerdeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.VerdeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VerdeBtn.ForeColor = System.Drawing.Color.Black;
            this.VerdeBtn.Location = new System.Drawing.Point(118, 154);
            this.VerdeBtn.Name = "VerdeBtn";
            this.VerdeBtn.Size = new System.Drawing.Size(100, 100);
            this.VerdeBtn.TabIndex = 3;
            this.VerdeBtn.UseVisualStyleBackColor = false;
            this.VerdeBtn.Click += new System.EventHandler(this.VerdeBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(63, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Elige el color";
            // 
            // PaletaColores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(230, 266);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VerdeBtn);
            this.Controls.Add(this.AmarilloBtn);
            this.Controls.Add(this.AzulBtn);
            this.Controls.Add(this.RojoBtn);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PaletaColores";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaletaColores";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RojoBtn;
        private System.Windows.Forms.Button AzulBtn;
        private System.Windows.Forms.Button AmarilloBtn;
        private System.Windows.Forms.Button VerdeBtn;
        private System.Windows.Forms.Label label1;
    }
}