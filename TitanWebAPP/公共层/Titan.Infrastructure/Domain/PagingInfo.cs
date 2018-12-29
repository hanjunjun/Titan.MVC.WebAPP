/************************************************************************
 * 文件名：PagingInfo
 * 文件功能描述：分页操作属性类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;

namespace Titan.Infrastructure.Domain
{
    public class PagingInfo
    {
        //项目总数量
        public int TotalItems { get; set; }
        //当前索引
        public int PageIndex { get; set; }
        //分页大小
        public int PageSize { get; set; }
        //页数
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }
    }
}