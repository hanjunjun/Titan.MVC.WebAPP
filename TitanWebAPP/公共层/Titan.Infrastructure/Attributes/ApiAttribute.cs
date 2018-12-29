using System;

namespace Titan.Infrastructure.Attributes
{
    /// <summary>
    /// Api特性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiAttribute : Attribute
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public double Version { get; set; }
    }

    /// <summary>
    /// Api版本异常
    /// </summary>
    public class ApiVersionException : Exception
    {
        /// <summary>
        /// Api版本异常
        /// </summary>
        /// <param name="message"></param>
        public ApiVersionException(string message = "多个方法标记为相同版本") : base(message)
        { }
    }
}
