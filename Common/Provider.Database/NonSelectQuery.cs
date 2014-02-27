using System;
using System.Collections.Generic;
using System.Linq;

namespace Provider.Database
{
    /// <summary>
    /// ������, �� ������������ ������
    /// </summary>
    [Obsolete("��������� ������")]
    public class NonSelectQuery : Query
    {
        /// <summary>
        /// �������������� ����� ������
        /// </summary>
        /// <param name="sql">������� SQL</param>
        /// <param name="storedProcedure">�������� �� �������� ����������</param>
        /// <param name="parameters">������ ���������� �� ����������</param>
        public NonSelectQuery(string sql, bool storedProcedure, params DbParam[] parameters)
            : base(QueryType.NonSelectQuery, sql, storedProcedure, parameters)
        {
            int parValCount = 0;
            foreach (DbParam par in parameters)
                if (par is DbParamValue)
                    parValCount++;

            if (parValCount > 0)
            {
                if (parValCount < parameters.Count()) throw new Exception("��������� ���������� ������ ��������� ��������� ������ ���� (���� DbParam, ���� DbParamValue)");

                object[] values = new object[parameters.Length];                
                for (int i = 0; i < parameters.Length; i++)                   
                    values[i] = ((DbParamValue)parameters[i]).Value;                
                paramValues.Add(values);
            }
        }

        /*public NonSelectQuery(string sql, bool storedProcedure, params DbParamValue[] parameters)
            : base(QueryType.NonSelectQuery, sql, storedProcedure, parameters)
        {
            isBatch = false;
        }*/
        
        /*private DataColumnCollection GetColumns(DbParam[] parameters)
        {
            DataColumnCollection retval = new DataColumnCollection();

            foreach (DbParam param in parameters)
            {
                retval.Add(param.Name, param.DBType);
            }

            return retval;
        }*/

        public void AddParamValues(params object[] values)
        {
            if (Parameters == null || Parameters.Length == 0)
                throw new Exception("���������� �������� �������� � �������. �� ������ ���������.");

            if (values.Length != Parameters.Length)
                throw new Exception("���������� �������� �� ������������� ���������� ���������� � �������.");

            paramValues.Add(values);
        }

        public void AddParamValues(IEnumerable<object[]> values)
        {
            if (Parameters == null || Parameters.Length == 0)
                throw new Exception("���������� �������� �������� � �������. �� ������ ���������.");

            if (!values.Any()) return;
            if (values.ToArray()[0].Length != Parameters.Length)
                throw new Exception("���������� �������� �� ������������� ���������� ���������� � �������.");

            paramValues.Clear();
            paramValues = values.ToList();
        }

        // DbParam[] paramsDefinitions = null;

        List<object[]> paramValues = new List<object[]>();

        public List<object[]> Values
        {
            get
            {
                return paramValues;
            }
        }

        public bool IsBatch
        {/* isBatch;*/
            get
            {
                return paramValues.Count > 1 ||
                       (Parameters != null && Parameters.Length > 0 && !(Parameters[0] is DbParamValue)); // additional check (need to simplify)
            }
        }

        // protected DataTable valuesTable = new DataTable();
    }
}