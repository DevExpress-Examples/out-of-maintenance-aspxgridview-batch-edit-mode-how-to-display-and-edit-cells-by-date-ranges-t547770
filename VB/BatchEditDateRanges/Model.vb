Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace BatchEditDateRanges
    Public NotInheritable Class ModelRepository

        Private Sub New()
        End Sub


        Private Shared SessionKey As String = "Data"
        Public Shared Function GetData() As List(Of SampleData)
            If HttpContext.Current.Session(SessionKey) Is Nothing Then
                HttpContext.Current.Session(SessionKey) = Enumerable.Range(0, 10).Select(Function(i) New SampleData With { _
                    .ID = i, _
                    .Text = "Text" & i, _
                    .AmountDateMap = "{}" _
                }).ToList()
            End If

            Return DirectCast(HttpContext.Current.Session(SessionKey), List(Of SampleData))
        End Function
    End Class
    Public Class SampleData
        Public Property ID() As Integer
        Public Property Text() As String
        Public Property AmountDateMap() As String
    End Class

    Public Class DateFieldParts
        Public Property Year() As Integer
        Public Property Month() As Integer
        Public Property Day() As Integer

        Private Sub New(ByVal dateFieldName As String)
            If IsDateFieldParts(dateFieldName) Then
                Dim dateFieldPartsArray() As String = dateFieldName.Split("_"c)
                Year = Convert.ToInt32(dateFieldPartsArray(0))
                Month = Convert.ToInt32(dateFieldPartsArray(1))
                Day = Convert.ToInt32(dateFieldPartsArray(2))
            End If
        End Sub

        Public Shared Function GetDateFieldParts(ByVal dateFieldName As String) As DateFieldParts

            Dim dateFieldParts_Renamed As DateFieldParts = Nothing
            If IsDateFieldParts(dateFieldName) Then
                dateFieldParts_Renamed = New DateFieldParts(dateFieldName)
            End If

            Return dateFieldParts_Renamed
        End Function

        Public Shared Function IsDateFieldParts(ByVal dateFieldName As String) As Boolean
            Dim dateFieldPartsArray() As String = dateFieldName.Split("_"c)
            Return dateFieldPartsArray IsNot Nothing AndAlso dateFieldPartsArray.Length = 3
        End Function
    End Class

    Public Class DateAmountMap
        Public Property Years() As Dictionary(Of Integer, MonthMap)


        Public Function GetDateAmount(ByVal dateFieldParts_Renamed As DateFieldParts) As Double
            Dim amount As Double = 0
            Dim monthMap As MonthMap = Nothing
            Dim dayMap As DayMap = Nothing

            If GetMonthMapFromYear(dateFieldParts_Renamed.Year, monthMap) Then
                If monthMap.GetDayMapFromMonth(dateFieldParts_Renamed.Month, dayMap) Then
                    dayMap.GetAmountFromDay(dateFieldParts_Renamed.Day, amount)
                End If
            End If
            Return amount
        End Function


        Public Sub SetDateAmount(ByVal dateFieldParts_Renamed As DateFieldParts, ByVal amount As Double)
            Dim monthMap As MonthMap = Nothing
            Dim dayMap As DayMap = Nothing

            If GetMonthMapFromYear(dateFieldParts_Renamed.Year, monthMap) Then
                If monthMap.GetDayMapFromMonth(dateFieldParts_Renamed.Month, dayMap) Then
                    dayMap.SetAmountForDay(dateFieldParts_Renamed.Day, amount)
                End If
            End If
        End Sub

        Public Function GetMonthMapFromYear(ByVal year As Integer, <System.Runtime.InteropServices.Out()> ByRef monthMap As MonthMap) As Boolean
            If Years Is Nothing Then
                CreateYear(year)
            End If
            If Not Years.ContainsKey(year) Then
                Years.Add(year, New MonthMap())
            End If

            Return Years.TryGetValue(year, monthMap)
        End Function

        Private Sub CreateYear(ByVal year As Integer)
            Years = New Dictionary(Of Integer, MonthMap)()
        End Sub
    End Class

    Public Class MonthMap
        Public Property Months() As Dictionary(Of Integer, DayMap)

        Public Function GetDayMapFromMonth(ByVal month As Integer, <System.Runtime.InteropServices.Out()> ByRef dayMap As DayMap) As Boolean
            If Months Is Nothing Then
                CreateMonth(month)
            End If
            If Not Months.ContainsKey(month) Then
                Months.Add(month, New DayMap())
            End If
            Return Months.TryGetValue(month, dayMap)
        End Function

        Private Sub CreateMonth(ByVal month As Integer)
            Months = New Dictionary(Of Integer, DayMap)()
        End Sub
    End Class

    Public Class DayMap
        Public Property Days() As Dictionary(Of Integer, Double)

        Public Function GetAmountFromDay(ByVal day As Integer, <System.Runtime.InteropServices.Out()> ByRef amount As Double) As Boolean
            If Days Is Nothing Then
                CreateDay(day)
            End If
            If Not Days.ContainsKey(day) Then
                Days.Add(day, 0)
            End If
            Return Days.TryGetValue(day, amount)
        End Function

        Public Sub SetAmountForDay(ByVal day As Integer, ByVal amount As Double)
            If Days Is Nothing Then
                CreateDay(day)
            End If
            If Not Days.ContainsKey(day) Then
                Days.Add(day, 0)
            End If

            Days(day) = amount
        End Sub

        Private Sub CreateDay(ByVal day As Integer)
            Days = New Dictionary(Of Integer, Double)()
        End Sub
    End Class
End Namespace