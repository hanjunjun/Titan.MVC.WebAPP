/************************************************************************
 * 文件名：CodeToolsHelper
 * 文件功能描述：代码生成器帮助类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.CodeDom.Compiler;

namespace Titan.Infrastructure.Domain
{
    public class CodeToolsHelper
    {
        public static void Test(string templateFileName)
        {
            //测试动态执行T4
            CustomCmdLineHost host = new CustomCmdLineHost();
            Engine engine = new Engine();
            host.TemplateFileValue = templateFileName;
            string input = System.IO.File.ReadAllText(templateFileName);
            string output = engine.ProcessTemplate(input, host);
            string outputFileName = "1" + host.FileExtension;
            System.IO.File.WriteAllText(outputFileName, output, host.FileEncoding);

            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine(error.ToString());
            }
        }
    }
}