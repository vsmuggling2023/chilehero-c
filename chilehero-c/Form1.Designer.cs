namespace chilehero_c
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lb_titulo = new System.Windows.Forms.Label();
            this.lb_correo = new System.Windows.Forms.Label();
            this.lb_pass = new System.Windows.Forms.Label();
            this.txt_correo = new System.Windows.Forms.TextBox();
            this.txt_pass = new System.Windows.Forms.TextBox();
            this.btn_iniciasesion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(102, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(129, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lb_titulo
            // 
            this.lb_titulo.AutoSize = true;
            this.lb_titulo.BackColor = System.Drawing.Color.Transparent;
            this.lb_titulo.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_titulo.ForeColor = System.Drawing.Color.Yellow;
            this.lb_titulo.Location = new System.Drawing.Point(63, 162);
            this.lb_titulo.Name = "lb_titulo";
            this.lb_titulo.Size = new System.Drawing.Size(209, 24);
            this.lb_titulo.TabIndex = 1;
            this.lb_titulo.Text = "ACCEDE A TU CUENTA";
            // 
            // lb_correo
            // 
            this.lb_correo.AutoSize = true;
            this.lb_correo.Font = new System.Drawing.Font("Oxanium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_correo.ForeColor = System.Drawing.Color.Yellow;
            this.lb_correo.Location = new System.Drawing.Point(138, 200);
            this.lb_correo.Name = "lb_correo";
            this.lb_correo.Size = new System.Drawing.Size(62, 20);
            this.lb_correo.TabIndex = 2;
            this.lb_correo.Text = "Correo:";
            // 
            // lb_pass
            // 
            this.lb_pass.AutoSize = true;
            this.lb_pass.Font = new System.Drawing.Font("Oxanium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_pass.ForeColor = System.Drawing.Color.Yellow;
            this.lb_pass.Location = new System.Drawing.Point(118, 262);
            this.lb_pass.Name = "lb_pass";
            this.lb_pass.Size = new System.Drawing.Size(100, 20);
            this.lb_pass.TabIndex = 3;
            this.lb_pass.Text = "Contraseña:";
            // 
            // txt_correo
            // 
            this.txt_correo.Location = new System.Drawing.Point(67, 228);
            this.txt_correo.Name = "txt_correo";
            this.txt_correo.Size = new System.Drawing.Size(205, 20);
            this.txt_correo.TabIndex = 4;
            // 
            // txt_pass
            // 
            this.txt_pass.Location = new System.Drawing.Point(67, 297);
            this.txt_pass.Name = "txt_pass";
            this.txt_pass.Size = new System.Drawing.Size(205, 20);
            this.txt_pass.TabIndex = 5;
            // 
            // btn_iniciasesion
            // 
            this.btn_iniciasesion.Font = new System.Drawing.Font("Oxanium ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_iniciasesion.Location = new System.Drawing.Point(77, 355);
            this.btn_iniciasesion.Name = "btn_iniciasesion";
            this.btn_iniciasesion.Size = new System.Drawing.Size(185, 36);
            this.btn_iniciasesion.TabIndex = 6;
            this.btn_iniciasesion.Text = "Iniciar Sesión";
            this.btn_iniciasesion.UseVisualStyleBackColor = true;
            this.btn_iniciasesion.Click += new System.EventHandler(this.btn_iniciasesion_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 450);
            this.Controls.Add(this.btn_iniciasesion);
            this.Controls.Add(this.txt_pass);
            this.Controls.Add(this.txt_correo);
            this.Controls.Add(this.lb_pass);
            this.Controls.Add(this.lb_correo);
            this.Controls.Add(this.lb_titulo);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Name = "Form1";
            this.Text = "Iniciar Sesión | Chile Hero";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lb_titulo;
        private System.Windows.Forms.Label lb_correo;
        private System.Windows.Forms.Label lb_pass;
        private System.Windows.Forms.TextBox txt_correo;
        private System.Windows.Forms.TextBox txt_pass;
        private System.Windows.Forms.Button btn_iniciasesion;
    }
}

