using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Calendar
{
    internal class Program
    {
        private static List<char> list = new List<char>();

        static void Main()
        {
            list = GetDataFromApi(2024);
            if (list == null)
            {
                Console.ReadKey();
                return;
            }

            Console.WriteLine(list.Count);

            using (var dbContext = new UserContext())
            {
                var requestManager = new RequestManager(dbContext);

                if (requestManager.IsDatabaseConnected())
                {
                    Console.WriteLine("Подключение к базе данных: ОК.\n");
                }
                else
                {
                    Console.WriteLine("Подключение к базе данных: не подключено.");
                    Console.ReadKey();
                    return;
                }

                requestManager.UpdateOrInsertCalendar(2023, list);
                
                while (true)
                {
                    Console.WriteLine("Меню:");
                    Console.WriteLine("1. Определить типа дня");
                    Console.WriteLine("0. Выход");

                    Console.Write("\nВвод: ");
                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            while (true)
                            {
                                Console.Write("Введите день в формате год.месяц.день: ");
                                var inputDate = Console.ReadLine();

                                if (DateTime.TryParse(inputDate, out var date))
                                {

                                    Console.WriteLine($"{inputDate} - : {requestManager.GetDayType(date)}\n");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Введенное значение не является корректной датой");
                                }
                            }

                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Неверный ввод. Попробуйте снова.\n");
                            break;
                    }
                }
            }
        }

        private static List<char> GetDataFromApi(int year)
        {
            string apiUrl = $"https://isdayoff.ru/api/getdata?year={year}&pre=1";

            try
            {
                var request = WebRequest.Create(apiUrl);
                request.Method = "GET";

                using (var response = request.GetResponse())
                using (var dataStream = response.GetResponseStream())
                using (var reader = new StreamReader(dataStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    return responseFromServer.ToList();
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Ошибка при получении данных с API: {ex.Message}");
                return null;
            }
        }
    }
}
