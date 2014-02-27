namespace Provider.Database
{
    /// <summary>
    /// ������ �� ������� ������
    /// </summary>
    public class SelectQuery : Query
    {
        /// <summary>
        /// �������������� ����� ������ �� �������
        /// </summary>
        /// <param name="sql">������� SQL</param>
        /// <param name="storedProcedure">�������� �� �������� ����������</param>
        /// <param name="parameters">������ ���������� �� ����������</param>
        public SelectQuery(string sql, bool storedProcedure, params DbParamValue[] parameters)
            : base(QueryType.SelectQuery, sql, storedProcedure, parameters) { }

        public new DbParamValue[] Parameters { get { return (DbParamValue[])base.Parameters; } }
    }
}