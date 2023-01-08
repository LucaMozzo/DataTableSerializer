using DataTableSerializer.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DataTableSerializer
{
    internal static class DataTableHelper
    {
        internal static void GenerateDataTableColumnsFromObject<T>(DataTable dataTable, PropertyTransformer propertyConverter)
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
                string resolvedName = GetDataTableTargetColumnName(property);
                Type resolvedType = GetDataTableTargetColumnType(property, propertyConverter);
                dataTable.Columns.Add(resolvedName, resolvedType);
            }
        }

        internal static void FillTable<T>(DataTable dataTable, IEnumerable<T> values, PropertyTransformer propertyConverter)
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();

            foreach (var value in values)
            {
                object[] itemsArray = new object[typeProperties.Length];
                for (int i = 0; i < typeProperties.Length; ++i) 
                {
                    var propertyValue = typeProperties[i].GetValue(value);
                    if (propertyConverter != null && propertyConverter[typeProperties[i]] != null)
                    {
                        propertyValue = propertyConverter[typeProperties[i]].DynamicInvoke(propertyValue);
                    }
                    itemsArray[i] = propertyValue;
                }
                dataTable.Rows.Add(itemsArray);
            }
        }

        private static Type GetDataTableTargetColumnType(PropertyInfo property, PropertyTransformer propertyConverter
            )
        {
            var resolvedType = propertyConverter?.GetOutputType(property);
            return resolvedType ?? property.PropertyType;
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

        private static string GetDataTableTransformedValue(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes(true);
            var customName = attributes
                .Select(attr => attr as DataTableColumnNameAttribute)
                .Where(attr => attr != null)
                .Select(attr => attr.Name)
                .FirstOrDefault();

            return customName ?? property.Name;
        }
    }
}
