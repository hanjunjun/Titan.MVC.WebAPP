/************************************************************************
 * 文件名：AuthInfo
 * 文件功能描述：xx控制层
 * 作    者：  韩俊俊
 * 创建日期：2018/11/26 12:01:08
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
    public class AuthInfo
    {
        //模拟JWT的payload
        public string UserName { get; set; }

        public List<string> Roles { get; set; }

        public bool IsAdmin { get; set; }
    }
}
