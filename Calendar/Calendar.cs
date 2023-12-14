using System;

namespace Calendar
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string DayOfWeek { get; set; }
        public string DayType { get; set; }
    }
    
    public enum DayType
    {
        Рабочий = 0,
        Выходной = 1,
        Сокращенный = 2,
        Ошибка = -1
    }
}