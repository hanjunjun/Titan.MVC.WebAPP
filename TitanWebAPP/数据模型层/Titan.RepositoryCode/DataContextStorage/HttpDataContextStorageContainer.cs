/************************************************************************
 * 文件名：HttpDataContextStorageContainer
 * 文件功能描述：数据上下文缓存工厂
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using Titan.Model.DataModel;
using System.Web;


namespace Titan.RepositoryCode.DataContextStorage
{
    public class HttpDataContextStorageContainer : IDataContextStorageContainer
    {
        private string _dataContextKey = "DataContext";
         
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        public ModelBaseContext GetDataContext()
        {
            ModelBaseContext objectContext = null;
            if (HttpContext.Current.Items.Contains(_dataContextKey))
                objectContext = (ModelBaseContext)HttpContext.Current.Items[_dataContextKey];

            return objectContext;
        }

        /// <summary>
        /// 获取缓存上下文
        /// </summary>
        /// <param name="libraryDataContext"></param>
        public void Store(ModelBaseContext libraryDataContext)
        {
            if (HttpContext.Current.Items.Contains(_dataContextKey))
                HttpContext.Current.Items[_dataContextKey] = libraryDataContext;
            else
                HttpContext.Current.Items.Add(_dataContextKey, libraryDataContext);
        }
    }
}