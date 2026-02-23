namespace chilehero_c
{
    partial class Form4
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
            this.cb_cancion = new System.Windows.Forms.ComboBox();
            this.lb_cancion = new System.Windows.Forms.Label();
            this.lb_instrumento = new System.Windows.Forms.Label();
            this.cb_instrumento = new System.Windows.Forms.ComboBox();
            this.lb_puntaje = new System.Windows.Forms.Label();
            this.txt_puntaje = new System.Windows.Forms.TextBox();
            this.txt_porcentaje = new System.Windows.Forms.TextBox();
            this.lb_porcentaje = new System.Windows.Forms.Label();
            this.txt_youtube = new System.Windows.Forms.TextBox();
            this.lb_youtube = new System.Windows.Forms.Label();
            this.lb_captura = new System.Windows.Forms.Label();
            this.pb_captura = new System.Windows.Forms.PictureBox();
            this.btn_subircaptura = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_enviarpuntaje = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_captura)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_cancion
            // 
            this.cb_cancion.FormattingEnabled = true;
            this.cb_cancion.Location = new System.Drawing.Point(73, 80);
            this.cb_cancion.Name = "cb_cancion";
            this.cb_cancion.Size = new System.Drawing.Size(346, 21);
            this.cb_cancion.TabIndex = 0;
            // 
            // lb_cancion
            // 
            this.lb_cancion.AutoSize = true;
            this.lb_cancion.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_cancion.Location = new System.Drawing.Point(160, 53);
            this.lb_cancion.Name = "lb_cancion";
            this.lb_cancion.Size = new System.Drawing.Size(189, 24);
            this.lb_cancion.TabIndex = 1;
            this.lb_cancion.Text = "Seleccionar Canción";
            // 
            // lb_instrumento
            // 
            this.lb_instrumento.AutoSize = true;
            this.lb_instrumento.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_instrumento.Location = new System.Drawing.Point(191, 145);
            this.lb_instrumento.Name = "lb_instrumento";
            this.lb_instrumento.Size = new System.Drawing.Size(122, 24);
            this.lb_instrumento.TabIndex = 3;
            this.lb_instrumento.Text = "Instrumento";
            // 
            // cb_instrumento
            // 
            this.cb_instrumento.FormattingEnabled = true;
            this.cb_instrumento.Items.AddRange(new object[] {
            "Guitarra",
            "Bajo",
            "Bateria"});
            this.cb_instrumento.Location = new System.Drawing.Point(73, 172);
            this.cb_instrumento.Name = "cb_instrumento";
            this.cb_instrumento.Size = new System.Drawing.Size(346, 21);
            this.cb_instrumento.TabIndex = 1;
            // 
            // lb_puntaje
            // 
            this.lb_puntaje.AutoSize = true;
            this.lb_puntaje.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_puntaje.Location = new System.Drawing.Point(69, 221);
            this.lb_puntaje.Name = "lb_puntaje";
            this.lb_puntaje.Size = new System.Drawing.Size(78, 24);
            this.lb_puntaje.TabIndex = 4;
            this.lb_puntaje.Text = "Puntaje";
            // 
            // txt_puntaje
            // 
            this.txt_puntaje.Location = new System.Drawing.Point(73, 249);
            this.txt_puntaje.Name = "txt_puntaje";
            this.txt_puntaje.Size = new System.Drawing.Size(154, 20);
            this.txt_puntaje.TabIndex = 2;
            // 
            // txt_porcentaje
            // 
            this.txt_porcentaje.Location = new System.Drawing.Point(275, 249);
            this.txt_porcentaje.Name = "txt_porcentaje";
            this.txt_porcentaje.Size = new System.Drawing.Size(154, 20);
            this.txt_porcentaje.TabIndex = 3;
            // 
            // lb_porcentaje
            // 
            this.lb_porcentaje.AutoSize = true;
            this.lb_porcentaje.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_porcentaje.Location = new System.Drawing.Point(271, 221);
            this.lb_porcentaje.Name = "lb_porcentaje";
            this.lb_porcentaje.Size = new System.Drawing.Size(107, 24);
            this.lb_porcentaje.TabIndex = 6;
            this.lb_porcentaje.Text = "Porcentaje";
            // 
            // txt_youtube
            // 
            this.txt_youtube.Location = new System.Drawing.Point(73, 326);
            this.txt_youtube.Name = "txt_youtube";
            this.txt_youtube.Size = new System.Drawing.Size(356, 20);
            this.txt_youtube.TabIndex = 4;
            // 
            // lb_youtube
            // 
            this.lb_youtube.AutoSize = true;
            this.lb_youtube.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_youtube.Location = new System.Drawing.Point(131, 299);
            this.lb_youtube.Name = "lb_youtube";
            this.lb_youtube.Size = new System.Drawing.Size(264, 24);
            this.lb_youtube.TabIndex = 8;
            this.lb_youtube.Text = "Video de YouTube (Opcional)";
            // 
            // lb_captura
            // 
            this.lb_captura.AutoSize = true;
            this.lb_captura.Font = new System.Drawing.Font("Oxanium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_captura.Location = new System.Drawing.Point(160, 371);
            this.lb_captura.Name = "lb_captura";
            this.lb_captura.Size = new System.Drawing.Size(185, 24);
            this.lb_captura.TabIndex = 10;
            this.lb_captura.Text = "Captura de Pantalla";
            // 
            // pb_captura
            // 
            this.pb_captura.BackColor = System.Drawing.Color.Transparent;
            this.pb_captura.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_captura.Location = new System.Drawing.Point(248, 398);
            this.pb_captura.Name = "pb_captura";
            this.pb_captura.Size = new System.Drawing.Size(181, 23);
            this.pb_captura.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_captura.TabIndex = 11;
            this.pb_captura.TabStop = false;
            // 
            // btn_subircaptura
            // 
            this.btn_subircaptura.Font = new System.Drawing.Font("Oxanium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_subircaptura.Location = new System.Drawing.Point(73, 398);
            this.btn_subircaptura.Name = "btn_subircaptura";
            this.btn_subircaptura.Size = new System.Drawing.Size(154, 23);
            this.btn_subircaptura.TabIndex = 12;
            this.btn_subircaptura.Text = "Seleccionar Captura";
            this.btn_subircaptura.UseVisualStyleBackColor = true;
            this.btn_subircaptura.Click += new System.EventHandler(this.btn_subircaptura_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Imágenes (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|PNG (*.png)|*.png|JP" +
    "G (*.jpg;*.jpeg)|*.jpg;*.jpeg|Todos los archivos (*.*)|*.*";
            this.openFileDialog1.Title = "Selecciona tu captura";
            // 
            // btn_enviarpuntaje
            // 
            this.btn_enviarpuntaje.Font = new System.Drawing.Font("Oxanium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_enviarpuntaje.Location = new System.Drawing.Point(174, 469);
            this.btn_enviarpuntaje.Name = "btn_enviarpuntaje";
            this.btn_enviarpuntaje.Size = new System.Drawing.Size(150, 53);
            this.btn_enviarpuntaje.TabIndex = 13;
            this.btn_enviarpuntaje.Text = "Enviar Puntaje";
            this.btn_enviarpuntaje.UseVisualStyleBackColor = true;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 575);
            this.Controls.Add(this.btn_enviarpuntaje);
            this.Controls.Add(this.btn_subircaptura);
            this.Controls.Add(this.pb_captura);
            this.Controls.Add(this.lb_captura);
            this.Controls.Add(this.txt_youtube);
            this.Controls.Add(this.lb_youtube);
            this.Controls.Add(this.txt_porcentaje);
            this.Controls.Add(this.lb_porcentaje);
            this.Controls.Add(this.txt_puntaje);
            this.Controls.Add(this.lb_puntaje);
            this.Controls.Add(this.lb_instrumento);
            this.Controls.Add(this.cb_instrumento);
            this.Controls.Add(this.lb_cancion);
            this.Controls.Add(this.cb_cancion);
            this.Name = "Form4";
            this.Text = "Sube tu puntaje | Chile Hero";
            ((System.ComponentModel.ISupportInitialize)(this.pb_captura)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_cancion;
        private System.Windows.Forms.Label lb_cancion;
        private System.Windows.Forms.Label lb_instrumento;
        private System.Windows.Forms.ComboBox cb_instrumento;
        private System.Windows.Forms.Label lb_puntaje;
        private System.Windows.Forms.TextBox txt_puntaje;
        private System.Windows.Forms.TextBox txt_porcentaje;
        private System.Windows.Forms.Label lb_porcentaje;
        private System.Windows.Forms.TextBox txt_youtube;
        private System.Windows.Forms.Label lb_youtube;
        private System.Windows.Forms.Label lb_captura;
        private System.Windows.Forms.PictureBox pb_captura;
        private System.Windows.Forms.Button btn_subircaptura;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_enviarpuntaje;
    }
}