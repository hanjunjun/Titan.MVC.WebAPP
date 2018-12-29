using System;

namespace Titan.Infrastructure.Domain
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class ControllersTypeAttribute : Attribute
    {
        private string _strControllerTypeName = string.Empty;
        public string ControllerTypeName
        {
            get { return (_strControllerTypeName); }
            set { _strControllerTypeName = value; }
        }

        private string _ataterValue = string.Empty;
        public string AtaterValue
        {
            get { return (_ataterValue); }
            set { _ataterValue = value; }
        }

    }
}