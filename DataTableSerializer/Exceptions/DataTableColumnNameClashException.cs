using System;

namespace DataTableSerializer.Exceptions
{
    public class DataTableColumnNameClashException : Exception
    {
        public DataTableColumnNameClashException(string message) : base(message) { }
    }
}
