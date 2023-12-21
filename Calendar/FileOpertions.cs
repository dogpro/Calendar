using System;
using System.IO;
using Newtonsoft.Json;

namespace Calendar;

public class FileOpertions
{
    
    private const string ConfigPath = "config.json";
    
    /// <summary>
    /// Метод загрузки даты последнего апдейта
    /// </summary>
    /// <returns>дата</returns>
    public static DateTime LoadLastRequestTime()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                string json = File.ReadAllText(ConfigPath);
                return JsonConvert.DeserializeObject<DateTime>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке времени запроса из файла: {ex.Message}");
        }

        return DateTime.MinValue;
    }

    /// <summary>
    /// Метод сохранения даты последнего апдейта
    /// </summary>
    /// <param name="_lastRequestTime">Дата</param>
    public static void SaveLastRequestTime(DateTime _lastRequestTime)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_lastRequestTime);
            File.WriteAllText(ConfigPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении времени запроса в файл: {ex.Message}");
        }
    }
}