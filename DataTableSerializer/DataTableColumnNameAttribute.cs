using System;

namespace DataTableSerializer
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataTableColumnNameAttribute : Attribute
    {
        public string Name { get; private set; }

        /// <summary>
        /// Use a friendly name as column header
        /// </summary>
        /// <param name="name">The name of the column</param>
        public DataTableColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
