namespace WinFormsApp1
{
    partial class Form3
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
            lb_usuarios = new Label();
            btn_buscar = new Button();
            txt_buscar = new TextBox();
            lb_busqueda = new Label();
            dataGridView1 = new DataGridView();
            lb_actualizacion = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // lb_usuarios
            // 
            lb_usuarios.AutoSize = true;
            lb_usuarios.Location = new Point(31, 60);
            lb_usuarios.Name = "lb_usuarios";
            lb_usuarios.Size = new Size(38, 15);
            lb_usuarios.TabIndex = 14;
            lb_usuarios.Text = "label1";
            // 
            // btn_buscar
            // 
            btn_buscar.Location = new Point(696, 31);
            btn_buscar.Name = "btn_buscar";
            btn_buscar.Size = new Size(75, 23);
            btn_buscar.TabIndex = 13;
            btn_buscar.Text = "Buscar";
            btn_buscar.UseVisualStyleBackColor = true;
            btn_buscar.Click += btn_buscar_Click;
            // 
            // txt_buscar
            // 
            txt_buscar.Location = new Point(516, 31);
            txt_buscar.Name = "txt_buscar";
            txt_buscar.Size = new Size(174, 23);
            txt_buscar.TabIndex = 12;
            // 
            // lb_busqueda
            // 
            lb_busqueda.AutoSize = true;
            lb_busqueda.Location = new Point(443, 35);
            lb_busqueda.Name = "lb_busqueda";
            lb_busqueda.Size = new Size(59, 15);
            lb_busqueda.TabIndex = 11;
            lb_busqueda.Text = "Busqueda";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(31, 78);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(740, 329);
            dataGridView1.TabIndex = 8;
            // 
            // lb_actualizacion
            // 
            lb_actualizacion.AutoSize = true;
            lb_actualizacion.Location = new Point(33, 414);
            lb_actualizacion.Name = "lb_actualizacion";
            lb_actualizacion.Size = new Size(38, 15);
            lb_actualizacion.TabIndex = 15;
            lb_actualizacion.Text = "label1";
            lb_actualizacion.Visible = false;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lb_actualizacion);
            Controls.Add(lb_usuarios);
            Controls.Add(btn_buscar);
            Controls.Add(txt_buscar);
            Controls.Add(lb_busqueda);
            Controls.Add(dataGridView1);
            Name = "Form3";
            Text = "Gestión de Usuarios | Chile Hero";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lb_usuarios;
        private Button btn_buscar;
        private TextBox txt_buscar;
        private Label lb_busqueda;
        private DataGridView dataGridView1;
        private Label lb_actualizacion;
    }
}