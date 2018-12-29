using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Titan.Model;

namespace Titan.Infrastructure.Domain
{
    public static class SqlHelper
    {
        /// <summary>
        /// 动态生成简单的SQL
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string Add<T>(T entity) where T : IAggregateRoot
        {
            try
            {
                StringBuilder strFields = new StringBuilder();
                StringBuilder strFieldsValue = new StringBuilder();
                PropertyInfo[] proInfos = entity.GetType().GetProperties();
                foreach (var pro in proInfos)
                {
                    var value = pro.GetValue(entity, null) ?? DBNull.Value;
                    var typename = pro.PropertyType.Name;
                    var isNullable = pro.PropertyType.IsGenericType && pro.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    if (isNullable)
                    {
                        typename = pro.PropertyType.GetGenericArguments()[0].Name; ;
                    }
                    if (pro.PropertyType.FullName != null && !pro.PropertyType.FullName.ToLower().Contains("system."))//不是.net的原生类型
                    {
                        continue;
                    }
                    if (pro.PropertyType.IsGenericType && !isNullable)//
                    {
                        continue;
                    }
                    if (pro.PropertyType.IsEnum)//枚举类型
                    {
                        continue;
                    }
                    if (pro.PropertyType.IsArray)//
                    {
                        continue;
                    }
                    switch (typename.ToUpper())
                    {
                        case "DATETIME":
                            DateTime dtRet;
                            if (DateTime.TryParse(value.ToString(), out dtRet) && dtRet.Year >= 1753)
                            {
                                strFields.Append(pro.Name + ",");
                                strFieldsValue.Append(
                                    $@"'{dtRet:yyyy-MM-dd HH:mm:ss:fff}',");
                            }
                            break;
                        case "BOOLEAN":
                            int b = 0;
                            bool bb;
                            if (Boolean.TryParse(value.ToString(), out bb))
                            {
                                if (bb)
                                {
                                    b = 1;
                                }
                            }
                            strFields.Append(pro.Name + ",");
                            strFieldsValue.Append($@"'{b}',");
                            break;
                        case "GUID":
                            var strGuid = value.ToString().ToUpper();
                            if (value.Equals(Guid.Empty) || strGuid == "")
                            {
                                continue;
                            }
                            strFields.Append(pro.Name + ",");
                            strFieldsValue.Append($@"'{strGuid}',");
                            break;
                        default:
                            strFields.Append(pro.Name + ",");
                            strFieldsValue.AppendFormat("'{0}',", value);
                            break;
                    }
                }
                var strFields2 = strFields.ToString().Substring(0, strFields.Length - 1);
                var strFieldsValue2 = strFieldsValue.ToString().Substring(0, strFieldsValue.Length - 1);
                string strSql = $"INSERT INTO {entity.GetType().Name}({strFields2}) VALUES({strFieldsValue2});";
                return strSql;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 动态生成简单的SQL
        /// </summary>
        public static string Adds<T>(List<T> entitys) where T : IAggregateRoot
        {
            var sql = string.Empty;
            if (entitys!=null && entitys.Count>0)
            {
                foreach (var entity in entitys)
                {
                    sql += Add(entity);
                }
            }
            return sql;
        }

        // 执行Sql
        //public static void ExecSql(string sql)
        //{
        //    var newContext = new ModelBaseContext();
        //    //设置永不超时
        //    ((IObjectContextAdapter)newContext).ObjectContext.CommandTimeout = 0;
        //    newContext.Database.ExecuteSqlCommand(sql);
        //}
    }
}
