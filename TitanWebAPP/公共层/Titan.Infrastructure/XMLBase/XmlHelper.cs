using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Titan.Infrastructure.XMLBase
{

    /// <summary>
    /// XmlHelper 的摘要说明。
    /// xml操作类
    /// </summary>
    public class XmlHelper
    {
        protected string strXmlFile;
        protected XmlDocument objXmlDoc = new XmlDocument();

        public XmlHelper(string XmlFile)
        {
            // 
            // TODO: 在这里加入建构函式的程序代码 
            // 
            try
            {
                objXmlDoc.Load(XmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            strXmlFile = XmlFile;
        }

        public DataTable GetData(string XmlPathNode)
        {
            //查找数据。返回一个DataView 
            DataSet ds = new DataSet();
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
            ds.ReadXml(read);
            return ds.Tables[0];
        }
     
        /// <summary>
        /// 新节点内容。
        /// 示例：xmlTool.Replace("Book/Authors[ISBN=\"0002\"]/Content","ppppppp"); 
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <param name="Content"></param>
        public void Replace(string XmlPathNode, string Content)
        {
            //更新节点内容。 
            objXmlDoc.SelectSingleNode(XmlPathNode).InnerText = Content;
        }

        /// <summary>
        /// 删除一个指定节点的子节点。 
        /// 示例： xmlTool.DeleteChild("Book/Authors[ISBN=\"0003\"]"); 
        /// </summary>
        /// <param name="Node"></param>
        public void DeleteChild(string Node)
        {
            //删除一个节点。 
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
        }

        /// <summary>
        ///  * 使用示列:
        ///  示例： XmlHelper.Delete( "/Node", "")
        ///  XmlHelper.Delete( "/Node", "Attribute")
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        public void Delete(string node, string attribute)
        {
            try
            {
                XmlNode xn = objXmlDoc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);

            }
            catch { }
        }
        
        /// <summary>
        /// 插入一节点和此节点的一子节点。 
        /// 示例：xmlTool.InsertNode("Book","Author","ISBN","0004"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="ChildNode">子节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Content">内容</param>
        public void InsertNode(string MainNode, string ChildNode, string Element, string Content)
        {
            //插入一节点和此节点的一子节点。 
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objChildNode.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一个节点，带一属性。
        /// 示例： xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Title","Sex","man","iiiiiiii"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Attrib">属性</param>
        /// <param name="AttribContent">属性内容</param>
        /// <param name="Content">元素内容</param>
        public void InsertElement(string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            //插入一个节点，带一属性。 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }
        /// <summary>
        /// 插入一个节点，不带属性。
        /// 示例：xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Content","aaaaaaaaa"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Content">元素内容</param>
        public void InsertElement(string MainNode, string Element, string Content)
        {
            //插入一个节点，不带属性。 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }

        /// <summary>
        /// 对xml文件做插入，更新，删除后需做Save()操作，以保存修改
        /// </summary>
        public void Save()
        {
            //保存文檔。 
            try
            {
                objXmlDoc.Save(strXmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            objXmlDoc = null;
        }

        public static void SetXmlCompanyIds()
        {
            var path = System.Web.HttpContext.Current.Server.MapPath("~")+ "\\XMLModel";
            var files = Directory.GetFiles(path, "*.xml");
            foreach (var file in files)
            {
                SetXmlCompanyId(file);
            }
        }

        public static void SetXmlCompanyId(string fileName="")
        {
            var strCid = System.Web.HttpContext.Current.Session["CID"].ToString();
            if (string.IsNullOrEmpty(strCid))
            {
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                string xmlpath = System.Web.HttpContext.Current.Server.MapPath("~");
                fileName = xmlpath + "/XMLModel/AgrFeesFile.xml";
            }
            if (System.IO.File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                if (doc.DocumentElement != null)
                {
                    var selectSingleNode = doc.SelectSingleNode("NewDataSet");
                    if (selectSingleNode != null)
                    {
                        var nodeList = selectSingleNode.ChildNodes;
                        foreach (var xn in nodeList)
                        {
                            var xe1 = (XmlElement)xn;
                            var isHaveCid = xe1.GetElementsByTagName("CID");
                            if (isHaveCid==null||isHaveCid.Count==0)
                            {
                                var xe2 = doc.CreateElement("CID");
                                xe2.InnerXml = $"{strCid}";
                                xe1.AppendChild(xe2);
                            }
                        }
                    }
                }
                doc.Save(fileName);
            }
        }

        public static XmlDocument ClearXmlByCid(XmlDocument doc)
        {
            XmlNode xn = doc.SelectSingleNode("NewDataSet");
            XmlNodeList xnl = xn.ChildNodes;
            for (var i = 0; i < xnl.Count; i++)
            {
                XmlNode xn1 = xnl.Item(i);
                XmlElement xe = (XmlElement)xn1;
                var isHaveCid = xe.GetElementsByTagName("text");
                if (isHaveCid != null && isHaveCid.Count > 0)
                {
                    //xe.RemoveAll();
                    xn1.ParentNode.RemoveChild(xn1);
                    i--;
                }
            }
            return doc;
        }

        public static XmlDocument ClearXmlByCid(XmlDocument doc,string cid)
        {
            XmlNode xn = doc.SelectSingleNode("NewDataSet");
            XmlNodeList xnl = xn.ChildNodes;
            for (var i = 0;i< xnl.Count;i++)
            {
                XmlNode xn1 = xnl.Item(i);
                XmlElement xe = (XmlElement)xn1;
                var isHaveCid = xe.GetElementsByTagName("CID");
                if (isHaveCid != null && isHaveCid.Count > 0)
                {
                    if (isHaveCid.Item(0).InnerXml.ToLower() == cid.ToLower())
                    {
                        xn1.ParentNode.RemoveChild(xn1);
                        i--;
                    }
                }else
                {
                    xn1.ParentNode.RemoveChild(xn1);
                    i--;
                }
            }
            return doc;
        }

        #region 序列化与反序列化
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string s, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = System.IO.File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }

        #endregion

    }

    //========================================================= 

    //实例应用： 

    //string strXmlFile = Server.MapPath("TestXml.xml"); 
    //XmlControl xmlTool = new XmlControl(strXmlFile); 

    // 数据显视 
    // dgList.DataSource = xmlTool.GetData("Book/Authors[ISBN=\"0002\"]"); 
    // dgList.DataBind(); 

    // 更新元素内容 
    // xmlTool.Replace("Book/Authors[ISBN=\"0002\"]/Content","ppppppp"); 
    // xmlTool.Save(); 

    // 添加一个新节点 
    // xmlTool.InsertNode("Book","Author","ISBN","0004"); 
    // xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Content","aaaaaaaaa"); 
    // xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Title","Sex","man","iiiiiiii"); 
    // xmlTool.Save(); 

    // 删除一个指定节点的所有内容和属性 
    // xmlTool.Delete("Book/Author[ISBN=\"0004\"]"); 
    // xmlTool.Save(); 

    // 删除一个指定节点的子节点 
    // xmlTool.Delete("Book/Authors[ISBN=\"0003\"]"); 
    // xmlTool.Save();
}

public class ModelConvertHelper<T> where T : new()  // 此处一定要加上new()
{
    public static IList<T> ConvertToModel(DataTable dt)
    {

        IList<T> ts = new List<T>();// 定义集合
        Type type = typeof(T); // 获得此模型的类型
        string tempName = "";
        foreach (DataRow dr in dt.Rows)
        {
            T t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;
                if (dt.Columns.Contains(tempName))
                {
                    if (!pi.CanWrite) continue;
                    object value = dr[tempName];
                    if (value != DBNull.Value)
                        pi.SetValue(t, value, null);
                }
            }
            ts.Add(t);
        }
        return ts;
    }
}