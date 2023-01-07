using System.Collections.Generic;
using System.Data;

namespace DataTableSerializer
{
    public static class DataTableExtensions
    {
        public static void Fill<T>(this DataTable dataTable, IEnumerable<T> elements)
        {
            DataTableHelper.GenerateDataTableColumnsFromObject<T>(dataTable);
            DataTableHelper.FillTable(dataTable, elements);
        }
    }
}
