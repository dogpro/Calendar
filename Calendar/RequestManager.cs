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

    /// <summary>
    /// Получение типа дня по дате из БД
    /// </summary>
    /// <param name="date">Дата</param>
    /// <returns>Тип дня</returns>
    public string GetDayType(DateTime date)
    {
        string sql = $"SELECT DayTypes.TypeName FROM Calendars  " +
                     $"JOIN DayTypes ON Calendars.DayTypeId = DayTypes.Id " +
                     $"WHERE Calendars.Date = '{date:yyyy-MM-dd}'";
        try
        {
            return _dbContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "не найдена";
        }
    }
    
    /// <summary>
    /// Вставка данных в таблицу за указанный год
    /// </summary>
    /// <param name="year">год</param>
    /// <param name="dayTypes">Лист типов дней</param>
    public void InsertCalendar(int year, List<char> dayTypes)
    {
        _dbContext.Database.ExecuteSqlCommand("DELETE FROM Calendars");
        
        int daysInYear = new DateTime(year, 12, 31).DayOfYear;
        for (int day = 1; day <= daysInYear; day++)
        {
            DateTime currentDate = new DateTime(year, 1, 1).AddDays(day - 1);
            string date = currentDate.ToString("yyyy-MM-dd");
            string dayType = dayTypes[day - 1].ToString();

            string sql = "INSERT INTO Calendars (Id, Date, DayTypeId) VALUES ({0}, {1}, {2})";
            _dbContext.Database.ExecuteSqlCommand(sql, day, date, dayType);
        }
        
        _dbContext.SaveChanges();
    }
    
    /// <summary>
    /// Проверка подключения к БД
    /// </summary>
    /// <returns>true или false в зависисмости от результата</returns>
    public bool IsDatabaseConnected()
    {
        try
        {
            _dbContext.Database.Connection.Open();
            _dbContext.Database.Connection.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
            return false;
        }
    }
}