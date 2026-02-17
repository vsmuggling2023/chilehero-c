using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btn_iniciasesion_Click(object sender, EventArgs e)
        {
            var usuario = txt_usuario.Text.Trim();
            var pass = txt_pass.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var connectionString = "Server=INSERTARHOST;Database=INSERTARBASEDATOS;Uid=INSERTARUSUARIO;Pwd=INSERTARPASS;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    const string sql = @"
                        SELECT 
                            u.contrasena, 
                            u.rol, 
                            b.razon, 
                            b.baneado_por
                        FROM usuarios u
                        LEFT JOIN Baneados b ON b.correo = u.correo
                        WHERE u.correo = @user
                        LIMIT 1;
                    ";

                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@user", usuario);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            var dbHash = reader["contrasena"]?.ToString();
                            var rol = reader["rol"] == DBNull.Value ? "Usuario" : reader["rol"]?.ToString();

                            var razon = reader["razon"] == DBNull.Value ? null : reader["razon"]?.ToString();
                            var baneadoPor = reader["baneado_por"] == DBNull.Value ? null : reader["baneado_por"]?.ToString();

                            string ComputeSha256Hash(string raw)
                            {
                                using (var sha = SHA256.Create())
                                {
                                    var bytes = Encoding.UTF8.GetBytes(raw);
                                    var hash = sha.ComputeHash(bytes);
                                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                                }
                            }

                            var enteredHash = ComputeSha256Hash(pass);

                            if (!string.Equals(dbHash, enteredHash, StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            bool estaBaneado = string.Equals(rol, "Baneado", StringComparison.OrdinalIgnoreCase)
                                               || !string.IsNullOrWhiteSpace(razon)
                                               || !string.IsNullOrWhiteSpace(baneadoPor);

                            if (estaBaneado)
                            {
                                string razonMsg = string.IsNullOrWhiteSpace(razon) ? "No especificada" : razon;
                                string baneadoPorMsg = string.IsNullOrWhiteSpace(baneadoPor) ? "No especificado" : baneadoPor;

                                MessageBox.Show(
                                    $"Tu cuenta está BANEADA.\n\nRazón: {razonMsg}\nBaneado por: {baneadoPorMsg}\nContacta al administrador, para revisar tu cuenta\nchilehero2023@gmail.com",
                                    "Acceso denegado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );

                                txt_pass.Clear();
                                txt_pass.Focus();
                                return;
                            }

                            MessageBox.Show($"Inicio de sesión correcto. Rol: {rol}", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var f2 = new Form2(connectionString, rol);
                            f2.Show();
                            this.Hide();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar o ejecutar consulta: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}