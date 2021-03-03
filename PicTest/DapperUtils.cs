using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PicTest
{
    public class DapperUtils
    {
        private static string _connString = "server=localhost;port=3306;user id=root;password=123456;database=smartbookshelf_by;";
        private static object _obj = new object();
        private static bool _dbConn = false;

        public static string Conn
        {
            set
            {
                _connString = value;
            }
        }

        public static int Execute(string sql, object model)
        {
            int rt = 0;
            try
            {
                Monitor.Enter(_obj);
                using (IDbConnection connection = new MySqlConnection(_connString))
                {
                    rt = connection.Execute(sql, model);
                }
                return rt;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw ex;
                //LoggerHelper.Error($"MySqlHelper.Execute  sql:{sql}\r\n  {ex.Message}\r\n{ex.StackTrace}");
                //_dbConn = false;
                //return rt;
            }
            finally
            {
                Monitor.Exit(_obj);
            }
        }

        public static int Execute(string sql)
        {
            int rt = 0;
            try
            {
                Monitor.Enter(_obj);
                using (IDbConnection connection = new MySqlConnection(_connString))
                {
                    rt = connection.Execute(sql);
                }
                _dbConn = true;
                return rt;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw ex;
                //LoggerHelper.Error($"MySqlHelper.Execute  sql:{sql}\r\n  {ex.Message}\r\n{ex.StackTrace}");
                //_dbConn = false;
                //return rt;
            }
            finally
            {
                Monitor.Exit(_obj);
            }
        }

        public static List<T> Query<T>(string sql) where T : class
        {
            List<T> list = new List<T>();
            try
            {
                Monitor.Enter(_obj);
                using (IDbConnection connection = new MySqlConnection(_connString))
                {
                    list = connection.Query<T>(sql)?.ToList();
                }
                _dbConn = true;
                return list;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw ex;
                //LoggerHelper.Error($"MySqlHelper.Query  sql:{sql}\r\n  {ex.Message}\r\n{ex.StackTrace}");
                //_dbConn = false;
                //return list;
            }
            //catch (Exception ex)
            //{
            //    LoggerHelper.Error($"SqlHelper.Query  sql:{sql}\r\n  {ex.Message}\r\n{ex.StackTrace}");
            //    return list;
            //}
            finally
            {
                Monitor.Exit(_obj);
            }
        }

        public static T QueryFirst<T>(string sql)
        {
            T t = default(T);
            try
            {
                Monitor.Enter(_obj);
                using (IDbConnection connection = new MySqlConnection(_connString))
                {
                    t = connection.QueryFirst<T>(sql);
                }
                _dbConn = true;
                return t;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw ex;
                //LoggerHelper.Error($"MySqlHelper.Query  sql:{sql}\r\n  {ex.Message}\r\n{ex.StackTrace}");
                //_dbConn = false;
                //return t;
            }
            //catch (Exception ex)
            //{
            //    LoggerHelper.Error($"SqlHelper.Query  sql:{sql}\r\n  {ex.Message}\r\n{ex.StackTrace}");
            //    return list;
            //}
            finally
            {
                Monitor.Exit(_obj);
            }
        }
    }
}
