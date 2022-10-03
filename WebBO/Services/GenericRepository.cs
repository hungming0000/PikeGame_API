using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebBO.Interface;

namespace WebBO.Services
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private static readonly HttpClient HttpClient;

        static GenericRepository()
        {
            HttpClient = new HttpClient();
        }

        public async Task<string> CallCpApi(string apiName, T instance)
        {
            string responseBody = string.Empty;

            var authenticationBytes = Encoding.ASCII.GetBytes("9561845e-03cb-4f69-a819-6c774bc52967:1qazxsw2");

            HttpClient.DefaultRequestHeaders.Accept.Clear();

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                   Convert.ToBase64String(authenticationBytes));

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = await HttpClient.PostAsJsonAsync("https://api.land.moi.gov.tw/cp/api/" + apiName, instance);

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }

                return responseBody;
            }
            catch (Exception ex)
            {
                throw new Exception("錯誤代碼：" + apiName + ex);
            }
        }

        public async Task<string> CallOpApi(string apiName, T instance)
        {
            string responseBody = string.Empty;

            HttpClient.DefaultRequestHeaders.Accept.Clear();

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = await HttpClient.PostAsJsonAsync("https://openapi.land.moi.gov.tw/WEBAPI/" + apiName, instance);

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }

                return responseBody;
            }
            catch (Exception ex)
            {
                throw new Exception("錯誤代碼：" + apiName + ex);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
