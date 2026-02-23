namespace chilehero_c
{
    partial class Form2
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lb_bienvenido = new System.Windows.Forms.Label();
            this.lb_rol = new System.Windows.Forms.Label();
            this.btn_canciones = new System.Windows.Forms.Button();
            this.btn_subirpuntajes = new System.Windows.Forms.Button();
            this.btn_packs = new System.Windows.Forms.Button();
            this.btn_accederadmin = new System.Windows.Forms.Button();
            this.btn_vertuspuntajes = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::chilehero_c.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(75, -3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(698, 828);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lb_bienvenido
            // 
            this.lb_bienvenido.AutoSize = true;
            this.lb_bienvenido.Font = new System.Drawing.Font("Oxanium", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_bienvenido.ForeColor = System.Drawing.Color.Yellow;
            this.lb_bienvenido.Location = new System.Drawing.Point(827, 39);
            this.lb_bienvenido.Name = "lb_bienvenido";
            this.lb_bienvenido.Size = new System.Drawing.Size(127, 46);
            this.lb_bienvenido.TabIndex = 1;
            this.lb_bienvenido.Text = "label1";
            // 
            // lb_rol
            // 
            this.lb_rol.AutoSize = true;
            this.lb_rol.Font = new System.Drawing.Font("Oxanium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_rol.ForeColor = System.Drawing.Color.Yellow;
            this.lb_rol.Location = new System.Drawing.Point(831, 85);
            this.lb_rol.Name = "lb_rol";
            this.lb_rol.Size = new System.Drawing.Size(55, 20);
            this.lb_rol.TabIndex = 2;
            this.lb_rol.Text = "label1";
            // 
            // btn_canciones
            // 
            this.btn_canciones.Font = new System.Drawing.Font("Oxanium", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_canciones.Location = new System.Drawing.Point(1066, 134);
            this.btn_canciones.Name = "btn_canciones";
            this.btn_canciones.Size = new System.Drawing.Size(329, 89);
            this.btn_canciones.TabIndex = 3;
            this.btn_canciones.Text = "Canciones";
            this.btn_canciones.UseVisualStyleBackColor = true;
            this.btn_canciones.Click += new System.EventHandler(this.btn_canciones_Click);
            // 
            // btn_subirpuntajes
            // 
            this.btn_subirpuntajes.Font = new System.Drawing.Font("Oxanium", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_subirpuntajes.Location = new System.Drawing.Point(1066, 383);
            this.btn_subirpuntajes.Name = "btn_subirpuntajes";
            this.btn_subirpuntajes.Size = new System.Drawing.Size(329, 89);
            this.btn_subirpuntajes.TabIndex = 5;
            this.btn_subirpuntajes.Text = "Sube tu Puntaje";
            this.btn_subirpuntajes.UseVisualStyleBackColor = true;
            // 
            // btn_packs
            // 
            this.btn_packs.Font = new System.Drawing.Font("Oxanium", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_packs.Location = new System.Drawing.Point(1066, 253);
            this.btn_packs.Name = "btn_packs";
            this.btn_packs.Size = new System.Drawing.Size(329, 89);
            this.btn_packs.TabIndex = 4;
            this.btn_packs.Text = "Packs Disponibles";
            this.btn_packs.UseVisualStyleBackColor = true;
            // 
            // btn_accederadmin
            // 
            this.btn_accederadmin.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_accederadmin.Location = new System.Drawing.Point(1496, 29);
            this.btn_accederadmin.Name = "btn_accederadmin";
            this.btn_accederadmin.Size = new System.Drawing.Size(172, 41);
            this.btn_accederadmin.TabIndex = 7;
            this.btn_accederadmin.Text = "Admin";
            this.btn_accederadmin.UseVisualStyleBackColor = true;
            this.btn_accederadmin.Click += new System.EventHandler(this.btn_accederadmin_Click);
            // 
            // btn_vertuspuntajes
            // 
            this.btn_vertuspuntajes.Font = new System.Drawing.Font("Oxanium", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_vertuspuntajes.Location = new System.Drawing.Point(1066, 513);
            this.btn_vertuspuntajes.Name = "btn_vertuspuntajes";
            this.btn_vertuspuntajes.Size = new System.Drawing.Size(329, 89);
            this.btn_vertuspuntajes.TabIndex = 6;
            this.btn_vertuspuntajes.Text = "Ver tus puntajes";
            this.btn_vertuspuntajes.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1699, 733);
            this.Controls.Add(this.btn_vertuspuntajes);
            this.Controls.Add(this.btn_accederadmin);
            this.Controls.Add(this.btn_packs);
            this.Controls.Add(this.btn_subirpuntajes);
            this.Controls.Add(this.btn_canciones);
            this.Controls.Add(this.lb_rol);
            this.Controls.Add(this.lb_bienvenido);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form2";
            this.Text = "Menú Principal | Chile Hero";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lb_bienvenido;
        private System.Windows.Forms.Label lb_rol;
        private System.Windows.Forms.Button btn_canciones;
        private System.Windows.Forms.Button btn_subirpuntajes;
        private System.Windows.Forms.Button btn_packs;
        private System.Windows.Forms.Button btn_accederadmin;
        private System.Windows.Forms.Button btn_vertuspuntajes;
    }
}