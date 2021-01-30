using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API_COM.Helper
{
    public static class DataTableExtensions
    {
        #region Fields

        /// <summary>
        /// 序号列列名
        /// </summary>
        private const string RowNoColumnName = "_RowNo";

        #endregion Fields

        #region Methods

        /// <summary>
        /// 转化一个DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            return ToDataTable<T>(list, false);
        }

        /// <summary>
        /// 转化一个DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="hasRowNo">是否添加行号</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, bool hasRowNo)
        {  //获得反射的入口
            Type type = typeof(T);
            var colNames = type.GetProperties().Select(i => i.Name);
            return ToDataTable<T>(list, colNames, hasRowNo);
        }

        /// <summary>
        /// 转化一个DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="colNames"></param>
        /// <param name="hasRowNo">是否添加行号</param>
        /// <returns></returns> 
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, IEnumerable<string> colNames, bool hasRowNo)
        {
            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(T);
            DataTable dt = new DataTable();
            if (hasRowNo)
                dt.Columns.Add(RowNoColumnName, typeof(Int32));
            var properties = type.GetProperties();
            //把所有的public属性加入到集合 并添加DataTable的列
            foreach (var colName in colNames)
            {
                var p = properties.FirstOrDefault(i => i.Name == colName);
                if (p != null)
                {
                    pList.Add(p); dt.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);
                }
            }

            int index = 1;
            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                if (hasRowNo)
                    row[RowNoColumnName] = index;
                index++;
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null) ?? DBNull.Value);
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        /// 转化一个DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="colNames"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, IEnumerable<string> colNames)
        {
            return ToDataTable(list, colNames, false);
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable<T>(list, null);
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static IEnumerable<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = new T();
                //找到对应的数据  并赋值
                prlist.ForEach(p =>
                {
                    if (row[p.Name] != DBNull.Value)
                    {
                        try
                        {
                            if (p.PropertyType == typeof(string))
                            {
                                p.SetValue(ob, row[p.Name].ToString(), null);
                            }
                            else if (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?))
                            {
                                p.SetValue(ob, int.Parse(row[p.Name].ToString()), null);
                            }
                            else if (p.PropertyType == typeof(DateTime?) || p.PropertyType == typeof(DateTime))
                            {
                                p.SetValue(ob, DateTime.Parse(row[p.Name].ToString()), null);
                            }
                            else if (p.PropertyType == typeof(float))
                            {
                                p.SetValue(ob, float.Parse(row[p.Name].ToString()), null);
                            }
                            else if (p.PropertyType == typeof(double))
                            {
                                p.SetValue(ob, double.Parse(row[p.Name].ToString()), null);
                            }
                            else
                            {
                                p.SetValue(ob, row[p.Name], null);
                            }
                        }
                        catch (Exception)
                        {
                            //TODO 读取excel数据异常处理
                            //LogFactory.GetLogger(typeof(DataTableExtensions)).Error(ex.Message);
                        }
                    }
                });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }

        #endregion Methods

        /**/
        /**/
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable2(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData2(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private static DataTable CreateData2(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }
    }
}
