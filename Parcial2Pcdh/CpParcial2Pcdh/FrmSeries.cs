using CadParcial2Pcdh;
using ClnParcial2Pcdh;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Pcdh
{
    public partial class FrmSeries : Form
    {
        bool esNuevo = false;
        public FrmSeries()
        {
            InitializeComponent();
        }
        private void listar()
        {
            var series = SerieCln.listarPa(txtParametro.Text.Trim());
            dgvLista.DataSource = series;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["titulo"].HeaderText = "Titulo ";
            dgvLista.Columns["sinopsis"].HeaderText = "Sinopsis";
            dgvLista.Columns["director"].HeaderText = "Director";
            dgvLista.Columns["duracion"].HeaderText = "Duracion";
            dgvLista.Columns["fechaEstreno"].HeaderText = "Fecha de Estreno";
            dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario";
            btnEditar.Enabled = series.Count > 0;
            btnEliminar.Enabled = series.Count > 0;
            if (series.Count > 0) dgvLista.Rows[0].Cells["titulo"].Selected = true;
        }

        private void FrmSeries_Load(object sender, EventArgs e)
        {
            Size = new Size(852, 378);
            listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(852, 558);
            txtTitulo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(852, 558);

            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            var serie = SerieCln.get(id);
            txtTitulo.Text = serie.titulo;
            txtSinopsis.Text = serie.sinopsis;
            txtDirector.Text = serie.director;
            nudDuracion.Value = serie.duracion;
            dtpFechaEstreno.Value = serie.fechaEstreno;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(852, 558);
            limpiar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private bool validar()
        {
            bool esValido = true;
            erpTitulo.SetError(txtTitulo, "");
            erpSinopsis.SetError(txtSinopsis, "");
            erpDirector.SetError(txtDirector, "");
            erpDuracion.SetError(nudDuracion, "");
            erpFechaEstreno.SetError(dtpFechaEstreno, "");
            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                esValido = false;
                erpTitulo.SetError(txtTitulo, "El campo Titulo es obligatorio");
            }
            if (string.IsNullOrEmpty(txtSinopsis.Text))
            {
                esValido = false;
                erpSinopsis.SetError(txtSinopsis, "El campo Sinopsis es obligatorio");
            }
            if (string.IsNullOrEmpty(txtDirector.Text))
            {
                esValido = false;
                erpDirector.SetError(txtDirector, "El campo Director es obligatorio");
            }
            if (string.IsNullOrEmpty(nudDuracion.Text))
            {
                esValido = false;
                erpDuracion.SetError(nudDuracion, "El campo Duracion es obligatorio");
            }
            if (nudDuracion.Value < 0)
            {
                esValido = false;
                erpDuracion.SetError(nudDuracion, "El campo Duracion debe ser mayor o igual a 0");
            }
            if (string.IsNullOrEmpty(dtpFechaEstreno.Text))
            {
                esValido = false;
                erpFechaEstreno.SetError(dtpFechaEstreno, "El campo Fecha de Estreno es obligatorio");
            }
            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text;
                serie.duracion = (int)nudDuracion.Value;
                serie.fechaEstreno = dtpFechaEstreno.Value;
                serie.usuarioRegistro = "Parcial2";

                if (esNuevo)
                {
                    serie.fechaRegistro = DateTime.Now;
                    serie.estado = 1;
                    SerieCln.insertar(serie);
                }
                else
                {
                    int index = dgvLista.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    SerieCln.actualizar(serie);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("La serie ha sido guardada correctamente", "::: Serie - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        
        }
        private void limpiar()
        {
            txtTitulo.Text = string.Empty;
            txtSinopsis.Text = string.Empty;
            txtDirector.Text = string.Empty;
            nudDuracion.Value = 0;
            dtpFechaEstreno.MaxDate = DateTime.Now;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string titulo = dgvLista.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro que desea dar de baja la serie {titulo}?",
                "::: Serie - Mensaje :::", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialog == DialogResult.OK)
            {
                SerieCln.eliminar(id, "Parcial2");
                listar();
                MessageBox.Show("La serie ha sido dado de baja correctamente", "::: Serie - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}