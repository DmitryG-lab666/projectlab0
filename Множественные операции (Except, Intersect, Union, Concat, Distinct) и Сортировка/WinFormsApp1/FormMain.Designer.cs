namespace LinqSetOperations
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvCatalog = new System.Windows.Forms.DataGridView();
            this.pnlOps = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExcept = new System.Windows.Forms.Button();
            this.btnIntersect = new System.Windows.Forms.Button();
            this.btnUnionSorted = new System.Windows.Forms.Button();
            this.btnConcatDistinct = new System.Windows.Forms.Button();
            this.grpSort = new System.Windows.Forms.GroupBox();
            this.rdbAsc = new System.Windows.Forms.RadioButton();
            this.rdbDesc = new System.Windows.Forms.RadioButton();
            this.btnApplySort = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAvg = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalog)).BeginInit();
            this.pnlOps.SuspendLayout();
            this.grpSort.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            // pnlOps
            this.pnlOps.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOps.Height = 48;
            this.pnlOps.Padding = new System.Windows.Forms.Padding(6);
            this.pnlOps.Controls.Add(this.btnExcept);
            this.pnlOps.Controls.Add(this.btnIntersect);
            this.pnlOps.Controls.Add(this.btnUnionSorted);
            this.pnlOps.Controls.Add(this.btnConcatDistinct);

            // buttons
            this.btnExcept.Text = "Except (только А)";
            this.btnExcept.AutoSize = true;
            this.btnIntersect.Text = "Intersect (общие)";
            this.btnIntersect.AutoSize = true;
            this.btnUnionSorted.Text = "Union + сортировка";
            this.btnUnionSorted.AutoSize = true;
            this.btnConcatDistinct.Text = "Concat + Distinct";
            this.btnConcatDistinct.AutoSize = true;

            // grpSort
            this.grpSort.Text = "Сортировка (Категория, Цена)";
            this.grpSort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpSort.Height = 56;
            this.grpSort.Controls.Add(this.rdbAsc);
            this.grpSort.Controls.Add(this.rdbDesc);
            this.grpSort.Controls.Add(this.btnApplySort);

            this.rdbAsc.Text = "По возрастанию";
            this.rdbAsc.Checked = true;
            this.rdbAsc.AutoSize = true;
            this.rdbAsc.Location = new System.Drawing.Point(12, 22);
            this.rdbDesc.Text = "По убыванию";
            this.rdbDesc.AutoSize = true;
            this.rdbDesc.Location = new System.Drawing.Point(160, 22);
            this.btnApplySort.Text = "Применить";
            this.btnApplySort.Location = new System.Drawing.Point(290, 17);
            this.btnApplySort.Size = new System.Drawing.Size(110, 28);

            // dgvCatalog
            this.dgvCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCatalog.ReadOnly = true;
            this.dgvCatalog.AllowUserToAddRows = false;
            this.dgvCatalog.AllowUserToDeleteRows = false;
            this.dgvCatalog.RowHeadersVisible = false;
            this.dgvCatalog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // statusStrip1
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblMode, this.lblCount, this.lblAvg });
            this.lblMode.Spring = true;
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // FormMain
            this.ClientSize = new System.Drawing.Size(760, 460);
            this.Text = "Каталог поставщиков — LINQ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.Add(this.dgvCatalog);
            this.Controls.Add(this.grpSort);
            this.Controls.Add(this.pnlOps);
            this.Controls.Add(this.statusStrip1);

            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalog)).EndInit();
            this.pnlOps.ResumeLayout(false);
            this.grpSort.ResumeLayout(false);
            this.grpSort.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dgvCatalog;
        private System.Windows.Forms.FlowLayoutPanel pnlOps;
        private System.Windows.Forms.Button btnExcept;
        private System.Windows.Forms.Button btnIntersect;
        private System.Windows.Forms.Button btnUnionSorted;
        private System.Windows.Forms.Button btnConcatDistinct;
        private System.Windows.Forms.GroupBox grpSort;
        private System.Windows.Forms.RadioButton rdbAsc;
        private System.Windows.Forms.RadioButton rdbDesc;
        private System.Windows.Forms.Button btnApplySort;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblMode;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.ToolStripStatusLabel lblAvg;
    }
}
