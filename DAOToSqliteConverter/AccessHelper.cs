using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOToSqliteConverter
{

        public class AccessHelper
        {
            private const string SConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};";

            public List<string> GetAccessTableList(string filepath)
            {
                try
                {
                    var list = new List<string>();
                    var dbe = new DBEngine();
                    var db = dbe.OpenDatabase(filepath, false, false, "");

                    foreach (dao.TableDef tableDef in db.TableDefs)
                    {
                        if (tableDef.Name.Length >= 4 && tableDef.Name.Substring(0, 4).ToLower() == "msys") continue;
                        //if (tableDef.Connect.Length > 0) continue;
                        list.Add(tableDef.Name);
                    }
                    db.Close();
                    db = null;
                    dbe = null;
                    return list;
                }
                catch
                {
                    return new List<string>();
                }
            }

            public List<string> GetAccessQueryList(string filepath)
            {
                try
                {
                    var list = new List<string>();
                    var dbe = new DBEngine();
                    var db = dbe.OpenDatabase(filepath, false, false, "");

                    foreach (dao.QueryDef queryDef in db.QueryDefs)
                    {
                        if (queryDef.Name.Length >= 4 && queryDef.Name.Substring(0, 4).ToLower() == "msys") continue;
                        if (queryDef.Connect.Length > 0) continue;
                        list.Add(queryDef.Name);
                    }
                    db.Close();
                    db = null;
                    dbe = null;
                    return list;
                }
                catch
                {
                    return new List<string>();
                }
            }

            public List<string> GetAccessTableFields(string filepath, string tableName)
            {
                if (!string.IsNullOrWhiteSpace(filepath))
                {
                    try
                    {
                        var list = new List<string>();

                        var dbe = new DBEngine();
                        var db = dbe.OpenDatabase(filepath, false, false, "");
                        var table = db.TableDefs[tableName];
                        foreach (dao.Field wField in table.Fields)
                        {
                            list.Add(wField.Name);
                        }
                        db.Close();
                        db = null;
                        dbe = null;
                        return list;
                    }
                    catch
                    {
                        return new List<string>();
                    }
                }
                else
                    return new List<string>();
            }

            public List<string> GetAccessQueryFields(string filepath, string queryName)
            {
                if (!string.IsNullOrWhiteSpace(filepath))
                {
                    try
                    {
                        var list = new List<string>();

                        var dbe = new DBEngine();
                        var db = dbe.OpenDatabase(filepath, false, false, "");
                        var queryDef = db.QueryDefs[queryName];
                        foreach (dao.Field wField in queryDef.Fields)
                        {
                            list.Add(wField.Name);
                        }
                        db.Close();
                        db = null;
                        dbe = null;
                        return list;
                    }
                    catch
                    {
                        return new List<string>();
                    }
                }
                else
                    return new List<string>();
            }

            public AccessFieldType GetFieldType(string filepath, string tableOrQueryName, string fieldName)
            {
                if (!string.IsNullOrWhiteSpace(filepath))
                {
                    try
                    {
                        AccessFieldType fieldType = AccessFieldType.dbUnknown;

                        var dbe = new DBEngine();
                        var db = dbe.OpenDatabase(filepath, false, false, "");
                        var table = db.TableDefs[tableOrQueryName];
                        foreach (dao.Field wField in table.Fields)
                        {
                            if (wField.Name.ToLower() == fieldName.ToLower())
                            {
                                fieldType = (AccessFieldType)wField.Type;
                                break;
                            }
                        }

                        if (fieldType == AccessFieldType.dbUnknown)
                        {
                            var queryDef = db.QueryDefs[tableOrQueryName];

                            foreach (dao.Field wField in queryDef.Fields)
                            {
                                if (wField.Name.ToLower() == fieldName.ToLower())
                                {
                                    fieldType = (AccessFieldType)wField.Type;
                                    break;
                                }
                            }
                        }

                        db.Close();
                        db = null;
                        dbe = null;

                        return fieldType;
                    }
                    catch
                    {
                        return AccessFieldType.dbUnknown;
                    }
                }
                else
                    return AccessFieldType.dbUnknown;
            }

            public DataTable GetAccessData(string filepath, string sql)
            {
                try
                {

                    OleDbConnection conn = new OleDbConnection(string.Format(SConnectionString, filepath));
                    conn.Open();
                    var cmd = new OleDbCommand(sql, conn);
                    var adapter = new OleDbDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    conn.Close();
                    conn = null;
                    cmd = null;
                    adapter = null;
                    return table;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public List<string> GetDistinctValues(string filepath, string tablename, string fieldName)
            {
                try
                {
                    string sql = string.Format("SELECT DISTINCT {0} FROM {1}", fieldName, tablename);
                    DataTable table = GetAccessData(filepath, sql);
                    var list = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        list.Add(row[0].ToString());
                    }
                    table = null;
                    return list;
                }
                catch (Exception)
                {
                    return new List<string>();
                }
            }

            public enum AccessFieldType
            {
                dbUnknown = 0,
                dbBoolean = 1,
                dbByte = 2,
                dbInteger = 3,
                dbLong = 4,
                dbCurrency = 5,
                dbSingle = 6,
                dbDouble = 7,
                dbDate = 8,
                dbBinary = 9,
                dbText = 10,
                dbLongBinary = 11,
                dbMemo = 12,
                dbGUID = 15,
                dbBigInt = 16,
                dbVarBinary = 17,
                dbChar = 18,
                dbNumeric = 19,
                dbDecimal = 20,
                dbFloat = 21,
                dbTime = 22,
                dbTimeStamp = 23,
                dbAttachment = 101,
                dbComplexByte = 102,
                dbComplexInteger = 103,
                dbComplexLong = 104,
                dbComplexSingle = 105,
                dbComplexDouble = 106,
                dbComplexGUID = 107,
                dbComplexDecimal = 108,
                dbComplexText = 109
            }               

    }
}
