using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Titan.WebMVC
{
    public partial class ErrorAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string messageStr = Application["error"].ToString();
                ErrorHtml.Text = messageStr;
                //IPluralizationService
                //System.Data.Entity.Infrastructure.EdmxWriter w = new System.Data.Entity.Infrastructure.EdmxWriter();
            }
        }
    }
}