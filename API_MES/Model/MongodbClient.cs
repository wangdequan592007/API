//using System;
//using API_MES.Options;
//using MongoDB.Driver;

//namespace API_MES.Model
//{
//    public static class MongodbClient<T> where T : class
//    {
//        #region +MongodbInfoClient 获取mongodb实例
//        /// <summary>
//        /// 获取mongodb实例
//        /// </summary>
//        /// <param name="host">连接字符串，库，表</param>
//        /// <returns></returns>
//        public static IMongoCollection<T> MongodbInfoClient(MongodbHostOptions host)
//        {

//            MongoClient client = new MongoClient(host.Connection);
//            var dataBase = client.GetDatabase(host.DataBase);
//            if (string.IsNullOrEmpty(host.Table))
//            {
//                return dataBase.GetCollection<T>(typeof(T).Name);
//            }
//            else
//            {
//                return dataBase.GetCollection<T>(host.Table);
//            }

//        }
//        #endregion
//    }
//}