using System;
using System.Collections.Generic;
using System.Linq;

namespace Calendar;

public class RequestManager : IRequestManager
{
    private UserContext _dbContext;

    public RequestManager(UserContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DayType GetDayType(DateTime date)
    {
        string sql = "SELECT DayType " +
                     "FROM Calendars " +
                     $"WHERE Date = '{date:yyyy-MM-dd}'";
        try
        {
            var dayData = _dbContext.Database.SqlQuery<string>(sql).FirstOrDefault();

            Console.WriteLine(dayData);
            
            if (dayData != null)
            {
                if (Enum.TryParse<DayType>(dayData.ToString(), out DayType result))
                {
                    return result;
                }
                else
                {
                    return DayType.Рабочий;
                }
            }
            else
            {
                return DayType.Ошибка; 
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return DayType.Ошибка;
        }
    }

    public void UpdateOrInsertCalendar(int year, List<char> dayTypes)
    {
        try
        {
            if (!HasDataForYear(year))
            {
                Console.WriteLine("Обновление календаря...");
                InsertCalendar(year, dayTypes);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении или вставке данных календаря: {ex}");
        }
    }

    private bool HasDataForYear(int year)
    {
        if (_dbContext.Calendar.Any())
        {
            return _dbContext.Calendar.Any(d => d.Date.Contains(year.ToString()));
        }

        return false;
    }

    private void InsertCalendar(int year, List<char> dayTypes)
    {
        _dbContext.Database.ExecuteSqlCommand("DELETE FROM Calendars");
        
        int daysInYear = new DateTime(year, 12, 31).DayOfYear;
        for (int day = 1; day <= daysInYear; day++)
        {
            DateTime currentDate = new DateTime(year, 1, 1).AddDays(day - 1);
            string date = currentDate.ToString("yyyy-MM-dd");
            string dayOfWeek = currentDate.DayOfWeek.ToString();
            string dayType = dayTypes[day - 1].ToString();

            string sql = "INSERT INTO Calendars (Id, Date, DayOfWeek, DayType) VALUES ({0}, {1}, {2}, {3})";
            _dbContext.Database.ExecuteSqlCommand(sql, day, date, dayOfWeek, dayType);
        }
        
        _dbContext.SaveChanges();
    }
    
    public bool IsDatabaseConnected()
    {
        try
        {
            _dbContext.Database.Connection.Open();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
            return false;
        }
    }
}