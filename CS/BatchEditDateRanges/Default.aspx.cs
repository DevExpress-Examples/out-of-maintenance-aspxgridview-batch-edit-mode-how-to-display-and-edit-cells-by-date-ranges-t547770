using DevExpress.Web;
using DevExpress.Web.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BatchEditDateRanges {
    public partial class Default : System.Web.UI.Page {
        protected void SubmitButton_Click(object sender, EventArgs e) {
            Response.Redirect(string.Format("EditRange.aspx?StartDate={0}&EndDate={1}", StartDate.Date, EndDate.Date));
        }
    }
}