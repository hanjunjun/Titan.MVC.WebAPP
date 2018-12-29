/************************************************************************
 * 文件名：ViewModelBase
 * 文件功能描述：控制显示层母版类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/

using Titan.Controllers.ViewModel.CommonViewModel;
using Titan.Infrastructure.Domain;

namespace Titan.Controllers
{
    public class ViewModelBase
    {

    }

    /// <summary>
    /// 显示层对象列表基类
    /// </summary>
    public class ViewModelListBase
    {
        //分页参数
        public PagingInfo PagingInfo { get; set; }
        //查询条件
        public ViewQuery ViewQuery { get; set; }

        //添加权限控制模块
        public MyPowerModel MyPowerModel { get; set; }
    }

    /// <summary>
    /// 显示层对象创建基类
    /// </summary>
    public class ViewModelCreateBase
    {

    }

    /// <summary>
    /// 显示层对象修改基类
    /// </summary>
    public class ViewModelUpdateBase
    {

    }

    /// <summary>
    /// 行列数据封装
    /// </summary>
    public class RowData
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string Column9 { get; set; }
        public string Column10 { get; set; }
    }
}