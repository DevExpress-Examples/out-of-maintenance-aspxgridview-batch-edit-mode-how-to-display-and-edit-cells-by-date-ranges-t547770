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
    Partial Public Class EditRange
        Inherits System.Web.UI.Page

        Private startDate As Date
        Private endDate As Date

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            If Date.TryParse(Request.Params("StartDate"), startDate) AndAlso Date.TryParse(Request.Params("EndDate"), endDate) Then
                PageHeader.InnerText = String.Format("Edit data on range from {0} to {1}", startDate.ToShortDateString(), endDate.ToShortDateString())
                Grid.DataBind()
            Else
                Grid.Visible = False
                CreateMissingQueryParametersWarning()
            End If
        End Sub

        Protected Sub Grid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Grid.DataSource = ModelRepository.GetData()
            AddColumns()
        End Sub

        Protected Sub Grid_BatchUpdate(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs)
            If e.UpdateValues.Count > 0 Then
                ProcessUpdateValues(e.UpdateValues)
            End If

            e.Handled = True
        End Sub

        Protected Sub Grid_CustomUnboundColumnData(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewColumnDataEventArgs)

            Dim dateFieldParts_Renamed As DateFieldParts = BatchEditDateRanges.DateFieldParts.GetDateFieldParts(e.Column.FieldName)
            If dateFieldParts_Renamed IsNot Nothing Then
                Dim dateAmountMap As DateAmountMap = JsonConvert.DeserializeObject(Of DateAmountMap)(Convert.ToString(e.GetListSourceFieldValue("AmountDateMap")))
                e.Value = dateAmountMap.GetDateAmount(dateFieldParts_Renamed)
            End If
        End Sub

        Private Sub AddColumns()
            Grid.Columns.Clear()
            Grid.Columns.Add(New GridViewDataTextColumn With { _
                .FieldName = "ID", _
                .ReadOnly = True _
            })
            Grid.Columns.Add(New GridViewDataTextColumn With {.FieldName = "Text"})

            CreateDateColumns(startDate, endDate)
            Grid.KeyFieldName = "ID"
        End Sub

        Private Sub CreateDateColumns(ByVal startDate As Date, ByVal endDate As Date)
            Dim currentDate As Date = startDate

            Do While currentDate <= endDate
                Dim yearBand = FindYearBandColumn(Grid.Columns, currentDate)
                Dim monthBand = FindMonthBandColumn(yearBand.Columns, currentDate)

                Dim dayColumn As New GridViewDataSpinEditColumn() With { _
                    .FieldName = String.Format("{0}_{1}_{2}", currentDate.Year, currentDate.Month, currentDate.Day), _
                    .Caption = currentDate.Day.ToString(), _
                    .UnboundType = DevExpress.Data.UnboundColumnType.Decimal, _
                    .Width = Unit.Pixel(50) _
                }
                monthBand.Columns.Add(dayColumn)

                currentDate = currentDate.AddDays(1)
            Loop
        End Sub

        Private Function FindMonthBandColumn(ByVal yearBandColumns As GridViewColumnCollection, ByVal currentDate As Date) As GridViewBandColumn
            Return FindBandColumn(yearBandColumns, currentDate, currentDate.ToString("MMMM", CultureInfo.InvariantCulture))
        End Function

        Private Function FindYearBandColumn(ByVal gridColumns As GridViewColumnCollection, ByVal currentDate As Date) As GridViewBandColumn
            Return FindBandColumn(gridColumns, currentDate, currentDate.Year.ToString())
        End Function

        Private Function FindBandColumn(ByVal columns As GridViewColumnCollection, ByVal currentDate As Date, ByVal caption As String) As GridViewBandColumn
            Dim band As GridViewBandColumn = TryCast(columns(caption), GridViewBandColumn)
            If band Is Nothing Then
                band = New GridViewBandColumn() With {.Caption = caption}
                columns.Add(band)
            End If

            Return band
        End Function

        Private Sub ProcessUpdateValues(ByVal updateValues As List(Of ASPxDataUpdateValues))
            For Each updateInfo As ASPxDataUpdateValues In updateValues

                Dim item As SampleData = ModelRepository.GetData().Find(Function(i) i.ID = Convert.ToInt32(updateInfo.Keys(0)))
                item.Text = Convert.ToString(updateInfo.NewValues("Text"))

                Dim dateAmountMap As DateAmountMap = JsonConvert.DeserializeObject(Of DateAmountMap)(item.AmountDateMap)

                For Each fieldName As String In updateInfo.NewValues.Keys

                    Dim dateFieldParts_Renamed As DateFieldParts = BatchEditDateRanges.DateFieldParts.GetDateFieldParts(fieldName)
                    If dateFieldParts_Renamed IsNot Nothing Then
                        dateAmountMap.SetDateAmount(dateFieldParts_Renamed, Convert.ToDouble(updateInfo.NewValues(fieldName)))
                    End If
                Next fieldName

                item.AmountDateMap = JsonConvert.SerializeObject(dateAmountMap)
            Next updateInfo
        End Sub

        Private Sub CreateMissingQueryParametersWarning()
            Dim warningLabel As New ASPxLabel()
            warningLabel.EncodeHtml = False
            warningLabel.ID = "WarningLabel"
            warningLabel.Text = "Missing query parameters.<br/> Please return to the following page and enter the required values again"
            PageContainer.Controls.Add(warningLabel)

            Dim spaceBreak As New LiteralControl("<br />")
            PageContainer.Controls.Add(spaceBreak)

            Dim warningHyperLink As New ASPxHyperLink()
            warningHyperLink.ID = "WarningHyperLink"
            warningHyperLink.NavigateUrl = "/"
            warningHyperLink.Text = "Return"
            PageContainer.Controls.Add(warningHyperLink)
        End Sub
    End Class
End Namespace