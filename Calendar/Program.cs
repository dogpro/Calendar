using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Calendar
{
    internal class Program
    {
        private static List<char> list = new List<char>();

        static async Task Main()
        {
            APIClientService apiClient = new APIClientService();
            
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
                
                if (CheckAndGetDataFromApiIfExpired())
                {
                    var requestList = await apiClient.GetDataFromApiAsync(DateTime.Now.Year);
                    requestManager.InsertCalendar(DateTime.Now.Year, requestList);
                    
                    Console.WriteLine("Данные обновлены");
                }
                
                
                Console.WriteLine(requestManager.GetDayType(DateTime.Now));
            }

            Console.WriteLine("Нажмите любую кнопку для выхода");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Проверка что прошел год с момента последнего обновления
        /// </summary>
        /// <returns></returns>
        private static bool CheckAndGetDataFromApiIfExpired()
        {
            var lastDate = FileOpertions.LoadLastRequestTime();
            
            if ((DateTime.Now - lastDate).TotalDays >= 365)
            {
                Console.WriteLine("Данные обновляются");
                return true;
            }

            Console.WriteLine("Обновление не требуется.");
            return false;
        }
    }
}
