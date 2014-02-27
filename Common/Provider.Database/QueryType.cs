namespace Provider.Database
{
    /// <summary>
    /// ��� �������
    /// </summary>
    public enum QueryType
    {
        /// <summary>
        /// ������ �� ������� ������
        /// </summary>
        SelectQuery,
        /// <summary>
        /// ������ ������������ ���� ��������
        /// </summary>
        ScalarQuery,
        /// <summary>
        /// ������, �� ������������ ������
        /// </summary>
        NonSelectQuery,
        /// <summary>
        /// �������� ������� ������ � �������
        /// </summary>
        TableBulkInsert
    }
}