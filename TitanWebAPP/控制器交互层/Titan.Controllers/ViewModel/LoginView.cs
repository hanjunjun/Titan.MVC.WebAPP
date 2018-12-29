using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.AppService.ModelDTO;

namespace Titan.Controllers.ViewModel
{
    public class LoginView
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassWord { get; set; }

        /// <summary>
        /// 登录验证码
        /// </summary>
        public string VerifyCode { get; set; }
    }

    public class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 登录人用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 当前登录机构名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 当前机构Id
        /// </summary>
        public Guid? CID { get; set; }

        /// <summary>
        /// 用户所属机构Id
        /// </summary>
        public Guid? UserCID { get; set; }

        /// <summary>
        /// 用户部门Id
        /// </summary>
        public Guid? DeptId { get; set; }

        /// <summary>
        /// 用户岗位Id
        /// </summary>
        public Guid? PostId { get; set; }

        /// <summary>
        /// 登录系统帐号
        /// </summary>
        public string LoginNo { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 系统标题Title
        /// </summary>
        public string SysTitle { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string EmployeePhotoUrl { get; set; }

        /// <summary>
        /// 机构图标
        /// </summary>
        public string CompanyLogoImgUrl { get; set; }
    }

    public class ChangePwdView
    {
        ///<summary>
        /// 员工旧密码
        ///</summary>
        [Description("员工旧密码")]
        public string EmployeePwd { get; set; } // EmployeePwd (length: 40)

        /// <summary>
        /// 员工新密码
        /// </summary>
        public string EmployeeNewPwd { get; set; }

        /// <summary>
        /// 员工第二次确认密码
        /// </summary>
        public string EmployeeNewPwdSecond { get; set; }
    }
}
