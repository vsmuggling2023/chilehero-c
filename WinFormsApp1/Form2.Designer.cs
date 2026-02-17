namespace WinFormsApp1
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
            dataGridView1 = new DataGridView();
            menuStrip1 = new MenuStrip();
            archivoToolStripMenuItem = new ToolStripMenuItem();
            acercaDeToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesiónToolStripMenuItem = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            progressBar1 = new ProgressBar();
            lb_progress = new Label();
            lb_busqueda = new Label();
            txt_buscar = new TextBox();
            btn_buscar = new Button();
            lb_resultado = new Label();
            gestiónToolStripMenuItem = new ToolStripMenuItem();
            usuariosToolStripMenuItem = new ToolStripMenuItem();
            baneadosToolStripMenuItem = new ToolStripMenuItem();
            cancionesToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(29, 74);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(740, 270);
            dataGridView1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { archivoToolStripMenuItem, gestiónToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { acercaDeToolStripMenuItem, cerrarSesiónToolStripMenuItem, salirToolStripMenuItem });
            archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            archivoToolStripMenuItem.Size = new Size(60, 20);
            archivoToolStripMenuItem.Text = "Archivo";
            // 
            // acercaDeToolStripMenuItem
            // 
            acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            acercaDeToolStripMenuItem.Size = new Size(180, 22);
            acercaDeToolStripMenuItem.Text = "Acerca de";
            // 
            // cerrarSesiónToolStripMenuItem
            // 
            cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            cerrarSesiónToolStripMenuItem.Size = new Size(180, 22);
            cerrarSesiónToolStripMenuItem.Text = "Cerrar Sesión";
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(180, 22);
            salirToolStripMenuItem.Text = "Salir";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(29, 383);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(740, 32);
            progressBar1.TabIndex = 2;
            // 
            // lb_progress
            // 
            lb_progress.AutoSize = true;
            lb_progress.Location = new Point(28, 364);
            lb_progress.Name = "lb_progress";
            lb_progress.Size = new Size(38, 15);
            lb_progress.TabIndex = 3;
            lb_progress.Text = "label1";
            lb_progress.Visible = false;
            // 
            // lb_busqueda
            // 
            lb_busqueda.AutoSize = true;
            lb_busqueda.Location = new Point(441, 31);
            lb_busqueda.Name = "lb_busqueda";
            lb_busqueda.Size = new Size(59, 15);
            lb_busqueda.TabIndex = 4;
            lb_busqueda.Text = "Busqueda";
            // 
            // txt_buscar
            // 
            txt_buscar.Location = new Point(514, 27);
            txt_buscar.Name = "txt_buscar";
            txt_buscar.Size = new Size(174, 23);
            txt_buscar.TabIndex = 5;
            // 
            // btn_buscar
            // 
            btn_buscar.Location = new Point(694, 27);
            btn_buscar.Name = "btn_buscar";
            btn_buscar.Size = new Size(75, 23);
            btn_buscar.TabIndex = 6;
            btn_buscar.Text = "Buscar";
            btn_buscar.UseVisualStyleBackColor = true;
            // 
            // lb_resultado
            // 
            lb_resultado.AutoSize = true;
            lb_resultado.Location = new Point(31, 55);
            lb_resultado.Name = "lb_resultado";
            lb_resultado.Size = new Size(38, 15);
            lb_resultado.TabIndex = 7;
            lb_resultado.Text = "label1";
            // 
            // gestiónToolStripMenuItem
            // 
            gestiónToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { usuariosToolStripMenuItem, baneadosToolStripMenuItem, cancionesToolStripMenuItem });
            gestiónToolStripMenuItem.Name = "gestiónToolStripMenuItem";
            gestiónToolStripMenuItem.Size = new Size(59, 20);
            gestiónToolStripMenuItem.Text = "Gestión";
            // 
            // usuariosToolStripMenuItem
            // 
            usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            usuariosToolStripMenuItem.Size = new Size(180, 22);
            usuariosToolStripMenuItem.Text = "Usuarios";
            // 
            // baneadosToolStripMenuItem
            // 
            baneadosToolStripMenuItem.Name = "baneadosToolStripMenuItem";
            baneadosToolStripMenuItem.Size = new Size(180, 22);
            baneadosToolStripMenuItem.Text = "Baneados";
            // 
            // cancionesToolStripMenuItem
            // 
            cancionesToolStripMenuItem.Name = "cancionesToolStripMenuItem";
            cancionesToolStripMenuItem.Size = new Size(180, 22);
            cancionesToolStripMenuItem.Text = "Canciones";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lb_resultado);
            Controls.Add(btn_buscar);
            Controls.Add(txt_buscar);
            Controls.Add(lb_busqueda);
            Controls.Add(lb_progress);
            Controls.Add(progressBar1);
            Controls.Add(dataGridView1);
            Controls.Add(menuStrip1);
            Name = "Form2";
            Text = "Lista de Canciones | Chile Hero";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem acercaDeToolStripMenuItem;
        private ToolStripMenuItem cerrarSesiónToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private ProgressBar progressBar1;
        private Label lb_progress;
        private Label lb_busqueda;
        private TextBox txt_buscar;
        private Button btn_buscar;
        private Label lb_resultado;
        private ToolStripMenuItem gestiónToolStripMenuItem;
        private ToolStripMenuItem usuariosToolStripMenuItem;
        private ToolStripMenuItem baneadosToolStripMenuItem;
        private ToolStripMenuItem cancionesToolStripMenuItem;
    }
}