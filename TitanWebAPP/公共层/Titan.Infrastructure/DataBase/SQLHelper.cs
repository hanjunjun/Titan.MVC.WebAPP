using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Collections.Concurrent;

namespace Titan.Infrastructure.DataBase
{
    public class SQLHelper
    {
        private string strConnectionString;// = ConfigurationManager.ConnectionStrings["sqlserver"].ConnectionString;
        public SqlConnection cnn = null;
        private SqlCommand cmd = null;

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConn()
        {
            if (cnn == null)
            {
                cnn = new SqlConnection(strConnectionString);
                //return cnn;
            }
            if (cnn.State == ConnectionState.Closed)
            {
                try
                {
                    cnn.Open();
                    return cnn;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }


            }
            return cnn;
        }


        //    }
        //    return cnn;
        //}
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void OutConn()
        {
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 确认连接是否已经关闭
            if (cnn != null)
            {
                cnn.Dispose();

                cnn = null;
            }
        }

        /// <summary>
        /// 使用默认数据库
        /// </summary>
        public SQLHelper()
        {
            strConnectionString = ConfigurationManager.ConnectionStrings["sqlserver"].ToString();
            //cnn = new SqlConnection(strConnectionString);
        }

        /// <summary>
        /// 初始化SqlServer数据库操作类，指定连接数据库
        /// </summary>
        /// <param name="sqlName">SqlServer数据库连接串</param>
        /// <param name="ConfigType">连接配置 0：从配置文件中读取连接串 1：从数据库中读取连接串直接传进来</param>
        public SQLHelper(string sqlName, int ConfigType)
        {
            if (ConfigType == 0)
            {
                strConnectionString = ConfigurationManager.ConnectionStrings[sqlName].ConnectionString;
                //cnn = new SqlConnection(strConnectionString);
            }
            else if (ConfigType == 1)
            {
                strConnectionString = sqlName;
            }

        }

        #region SQLDataReader
        /// 执行不带参数的增删改SQL语句或存储过程  
        /// <summary>
        /// 执行不带参数的增删改SQL语句或存储过程  
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType ct)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OutConn();
            }
            return res;
        }
        public int ExecuteNonQuery(string cmdText)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = CommandType.Text;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OutConn();
            }
            return res;
        }
        /// 执行带参数的增删改SQL语句或存储过程  
        /// <summary>
        /// 执行含参SQL语句-增 删 改
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType ct, params SqlParameter[] paras)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OutConn();
            }
            return res;
        }

        /// 执行无参查询
        /// <summary>
        /// 执行无参查询--返回首行首列
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, CommandType ct)
        {
            object retval = null;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OutConn();
            }
            return retval;
        }

        /// 执行有参查询
        /// <summary>
        /// 执行有参查询--返回首行首列
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, CommandType ct, params SqlParameter[] paras)
        {
            object retval = null;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OutConn();
            }
            return retval;
        }

        /// 执行无参查询
        /// <summary>
        /// 执行无参查询 返回数据集
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string cmdText, CommandType ct)
        {
            SqlDataReader dr;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception ee)
            {
                cnn.Close();
                throw ee;
            }

        }

        /// 执行无参查询 返回数据集
        /// <summary>
        /// 执行无参查询 返回数据集
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string cmdText, CommandType ct, params SqlParameter[] paras)
        {
            SqlDataReader dr;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception ee)
            {
                cnn.Close();
                throw ee;
            }
        }
        #endregion

        #region DataTable
        #region 执行一个查询，并返回结果集

        /// <summary>
        /// 执行一个查询，并返回结果集
        /// </summary>
        /// <param name="sql">要查询的SQL文本命令</param>
        /// <returns>查询结果集</returns>
        public DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 执行一个查询，并返回结果集
        /// </summary>
        /// <param name="sql">要查询的SQL文本命令</param>
        /// <param name="commandType">查询语句类型，存储过程或SQL文本命令</param>
        /// <returns>查询结果集</returns>
        public DataTable ExecuteDataTable(string sql, CommandType commandType)
        {
            return ExecuteDataTable(sql, commandType, null);
        }

        /// <summary>
        /// 执行一个查询，并返回结果集
        /// </summary>
        /// <param name="sql">要查询的SQL文本命令</param>
        /// <param name="commandType">查询语句类型，存储过程或SQL文本命令</param>
        /// <param name="parameters">T-SQL语句或存储过程的参数组</param>
        /// <returns>查询结果集</returns>
        public DataTable ExecuteDataTable(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            //实例化DataTable，用于装载查询结果集
            try
            {
                DataTable data = new DataTable();
                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        //指定CommandType
                        command.CommandType = commandType;

                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        //实例化SqlDataAdapter
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        //填充DataTable
                        adapter.Fill(data);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region 老的
        ///// 执行无参数的查询SQL语句或存储过程
        ///// <summary>
        ///// 执行无参数的查询SQL语句或存储过程
        ///// </summary>
        ///// <param name="cmdText"></param>
        ///// <param name="ct"></param>
        ///// <returns></returns>
        //public DataTable ExecuteDataTable(string cmdText, CommandType ct)
        //{
        //    DataTable dt = new DataTable();
        //    cmd = new SqlCommand(cmdText, GetConn());
        //    cmd.CommandType = ct;
        //    using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        //    {
        //        dt.Load(sdr);
        //    }
        //    dt.TableName = "NewTable1";
        //    return dt;
        //}

        //public DataTable ExecuteDataTable(string cmdText)
        //{
        //    DataTable dt = new DataTable();
        //    cmd = new SqlCommand(cmdText, GetConn());
        //    cmd.CommandType = CommandType.Text;
        //    using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        //    {
        //        dt.Load(sdr);
        //    }
        //    dt.TableName = "NewTable1";
        //    return dt;
        //}
        #endregion

        ///// 执行无参数的查询SQL语句或存储过程
        ///// <summary>
        ///// 执行无参数的查询SQL语句或存储过程
        ///// </summary>
        ///// <param name="cmdText"></param>
        ///// <param name="ct"></param>
        ///// <returns></returns>
        //public DataTable ExecuteDataTable(string cmdText, CommandType ct, enDataBase endb)
        //{
        //    DataTable dt = new DataTable();
        //    cmd = new SqlCommand(cmdText, GetConn(endb));
        //    cmd.CommandType = ct;
        //    using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        //    {
        //        dt.Load(sdr);
        //    }
        //    dt.TableName = "NewTable1";
        //    return dt;
        //}


        ///// 执行带参数的查询SQL语句或存储过程  
        ///// <summary>
        ///// 执行带参数的查询SQL语句或存储过程  
        ///// </summary>
        ///// <param name="cmdText"></param>
        ///// <param name="paras"></param>
        ///// <param name="ct"></param>
        ///// <returns></returns>
        //public DataTable ExecuteDataTable(string cmdText, CommandType ct, params SqlParameter[] paras)
        //{
        //    DataTable dt = new DataTable();
        //    cmd = new SqlCommand(cmdText, GetConn());
        //    cmd.CommandType = ct;
        //    cmd.Parameters.AddRange(paras);
        //    using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        //    {
        //        dt.Load(sdr);
        //    }
        //    dt.TableName = "NewTable1";
        //    return dt;
        //}
        #endregion

        #region DataSet
        /// <summary>
        /// 执行无参查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText, CommandType ct)
        {

            SqlCommand cmd = new SqlCommand(cmdText, GetConn());

            cmd.CommandType = ct;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// 执行有参查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText, CommandType ct, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(cmdText, GetConn());

            cmd.CommandType = ct;

            cmd.Parameters.AddRange(paras);

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        #endregion


        #region 事物
        /// <summary>
        /// 执行事物
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                            count++;
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch (Exception exp)
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }


        /// <summary>
        /// 执行含参事物
        /// </summary>
        /// <param name="sqlDis"></param>
        /// <param name="parasDis"></param>
        /// <returns></returns>
        public int ExecuteSqlTran(ConcurrentDictionary<string, string> sqlDis, ConcurrentDictionary<string, SqlParameter[]> parasDis)
        {
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    foreach (var sd in sqlDis)
                    {
                        string strsql = sd.Value;
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            foreach (var pd in parasDis)
                            {
                                if (pd.Key == sd.Key && pd.Value != null)
                                {
                                    cmd.Parameters.AddRange(pd.Value);
                                }
                            }

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            count++;
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }

        #endregion


        #region DataTable分页
        /// DataTable
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="orderby"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryReturnDataSet(string cmdText, string orderby, int page, int pageSize, out int total, params SqlParameter[] commandParameters)
        {
            DataTable dt = new DataTable();
            total = 0;

            if (!string.IsNullOrEmpty(orderby))
            {
                orderby = " ORDER BY T." + orderby;
            }
            else
            {
                return dt;
            }

            string sql = string.Format("set @SqlTotalCount=(select count(1) from ({0}) C) SELECT * FROM(SELECT ROW_NUMBER()OVER( {1} )RN,T.* FROM ({0})T) c WHERE RN>({2} * ({3}-1))  AND RN<=({2} * ({3}))", cmdText, orderby, pageSize, page);
            SqlParameter param = new SqlParameter("@SqlTotalCount", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            List<SqlParameter> paramList;
            if (commandParameters == null)
            {
                paramList = new List<SqlParameter>();
            }
            else
            {
                paramList = commandParameters.ToList();
            }
            paramList.Add(param);
            dt = ExecuteDataTable(sql, CommandType.Text, paramList.ToArray());
            total = (int)param.Value;
            return dt;
        }


        /// DataReader分页
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="orderby"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteQueryReturnDataReader(string cmdText, string orderby, int page, int pageSize, out int total, params SqlParameter[] commandParameters)
        {
            total = 0;
            SqlDataReader dr;
            if (!string.IsNullOrEmpty(orderby))
            {
                orderby = " ORDER BY T." + orderby;
            }
            else
            {
                return null;
            }

            string sql = string.Format("set @SqlTotalCount=(select count(1) from ({0}) C) SELECT * FROM(SELECT ROW_NUMBER()OVER( {1} )RN,T.* FROM ({0})T) c WHERE RN>({2} * ({3}-1))  AND RN<=({2} * ({3}))", cmdText, orderby, pageSize, page);
            SqlParameter param = new SqlParameter("@SqlTotalCount", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            List<SqlParameter> paramList;
            if (commandParameters == null)
            {
                paramList = new List<SqlParameter>();
            }
            else
            {
                paramList = commandParameters.ToList();
            }
            paramList.Add(param);
            dr = ExecuteReader(sql, CommandType.Text, paramList.ToArray());
            total = (int)param.Value;
            return dr;
        }



        public DataTable ExecuteQueryReturnDataSet(string cmdText, string cmdTextpage, string orderby, int page, int pageSize, out int total, params SqlParameter[] commandParameters)
        {
            DataTable dt = new DataTable();
            total = 0;

            if (!string.IsNullOrEmpty(orderby))
            {
                orderby = " ORDER BY T." + orderby;
            }
            else
            {
                return dt;
            }

            string sql = string.Format("set @SqlTotalCount=(select count(1) from ({0}) C) SELECT * FROM(SELECT ROW_NUMBER()OVER( {1} )RN,T.* FROM ({4})T) c WHERE RN>({2} * ({3}-1))  AND RN<=({2} * ({3}))", cmdText, orderby, pageSize, page, cmdTextpage);
            SqlParameter param = new SqlParameter("@SqlTotalCount", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            List<SqlParameter> paramList;
            if (commandParameters == null)
            {
                paramList = new List<SqlParameter>();
            }
            else
            {
                paramList = commandParameters.ToList();
            }
            paramList.Add(param);
            dt = ExecuteDataTable(sql, CommandType.Text, paramList.ToArray());
            total = (int)param.Value;
            return dt;
        }

        #endregion
    }
}
