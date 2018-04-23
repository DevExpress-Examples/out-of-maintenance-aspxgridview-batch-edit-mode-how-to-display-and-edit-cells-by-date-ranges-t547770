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

namespace BatchEditDateRanges
{
    public partial class EditRange : System.Web.UI.Page
    {
        private DateTime startDate;
        private DateTime endDate;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (DateTime.TryParse(Request.Params["StartDate"], out startDate)
                && DateTime.TryParse(Request.Params["EndDate"], out endDate))
            {
                PageHeader.InnerText = string.Format("Edit data on range from {0} to {1}", startDate.ToShortDateString(), endDate.ToShortDateString());
                Grid.DataBind();
            }
            else
            {
                Grid.Visible = false;
                CreateMissingQueryParametersWarning();
            }
        }

        protected void Grid_DataBinding(object sender, EventArgs e)
        {
            Grid.DataSource = ModelRepository.GetData();
            AddColumns();
        }

        protected void Grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            if (e.UpdateValues.Count > 0)
            {
                ProcessUpdateValues(e.UpdateValues);
            }

            e.Handled = true;
        }

        protected void Grid_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
        {
            DateFieldParts dateFieldParts = DateFieldParts.GetDateFieldParts(e.Column.FieldName);
            if (dateFieldParts != null)
            {
                DateAmountMap dateAmountMap = JsonConvert.DeserializeObject<DateAmountMap>(Convert.ToString(e.GetListSourceFieldValue("AmountDateMap")));
                e.Value = dateAmountMap.GetDateAmount(dateFieldParts);
            }
        }

        private void AddColumns()
        {
            Grid.Columns.Clear();
            Grid.Columns.Add(new GridViewDataTextColumn { FieldName = "ID", ReadOnly = true });
            Grid.Columns.Add(new GridViewDataTextColumn { FieldName = "Text" });

            CreateDateColumns(startDate, endDate);
            Grid.KeyFieldName = "ID";
        }

        private void CreateDateColumns(DateTime startDate, DateTime endDate)
        {
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                var yearBand = FindYearBandColumn(Grid.Columns, currentDate);
                var monthBand = FindMonthBandColumn(yearBand.Columns, currentDate);

                GridViewDataSpinEditColumn dayColumn = new GridViewDataSpinEditColumn()
                {
                    FieldName = string.Format("{0}_{1}_{2}", currentDate.Year, currentDate.Month, currentDate.Day),
                    Caption = currentDate.Day.ToString(),
                    UnboundType = DevExpress.Data.UnboundColumnType.Decimal,
                    Width = Unit.Pixel(50)
                };
                monthBand.Columns.Add(dayColumn);

                currentDate = currentDate.AddDays(1);
            }
        }

        private GridViewBandColumn FindMonthBandColumn(GridViewColumnCollection yearBandColumns, DateTime currentDate)
        {
            return FindBandColumn(yearBandColumns, currentDate, currentDate.ToString("MMMM", CultureInfo.InvariantCulture));
        }

        private GridViewBandColumn FindYearBandColumn(GridViewColumnCollection gridColumns, DateTime currentDate)
        {
            return FindBandColumn(gridColumns, currentDate, currentDate.Year.ToString());
        }

        private GridViewBandColumn FindBandColumn(GridViewColumnCollection columns, DateTime currentDate, string caption)
        {
            GridViewBandColumn band = columns[caption] as GridViewBandColumn;
            if (band == null)
            {
                band = new GridViewBandColumn() { Caption = caption };
                columns.Add(band);
            }

            return band;
        }

        private void ProcessUpdateValues(List<ASPxDataUpdateValues> updateValues)
        {
            foreach (ASPxDataUpdateValues updateInfo in updateValues)
            {

                SampleData item = ModelRepository.GetData().Find(i => i.ID == Convert.ToInt32(updateInfo.Keys[0]));
                item.Text = Convert.ToString(updateInfo.NewValues["Text"]);

                DateAmountMap dateAmountMap = JsonConvert.DeserializeObject<DateAmountMap>(item.AmountDateMap);

                foreach (string fieldName in updateInfo.NewValues.Keys)
                {
                    DateFieldParts dateFieldParts = DateFieldParts.GetDateFieldParts(fieldName);
                    if (dateFieldParts != null)
                    {
                        dateAmountMap.SetDateAmount(dateFieldParts, Convert.ToDouble(updateInfo.NewValues[fieldName]));
                    }
                }

                item.AmountDateMap = JsonConvert.SerializeObject(dateAmountMap);
            }
        }

        private void CreateMissingQueryParametersWarning()
        {
            ASPxLabel warningLabel = new ASPxLabel();
            warningLabel.EncodeHtml = false;
            warningLabel.ID = "WarningLabel";
            warningLabel.Text = "Missing query parameters.<br/> Please return to the following page and enter the required values again";
            PageContainer.Controls.Add(warningLabel);

            LiteralControl spaceBreak = new LiteralControl("<br />");
            PageContainer.Controls.Add(spaceBreak);

            ASPxHyperLink warningHyperLink = new ASPxHyperLink();
            warningHyperLink.ID = "WarningHyperLink";
            warningHyperLink.NavigateUrl = "/";
            warningHyperLink.Text = "Return";
            PageContainer.Controls.Add(warningHyperLink);
        }
    }
}