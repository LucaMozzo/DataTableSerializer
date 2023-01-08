using System.Collections.Generic;
using System.Data;

namespace DataTableSerializer
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Automatically create columns and fill the table with data
        /// </summary>
        /// <typeparam name="T">The model class type</typeparam>
        /// <param name="dataTable"></param>
        /// <param name="elements">The instances of the model to add to the table</param>
        public static void Fill<T>(this DataTable dataTable, IEnumerable<T> elements)
        {
            Fill(dataTable, elements, null);
        }

        /// <summary>
        /// Automatically create columns and fill the table with data
        /// </summary>
        /// <typeparam name="T">The model class type</typeparam>
        /// <param name="dataTable"></param>
        /// <param name="elements">The instances of the model to add to the table</param>
        /// <param name="propertyTransformer">The transformer class to apply transformations to properties</param>
        public static void Fill<T>(this DataTable dataTable, IEnumerable<T> elements, PropertyTransformer propertyTransformer)
        {
            DataTableHelper.GenerateDataTableColumnsFromObject<T>(dataTable, propertyTransformer);
            DataTableHelper.FillTable(dataTable, elements, propertyTransformer);
        }
    }
}
