﻿using System;
using Oracle.ManagedDataAccess.Client;

namespace OptimaJet.Workflow.Oracle
{
    public class WorkflowGlobalParameter : DbObject<WorkflowGlobalParameter>
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        private const string _tableName = "WorkflowGlobalParameter";

        public WorkflowGlobalParameter()
            : base()
        {
            db_TableName = _tableName;
            db_Columns.AddRange(new ColumnInfo[]
            {
                new ColumnInfo() {Name = "Id", IsKey = true, Type = OracleDbType.Raw},
                new ColumnInfo() {Name = "Type"},
                new ColumnInfo() {Name = "Name"},
                new ColumnInfo() {Name = "Value"}
            });
        }

        public override object GetValue(string key)
        {
            switch (key)
            {
                case "Id":
                    return Id.ToByteArray();
                case "Type":
                    return Type;
                case "Name":
                    return Name;
                case "Value":
                    return Value;
                default:
                    throw new Exception(string.Format("Column {0} is not exists", key));
            }
        }

        public override void SetValue(string key, object value)
        {
            switch (key)
            {
                case "Id":
                    Id = new Guid((byte[])value);
                    break;
                case "Type":
                    Type = value as string;
                    break;
                case "Name":
                    Name = value as string;
                    break;
                case "Value":
                    Value = value as string;;
                    break;
               default:
                    throw new Exception(string.Format("Column {0} is not exists", key));
            }
        }

        public static WorkflowGlobalParameter[] SelectByTypeAndName(OracleConnection connection, string type, string name = null)
        {
            string selectText = string.Format("SELECT * FROM {0}  WHERE Type = :type", _tableName);

            if (!string.IsNullOrEmpty(name))
                selectText = selectText + " OR Name = :name";

            var p = new OracleParameter("type", OracleDbType.NVarchar2) { Value = type };

            if (string.IsNullOrEmpty(name))
                return Select(connection, selectText, p);

            var p1 = new OracleParameter("name", OracleDbType.NVarchar2) { Value = name };

            return Select(connection, selectText, p, p1);
        }

        public static int DeleteByTypeAndName(OracleConnection connection, string type, string name = null)
        {
            string selectText = string.Format("DELETE FROM {0}  WHERE Type = :type", _tableName);

            if (!string.IsNullOrEmpty(name))
                selectText = selectText + " OR Name = :name";

            var p = new OracleParameter("type", OracleDbType.NVarchar2) { Value = type };

            if (string.IsNullOrEmpty(name))
                return ExecuteCommand(connection, selectText, p);

            var p1 = new OracleParameter("name", OracleDbType.NVarchar2) { Value = name };

            return ExecuteCommand(connection, selectText, p, p1);
        }
    }
}