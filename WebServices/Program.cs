using System;
using System.Net;
using System.Text;
using Calendar;

namespace WebServices
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Сервер запущен. Для выхода нажмите Enter.");

            int port = 8080;

            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add($"http://localhost:{port}/");
                listener.Start();

                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;

                    HandleRequest(request, response);
                }
            }
        }

        /// <summary>
        /// Обработка HTTP-запроса
        /// </summary>
        /// <param name="request">HTTP-запрос</param>
        /// <param name="response">HTTP-ответ</param>
        static void HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            using (System.IO.Stream body = request.InputStream)
            using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
            {
                string data = reader.ReadToEnd();
                string responseString = "";
                string dayType = "";

                if (!string.IsNullOrEmpty(data))
                {
                    if (DateTime.TryParse(data, out DateTime parsedDate))
                    {

                        UserContext dbContext = new UserContext();

                        var requestManager = new RequestManager(dbContext);
                        dayType = requestManager.GetDayType(parsedDate);

                        Console.WriteLine($"{dayType}");
                        responseString = $"Введенная дата {parsedDate} - {dayType}";
                    }
                    else
                    {
                        Console.WriteLine("Некорректная дата");
                        responseString = "Invalid date";
                    }
                }

                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }

            response.Close();
        }
    }
}