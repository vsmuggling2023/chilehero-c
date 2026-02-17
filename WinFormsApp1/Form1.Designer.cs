namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            btn_iniciasesion = new Button();
            txt_usuario = new TextBox();
            txt_pass = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 56);
            label1.Name = "label1";
            label1.Size = new Size(47, 15);
            label1.TabIndex = 0;
            label1.Text = "Usuario";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(48, 108);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 1;
            label2.Text = "Contraseña";
            // 
            // btn_iniciasesion
            // 
            btn_iniciasesion.Location = new Point(94, 156);
            btn_iniciasesion.Name = "btn_iniciasesion";
            btn_iniciasesion.Size = new Size(139, 23);
            btn_iniciasesion.TabIndex = 2;
            btn_iniciasesion.Text = "Iniciar Sesión";
            btn_iniciasesion.UseVisualStyleBackColor = true;
            btn_iniciasesion.Click += btn_iniciasesion_Click;
            // 
            // txt_usuario
            // 
            txt_usuario.Location = new Point(121, 48);
            txt_usuario.Name = "txt_usuario";
            txt_usuario.Size = new Size(100, 23);
            txt_usuario.TabIndex = 3;
            // 
            // txt_pass
            // 
            txt_pass.Location = new Point(121, 108);
            txt_pass.Name = "txt_pass";
            txt_pass.Size = new Size(100, 23);
            txt_pass.TabIndex = 4;
            txt_pass.UseSystemPasswordChar = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(309, 216);
            Controls.Add(txt_pass);
            Controls.Add(txt_usuario);
            Controls.Add(btn_iniciasesion);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Inicio de Sesión | Chile Hero";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button btn_iniciasesion;
        private TextBox txt_usuario;
        private TextBox txt_pass;
    }
}
