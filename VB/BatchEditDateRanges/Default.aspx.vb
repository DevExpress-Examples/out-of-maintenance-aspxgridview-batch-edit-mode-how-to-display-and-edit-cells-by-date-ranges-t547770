Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace BatchEditDateRanges
    Partial Public Class [Default]
        Inherits System.Web.UI.Page

        Protected Sub SubmitButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect(String.Format("EditRange.aspx?StartDate={0}&EndDate={1}", StartDate.Date, EndDate.Date))
        End Sub
    End Class
End Namespace