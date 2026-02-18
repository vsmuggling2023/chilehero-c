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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::chilehero_c.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(69, 62);
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
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1699, 902);
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
    }
}