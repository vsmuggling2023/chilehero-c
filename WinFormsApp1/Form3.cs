using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace WinFormsApp1
{
    public partial class Form3 : Form
    {
        private readonly string _connectionString;
        private readonly string _rolActual;
        private readonly bool _soloBaneados;

        private DataTable _dtUsuarios;
        private readonly BindingSource _bsUsuarios = new BindingSource();

        private bool _cargando = false;

        private static readonly string[] RolesEnum =
        {
            "Baneado",
            "Usuario",
            "VIP Donador",
            "VIP Premium",
            "VIP Supremo",
            "VIP Legendario",
            "Staff",
            "Administrador",
            "Dueño"
        };

        public Form3(string connectionString, string rolActual, bool soloBaneados = false)
        {
            InitializeComponent();

            _connectionString = connectionString;
            _rolActual = rolActual;
            _soloBaneados = soloBaneados;

            StartPosition = FormStartPosition.CenterParent;

            Load += Form3_Load;

            btn_buscar.Click += btn_buscar_Click;
            txt_buscar.KeyDown += txt_buscar_KeyDown;

            // Eventos para edición de ComboBox + Update
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.DataError += dataGridView1_DataError; 
            ConfigurarLayoutResponsive_Form3();
        }

        private void ConfigurarLayoutResponsive_Form3()
        {
            this.AutoScaleMode = AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(900, 600);

            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            txt_buscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btn_buscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lb_busqueda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (!EsAdminODueno(_rolActual))
            {
                MessageBox.Show("No tienes permisos para acceder a Gestión.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }


            CargarUsuarios();
            lb_usuarios.Text = $"(Total: {_bsUsuarios.Count})";
        }

        private static bool EsAdminODueno(string rol)
        {
            return rol != null &&
                   (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                    rol.Equals("Dueño", StringComparison.OrdinalIgnoreCase));
        }

        private void CargarUsuarios()
        {
            _cargando = true;

            string sql = @"
                SELECT
                    id,
                    nombre,
                    correo,
                    genero,
                    NULLIF(fecha_nacimiento, '0000-00-00') AS fecha_nacimiento,
                    NULLIF(fecha_registro, '0000-00-00 00:00:00') AS fecha_registro,
                    rol,
                    IP
                FROM usuarios
            ";

            if (_soloBaneados)
                sql += " WHERE rol = 'Baneado' ";

            sql += " ORDER BY id DESC;";

            try
            {
                using var cn = new MySqlConnection(_connectionString);
                using var cmd = new MySqlCommand(sql, cn);
                using var da = new MySqlDataAdapter(cmd);

                var dt = new DataTable();
                cn.Open();
                da.Fill(dt);

                _dtUsuarios = dt;
                _bsUsuarios.DataSource = _dtUsuarios;

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = _bsUsuarios;

                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ConfigurarColumnaRolComoCombo();
                BloquearTodoMenosRol();

                lb_usuarios.Text = $"Búsqueda: (Total: {_bsUsuarios.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _cargando = false;
            }
        }

        private void ConfigurarColumnaRolComoCombo()
        {
            if (!dataGridView1.Columns.Contains("rol"))
                return;

            // Si ya es combo, no repetir
            if (dataGridView1.Columns["rol"] is DataGridViewComboBoxColumn)
                return;

            int idx = dataGridView1.Columns["rol"].Index;
            dataGridView1.Columns.Remove("rol");

            var combo = new DataGridViewComboBoxColumn
            {
                Name = "rol",
                HeaderText = "Rol",
                DataPropertyName = "rol",      
                DataSource = RolesEnum,        
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
                AutoComplete = true
            };

            dataGridView1.Columns.Insert(idx, combo);
        }

        private void BloquearTodoMenosRol()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.ReadOnly = true;

            if (dataGridView1.Columns.Contains("rol"))
                dataGridView1.Columns["rol"].ReadOnly = false;

            dataGridView1.ReadOnly = false;
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private async void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_cargando) return;
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "rol") return;

            try
            {
                
                var row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["id"].Value == null) return;

                int id = Convert.ToInt32(row.Cells["id"].Value);
                string nuevoRol = row.Cells["rol"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(nuevoRol))
                    return;

                bool rolValido = false;
                foreach (var r in RolesEnum)
                {
                    if (string.Equals(r, nuevoRol, StringComparison.Ordinal))
                    {
                        rolValido = true;
                        break;
                    }
                }

                if (!rolValido)
                {
                    MessageBox.Show("Rol inválido.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await ActualizarRolUsuarioAsync(id, nuevoRol);

                lb_actualizacion.Visible = true;
                lb_actualizacion.Text = $"Rol actualizado (ID {id} → {nuevoRol})";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar rol: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ActualizarRolUsuarioAsync(int id, string nuevoRol)
        {
            const string sql = "UPDATE usuarios SET rol = @rol WHERE id = @id;";

            using var cn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@rol", nuevoRol);
            cmd.Parameters.AddWithValue("@id", id);

            await cn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            if (rows <= 0)
                throw new Exception("No se actualizó ningún registro (ID no encontrado).");
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            AplicarFiltro(txt_buscar.Text);
        }

        private void txt_buscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                AplicarFiltro(txt_buscar.Text);
            }
        }

        private void AplicarFiltro(string texto)
        {
            if (_dtUsuarios == null) return;

            texto = (texto ?? "").Trim();

            if (texto.Length == 0)
            {
                _bsUsuarios.RemoveFilter();
                lb_usuarios.Text = $"Búsqueda: (Total: {_bsUsuarios.Count})";
                return;
            }

            string safe = EscapeRowFilterValue(texto);

            _bsUsuarios.Filter =
                $"[nombre] LIKE '%{safe}%'" +
                $" OR [correo] LIKE '%{safe}%'" +
                $" OR [rol] LIKE '%{safe}%'" +
                $" OR [IP] LIKE '%{safe}%'";

            lb_usuarios.Text = $"Búsqueda: {texto} ({_bsUsuarios.Count} resultados)";
        }

        private static string EscapeRowFilterValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("]", "[]]");
        }
    }
}
