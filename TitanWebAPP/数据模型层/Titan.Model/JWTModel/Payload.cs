/************************************************************************
 * 文件名：Payload
 * 文件功能描述：xx控制层
 * 作    者：  韩俊俊
 * 创建日期：2018/11/26 17:08:55
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2017 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Model.JWTModel
{
    #region Payload实体
    public class Payload
    {
        /// <summary>
        /// iss: 该JWT的签发者，是否使用是可选的；
        /// </summary>
        public string iss { get; set; }

        /// <summary>
        /// iat(issued at): 在什么时候签发的(UNIX时间)，是否使用是可选的；
        /// </summary>
        public long iat { get; set; }

        /// <summary>
        /// exp(expires): 什么时候过期，这里是一个Unix时间戳，是否使用是可选的；
        /// </summary>
        public double exp { get; set; }

        /// <summary>
        /// aud: 接收该JWT的一方，是否使用是可选的；
        /// </summary>
        public string aud { get; set; }

        /// <summary>
        /// sub: 该JWT所面向的用户，是否使用是可选的；
        /// </summary>
        public string sub { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public List<string> Role { get; set; }
    }
    #endregion
}
