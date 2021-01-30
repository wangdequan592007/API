﻿using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace API_MES.Data
{
    public class DapperHelper
    {
        //static IDbConnection _dbConnection = new SqlConnection();
        //public string ConnectionString => ConnectionOption.ConnectionString;
        //public DapperHelper() {
        //    if (string.IsNullOrEmpty(_dbConnection.ConnectionString))
        //    {
        //        _dbConnection.ConnectionString = ConnectionString;
        //    }

        //}
        /// <summary>
        /// 单个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ConnectionOption.DbConnection.Open();
            using (transaction = ConnectionOption.DbConnection.BeginTransaction())
            {
                var user = ConnectionOption.DbConnection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                transaction.Commit();
                ConnectionOption.DbConnection.Close();
                return user;
            }
        }
        /// <summary>
        /// 多个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">缓冲/缓存</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ConnectionOption.DbConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public int Execute<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ConnectionOption.DbConnection.Execute(sql, param, transaction, commandTimeout, commandType);
        }
    }
}

