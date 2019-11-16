using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForms
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageLoadCalledCount"] = Convert.ToInt32(Session["PageLoadCalledCount"] ?? 0) + 1;
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            lblTest.Text = Session["PageLoadCalledCount"].ToString();
        }
    }
}