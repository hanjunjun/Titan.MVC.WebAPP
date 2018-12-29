using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Titan.AppService.DomainService;
using Titan.AppService.ModelOtherService;
using Titan.AppService.ModelService;
using Titan.Infrastructure.DataBase;
using Titan.Infrastructure.Domain;
using Titan.Infrastructure.Verify;
using Titan.Model.DataModel;
using Titan.Model.JWTModel;
using Titan.RepositoryCode;

namespace Titan.WebAPPTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VerifyCodeHelper vCode = new VerifyCodeHelper();
            vCode.Height = 36;
            vCode.Width = 116;
            var code = vCode.GetRandomString(4);
            var img = vCode.CreateImage(code);
            
            pictureBox1.Image = BytesToImage(img);
            MessageBox.Show($"生成验证码：{code}", "msg");
            //   VerifyCodeHelper vCode = new VerifyCodeHelper();
            //string code = vCode.CreateValidateCode(6);
            //byte[] bytes = vCode.CreateValidateGraphic(code);
            //var img = BytesToImage(bytes);
            //pictureBox1.Image = img;
        }

        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public  Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            var startTime = now.Date.AddMonths(1).AddDays(-now.Day).AddDays(1);
            var endTime = now.Date.AddDays(1 - now.Day).AddMonths(2).AddSeconds(-1);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string connectionstring = "server=112.74.51.95;Initial Catalog=Titan;User ID=sa;Password=Hanhongwei123!;";
            SQLHelper sQLHelper = new SQLHelper(connectionstring, 1);
            DataTable ds = sQLHelper.ExecuteDataTable("select * from [SysTitle] where TitleFatherId is null order by TitleOrderIndex", CommandType.Text);
            List<string> list = new List<string>();
            TreeDataBind(ds, list);
        }

        public void TreeDataBind(DataTable dv, List<string> tnOld)
        {
            for (int i = 0; i < dv.Rows.Count; i++)
            {
                tnOld.Add(dv.Rows[i]["TitleName"].ToString());
                string connectionstring = "server=112.74.51.95;Initial Catalog=Titan;User ID=sa;Password=Hanhongwei123!;";
                SQLHelper sQLHelper = new SQLHelper(connectionstring, 1);
                DataTable db = sQLHelper.ExecuteDataTable($"select * from [SysTitle] where TitleFatherId ='{dv.Rows[i]["SysTitleId"]}' order by TitleOrderIndex", CommandType.Text);
                TreeDataBind(db,tnOld);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string esption = "(((((50)*1.1)+50.0000)*1.1)*1.1)";
            DataTable ds=new DataTable();
            var s = ds.Compute(esption, "");
            var b = s.ToString();
            decimal money = decimal.Parse(b);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();

            var now = provider.GetNow();

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);
            var payload = new Dictionary<string, object>{
                { "name", "MrBug" },
                {"exp",secondsSinceEpoch+10 },
                {"jti","luozhipeng" }
            };

            Console.WriteLine(secondsSinceEpoch);

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            string secret = "123";//密钥
            var token = encoder.Encode(payload, secret);
            Console.WriteLine(token);

            Decrypt(token, secret);
            Decrypt("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJVc2VyTmFtZSI6IndhbmdzaGliYW5nIiwiUm9sZXMiOlsiQWRtaW4iLCJNYW5hZ2UiXSwiSXNBZG1pbiI6dHJ1ZX0.YDxT9Ut28ei8N9fh_FM0h7QoQVFS503bULFsuRWihLc", secret);
        }

        /// <summary>
        ///  解密
        /// </summary>
        /// <param name="token">token信息</param>
        /// <param name="secret">密钥</param>
        private static void Decrypt(string token, string secret)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);//token为之前生成的字符串
                Console.WriteLine(json);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoginRequest model = new LoginRequest();
            model.UserName = "hjj";
            model.Password = "123";
            Console.WriteLine(JsonHelper.ToJsonStr(model));
        }
    }
}
