using DataTableSerializer.Exceptions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;

namespace DataTableSerializer
{
    internal static class DataTableHelper
    {
        internal static void GenerateDataTableColumnsFromObject<T>(DataTable dataTable)
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();
            bool namesClash = IsDataTableColumnNamesClash(typeProperties);
            if (namesClash)
            {
                throw new DataTableColumnNameClashException("Resolved column names from properties and attributes have a clash");
            }

            dataTable.Columns.Clear();

            foreach (var property in typeProperties)
            {
                dataTable.Columns.Add(GetDataTableTargetColumnName(property), property.PropertyType);
            }
        }

        internal static void FillTable<T>(DataTable dataTable, IEnumerable<T> values)
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();

            foreach (var value in values)
            {
                object[] itemsArray = new object[typeProperties.Length];
                for (int i = 0; i < typeProperties.Length; ++i) 
                {
                    var propertyValue = typeProperties[i].GetValue(value);
                    itemsArray[i] = propertyValue;
                }
                dataTable.Rows.Add(itemsArray);
            }
        }

        private static string GetDataTableTargetColumnName(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes(true);
            var customName = attributes
                .Select(attr => attr as DataTableColumnNameAttribute)
                .Where(attr => attr != null)
                .Select(attr => attr.Name)
                .FirstOrDefault();

            return customName ?? property.Name;
        }

        private static bool IsDataTableColumnNamesClash(PropertyInfo[] properties)
        {
            HashSet<string> names = new HashSet<string>();
            foreach (PropertyInfo property in properties)
            {
                var resolvedName = GetDataTableTargetColumnName(property);
                if (!names.Contains(resolvedName))
                {
                    names.Add(resolvedName);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
