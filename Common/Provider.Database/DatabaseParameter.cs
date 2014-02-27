using System.Data;

namespace Provider.Database
{
    /// <summary>
    /// Тип параметра
    /// </summary>C:\DEV_TEMP\CommonLibs\ProviderCore\Data\DatabaseParameter.cs
    public enum DbParamType
    {
        Binary,
        Boolean,
        Byte,
        Char,
        Currency,
        DataTable,
        DateTime,
        Decimal,
        Guid,
        Integer,
        Int64,
        Object,
        String,
        Time,
        Timestamp,
        Xml
    }

    /// <summary>
    /// Параметр запроса
    /// </summary>
    public class DbParam
    {
        public DbParam(string name)
        {
            Name = name;
        }

        public DbParam(string name, DbParamType dbType)
        {
            Name = name;
            DbType = dbType;
        }

        /// <summary>
        /// Имя параметра
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Типа параметра
        /// </summary>
        public DbParamType DbType { get; set; }
    }

    /*public class DbParamValue
    {
        public DbParamValue(string name, DbType dbType, object value)
        {
            Parameter = new DbParam(name, dbType);
            Value = value;
        }

        public DbParamValue(string name, object value)
        {
            Parameter = new DbParam(name);
            Value = value;
        }

        public DbParamValue(DbParam parameter, object value)
        {
            Parameter = parameter;
            Value = value;
        }
        /// <summary>
        /// Описание параметра
        /// </summary>
        public DbParam Parameter { get; set; }
        /// <summary>
        /// Значение параметра
        /// </summary>
        public object Value { get; set; }
    }*/

    public class DbParamValue : DbParam
    {
        public DbParamValue(string name, object value, DbParamType dbType, ParameterDirection direction)
            : base(name, dbType)
        {
            Value = value;
            Direction = direction;
        }

        public DbParamValue(string name, object value, DbParamType dbType)
            : this(name, value, dbType, ParameterDirection.Input)
        {
        }

        public DbParamValue(string name, object value)
            : base(name)
        {
            Direction = ParameterDirection.Input;
            Value = value;
        }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Направление параметра
        /// </summary>
        public ParameterDirection Direction { get; set; }
    }
}