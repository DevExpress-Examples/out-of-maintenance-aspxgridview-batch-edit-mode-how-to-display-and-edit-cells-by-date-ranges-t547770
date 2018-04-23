using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BatchEditDateRanges {
    public static class ModelRepository {

        private static string SessionKey = "Data";
        public static List<SampleData> GetData() {
            if (HttpContext.Current.Session[SessionKey] == null) {
                HttpContext.Current.Session[SessionKey] = Enumerable.Range(0, 10).Select(i => new SampleData {
                    ID = i,
                    Text = "Text" + i,
                    AmountDateMap = "{}"
                }).ToList<SampleData>();
            }

            return (List<SampleData>)HttpContext.Current.Session[SessionKey];
        }
    }
    public class SampleData {
        public int ID { get; set; }
        public string Text { get; set; }
        public string AmountDateMap { get; set; }
    }

    public class DateFieldParts {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        private DateFieldParts(string dateFieldName) {
            if (IsDateFieldParts(dateFieldName)) {
                string[] dateFieldPartsArray = dateFieldName.Split('_');
                Year = Convert.ToInt32(dateFieldPartsArray[0]);
                Month = Convert.ToInt32(dateFieldPartsArray[1]);
                Day = Convert.ToInt32(dateFieldPartsArray[2]);
            }
        }

        public static DateFieldParts GetDateFieldParts(string dateFieldName) {
            DateFieldParts dateFieldParts = null;
            if (IsDateFieldParts(dateFieldName)) {
                dateFieldParts = new DateFieldParts(dateFieldName);
            }

            return dateFieldParts;
        }

        public static bool IsDateFieldParts(string dateFieldName) {
            string[] dateFieldPartsArray = dateFieldName.Split('_');
            return dateFieldPartsArray != null && dateFieldPartsArray.Length == 3;
        }
    }

    public class DateAmountMap {
        public Dictionary<int, MonthMap> Years { get; set; }

        public double GetDateAmount(DateFieldParts dateFieldParts) {
            double amount = 0;
            MonthMap monthMap;
            DayMap dayMap;

            if (GetMonthMapFromYear(dateFieldParts.Year, out monthMap)) {
                if (monthMap.GetDayMapFromMonth(dateFieldParts.Month, out dayMap)) {
                    dayMap.GetAmountFromDay(dateFieldParts.Day, out amount);
                }
            }
            return amount;
        }

        public void SetDateAmount(DateFieldParts dateFieldParts, double amount) {
            MonthMap monthMap;
            DayMap dayMap;

            if (GetMonthMapFromYear(dateFieldParts.Year, out monthMap)) {
                if (monthMap.GetDayMapFromMonth(dateFieldParts.Month, out dayMap)) {
                    dayMap.SetAmountForDay(dateFieldParts.Day, amount);
                }
            }
        }

        public bool GetMonthMapFromYear(int year, out MonthMap monthMap) {
            if (Years == null)
                CreateYear(year);
            if (!Years.ContainsKey(year))
                Years.Add(year, new MonthMap());

            return Years.TryGetValue(year, out monthMap);
        }

        private void CreateYear(int year) {
            Years = new Dictionary<int, MonthMap>();
        }
    }

    public class MonthMap {
        public Dictionary<int, DayMap> Months { get; set; }

        public bool GetDayMapFromMonth(int month, out DayMap dayMap) {
            if (Months == null)
                CreateMonth(month);
            if (!Months.ContainsKey(month))
                Months.Add(month, new DayMap());
            return Months.TryGetValue(month, out dayMap);
        }

        private void CreateMonth(int month) {
            Months = new Dictionary<int, DayMap>();
        }
    }

    public class DayMap {
        public Dictionary<int, double> Days { get; set; }

        public bool GetAmountFromDay(int day, out double amount) {
            if (Days == null)
                CreateDay(day);
            if (!Days.ContainsKey(day))
                Days.Add(day, 0);
            return Days.TryGetValue(day, out amount);
        }

        public void SetAmountForDay(int day, double amount) {
            if (Days == null)
                CreateDay(day);
            if (!Days.ContainsKey(day))
                Days.Add(day, 0);

            Days[day] = amount;
        }

        private void CreateDay(int day) {
            Days = new Dictionary<int, double>();
        }
    }
}