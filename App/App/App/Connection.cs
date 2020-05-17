using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace App
{
    public class Connection : IDisposable
    {
        public const string BaseAddress = "https://match-my-dog.herokuapp.com/api/";

        public HttpClient Client = new HttpClient();
        public readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };
        public string Token
        {
            get => Client.DefaultRequestHeaders.Authorization.Parameter;
            set => Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
        }

        public Connection()
        {
            Client.BaseAddress = new Uri(BaseAddress);
        }

        public Connection(Uri baseAddress)
        {
            Client.BaseAddress = baseAddress;
        }
        /// <summary>
        /// Метод отправляет запрос типа Post серверу 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> Post<T>(string endpoint, object data = null)
        {
            var a = JsonSerializer.Serialize(data, options);
            var content = new StringContent(data == null ? string.Empty : JsonSerializer.Serialize(data, options), Encoding.UTF8, "application/json");

            var message = await Client.PostAsync(endpoint, content);

            return await HandleResponse<T>(message);
        }
        /// <summary>
        /// Метод отправляет запрос типа Put серверу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> Put<T>(string endpoint, object data = null)
        {
            var content = new StringContent(data == null ? string.Empty : JsonSerializer.Serialize(data, options), Encoding.UTF8, "application/json");

            var message = await Client.PutAsync(endpoint, content);

            return await HandleResponse<T>(message);
        }
        /// <summary>
        /// Метод отправляет запрос типа Get серверу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string endpoint)
        {
            var message = await Client.GetAsync(endpoint);

            return await HandleResponse<T>(message);
        }
        /// <summary>
        /// Метод отправляет запрос типа Delete серверу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<T> Delete<T>(string endpoint)
        {
            var message = await Client.DeleteAsync(endpoint);

            return await HandleResponse<T>(message);
        }
        /// <summary>
        /// Метод десериализует полученный ответ от сервера
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<T> HandleResponse<T>(HttpResponseMessage message)
        {
            if (message.StatusCode != HttpStatusCode.OK)
            {

                throw new ConnectionException(message);
            }

            var response = await message.Content.ReadAsStringAsync();
            return response == string.Empty ? default : JsonSerializer.Deserialize<T>(response, options);
        }

        public void Dispose() => Client.Dispose();
    }

    public class ConnectionException : Exception
    {
        public HttpResponseMessage ResponseMessage { get; }
        public HttpStatusCode StatusCode => ResponseMessage.StatusCode;
        public string ReasonPhrase => ResponseMessage.ReasonPhrase;
        public string ErrorMessage { get; set; }
        public ConnectionException(HttpResponseMessage message) : base(message.ReasonPhrase)
        {
            ResponseMessage = message;
        }

    }
}
