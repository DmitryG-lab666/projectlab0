using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LinqSetOperations
{
    public partial class FormMain : Form
    {
        private readonly CatalogService _service = new CatalogService();
        private List<Product> _current = new List<Product>();

        public FormMain()
        {
            InitializeComponent();
            SetupGrid();

            btnExcept.Click += (s, e) => Show(_service.GetExceptA(), "Except: только у Поставщика А");
            btnIntersect.Click += (s, e) => Show(_service.GetIntersect(), "Intersect: общие товары");
            btnUnionSorted.Click += (s, e) => Show(_service.GetUnionSorted(), "Union + OrderBy + ThenBy");
            btnConcatDistinct.Click += (s, e) => Show(_service.GetConcatDistinct(), "Concat + Distinct");
            btnApplySort.Click += (s, e) => Show(_service.Sort(_current, rdbAsc.Checked), "Пересортировка");

            Show(_service.GetUnionSorted(), "Union + OrderBy + ThenBy");
        }

        private void SetupGrid()
        {
            dgvCatalog.AutoGenerateColumns = false;
            dgvCatalog.Columns.Clear();
            dgvCatalog.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Название", DataPropertyName = "Name", Width = 220 });
            dgvCatalog.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Категория", DataPropertyName = "Category", Width = 150 });
            dgvCatalog.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Цена",
                DataPropertyName = "Price",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvCatalog.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Поставщик", DataPropertyName = "Supplier", Width = 130 });
        }

        private void Show(IEnumerable<Product> items, string mode)
        {
            _current = items.ToList();
            dgvCatalog.DataSource = null;
            dgvCatalog.DataSource = _current;

            int count = _current.Count();
            // Average() на пустой последовательности бросает исключение — проверяем
            decimal avg = count > 0 ? _current.Average(p => p.Price) : 0m;

            lblMode.Text = "Режим: " + mode;
            lblCount.Text = "Записей: " + count;
            lblAvg.Text = "Средняя цена: " + avg.ToString("N2");
        }
    }
}
