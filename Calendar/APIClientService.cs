using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Calendar;

public class APIClientService
{
    private readonly HttpClient _httpClient;

    public APIClientService()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Запрос к Апи для получения списка дней 
    /// </summary>
    /// <param name="year">Год, за который нужно получить список</param>
    /// <returns>Лист символов вида 0,1,2</returns>
    public async Task<List<char>> GetDataFromApiAsync(int year)
    {
        string apiUrl = $"https://isdayoff.ru/api/getdata?year={year}&pre=1";

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseFromServer = await response.Content.ReadAsStringAsync();
                
                FileOpertions.SaveLastRequestTime(DateTime.Now);
                
                return responseFromServer.ToList();
            }
            else
            {
                Console.WriteLine($"Ошибка при получении данных с API: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении данных с API: {ex.Message}");
            return null;
        }
    }
}