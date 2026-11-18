using System.Collections.Generic;
using System.Linq;

namespace LinqSetOperations
{
    // Сервис с исходными коллекциями и LINQ-запросами
    public class CatalogService
    {
        public List<Product> SupplierA { get; } = new List<Product>
        {
            new Product(1,  "Ноутбук Lenovo",   "Компьютеры",  54990m, "Поставщик А"),
            new Product(2,  "Мышь Logitech",    "Периферия",    1290m, "Поставщик А"),
            new Product(3,  "Клавиатура Razer", "Периферия",    6490m, "Поставщик А"),
            new Product(4,  "Монитор Dell 27",  "Мониторы",    21990m, "Поставщик А"),
            new Product(5,  "Принтер HP",       "Оргтехника",  12500m, "Поставщик А"),
            new Product(6,  "Веб-камера",       "Периферия",    3200m, "Поставщик А"),
        };

        public List<Product> SupplierB { get; } = new List<Product>
        {
            // Регистр отличается намеренно: Equals его игнорирует
            new Product(101, "ноутбук lenovo",   "компьютеры",  52900m, "Поставщик Б"),
            new Product(102, "МЫШЬ LOGITECH",    "Периферия",    1350m, "Поставщик Б"),
            new Product(103, "Монитор Samsung",  "Мониторы",    18990m, "Поставщик Б"),
            new Product(104, "Сканер Canon",     "Оргтехника",   8700m, "Поставщик Б"),
            new Product(105, "Веб-камера",       "Периферия",    2990m, "Поставщик Б"),
            new Product(106, "Принтер Epson",    "Оргтехника",  11800m, "Поставщик Б"),
        };

        // 1. Есть у А, нет у Б
        public IEnumerable<Product> GetExceptA() => SupplierA.Except(SupplierB);

        // 2. Позиции, которые поставляют оба
        public IEnumerable<Product> GetIntersect() => SupplierA.Intersect(SupplierB);

        // 3. Union без дубликатов: Категория ↑, внутри категории Цена ↓
        public IEnumerable<Product> GetUnionSorted() =>
            SupplierA.Union(SupplierB)
                     .OrderBy(p => p.Category)
                     .ThenByDescending(p => p.Price);

        // 4. Concat + Distinct
        public IEnumerable<Product> GetConcatDistinct() =>
            SupplierA.Concat(SupplierB).Distinct();

        // Пересортировка текущей выборки: Категория, затем Цена, в выбранном направлении
        public IEnumerable<Product> Sort(IEnumerable<Product> source, bool ascending) =>
            ascending
                ? source.OrderBy(p => p.Category).ThenBy(p => p.Price)
                : source.OrderByDescending(p => p.Category).ThenByDescending(p => p.Price);
    }
}
