using System;

namespace Titan.Infrastructure.Attributes
{
    /// <summary>
    /// 分页
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PagedAttribute : Attribute
    {
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; } = 5;

        /// <summary>
        /// 返回参数（仅在ReturnPageInfo为true是有意义）
        /// </summary>
        public bool ReturnParas { get; set; } = false;

        /// <summary>
        /// 返回url（仅在ReturnPageInfo为true是有意义）
        /// </summary>
        public bool ReturnUrl { get; set; } = false;

        /// <summary>
        /// 返回PageInfo
        /// </summary>
        public bool ReturnPageInfo { get; set; } = true;
    }
}
