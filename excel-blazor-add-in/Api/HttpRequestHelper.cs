using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace Benteler.WorkPlan.Web.Api
{
    public class HttpRequestHelper
    {


        private string _baseUrl;
        public HttpRequestHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<RequestResult<dynamic>> HttpGetFile(string url, string fileName)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? response = null;
                try
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }

                    Console.WriteLine($"File downloaded to: {fileName}");
                    return new RequestResult<dynamic>(true, response, fileName, null);
                }
                catch (Exception e)
                {
                    return new RequestResult<dynamic>(false, response, null, e);
                }
            }
        }
        public async Task<RequestResult<T>> HttpGetJsonObject<T>(string url)
        {
            RequestResult<dynamic> response = await HttpGet(url, null);
            if (response.IsSuccess())
            {
                string jsonData = await response.ResponseMessage!.Content.ReadAsStringAsync();
                return new RequestResult<T>(true, response.ResponseMessage, JsonConvert.DeserializeObject<T>(jsonData), null);
            }
            return new RequestResult<T>(false, response.ResponseMessage, default(T), response.Error);
        }
        public async Task<RequestResult<T>> HttpGetJsonObject<T>(string url, HttpContent content)
        {
            RequestResult<dynamic> response = await HttpGet(url, content);
            if (response.IsSuccess())
            {
                string jsonData = await response.ResponseMessage!.Content.ReadAsStringAsync();
                return new RequestResult<T>(true, response.ResponseMessage, JsonConvert.DeserializeObject<T>(jsonData), null);
            }
            return new RequestResult<T>(false, response.ResponseMessage, default(T), response.Error);
        }
        /// <summary>
        /// Sends a request to _baseUrl + url and returns the string returned by that method.
        /// </summary>
        /// <param name="url">The endpoint</param>
        /// <returns>The returned string from the api or the </returns>
        public async Task<RequestResult<string>> HttpGetString(string url)
        {
            RequestResult<dynamic> response = await HttpGet(url, null);
            if (response.IsSuccess())
            {
                string data = await response.ResponseMessage!.Content.ReadAsStringAsync();
                return new RequestResult<string>(true, response.ResponseMessage, data, null);
            }
            return new RequestResult<string>(false, response.ResponseMessage, string.Empty, response.Error);
        }
        /// <summary>
        /// Sends a request to _baseUrl + url and returns the http response.
        /// </summary>
        /// <param name="url">The Api url</param>
        /// <returns>The result of the Get request.</returns>
        public async Task<RequestResult<dynamic>> HttpGet(string url, HttpContent? content)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                HttpResponseMessage? response = null;
                try
                {
                    Console.Write($"Performing GET request: {_baseUrl + url}");
                    HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url)
                    {
                        Content = content 
                    };
                    response = await client.SendAsync(httpRequest);

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine($" GET Response Status: {response.StatusCode} ");
                    return new RequestResult<dynamic>(true, response, null, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($" GET Request error: {e.Message}");
                    return new RequestResult<dynamic>(false, response, null, e);
                }
            }
        }
        public async Task<RequestResult<dynamic>> HttpPatch(string url, HttpContent? content)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? response = null;
                client.BaseAddress = new Uri(_baseUrl);
                try
                {
                    Console.Write($"Performing Patch request: {_baseUrl + url}");
                    response = await client.PatchAsync(url, content);

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine($" Patch Response Status: {response.StatusCode} ");
                    return new RequestResult<dynamic>(true, response, null, null);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($" Patch Request error: {e.Message}");
                    return new RequestResult<dynamic>(false, response, null, e);
                }
            }
        }
        public async Task<RequestResult<dynamic>> HttpPost(string url, HttpContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                HttpResponseMessage? response = null;
                try
                {
                    Console.Write($"Performing Post request: {_baseUrl + url}");
                    response = await client.PostAsync(url, content);
                    

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine($" Patch Response Status: {response.StatusCode} ");
                    return new RequestResult<dynamic>(true, response, null, null);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($" Patch Request error: {e.Message}");
                    return new RequestResult<dynamic>(false, response, null, e);
                }
            }
        }
        public async Task<RequestResult<dynamic>> HttpPostJson(string url, object content)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                HttpResponseMessage? response = null;
                try
                {
                    Console.Write($"Performing Post request: {_baseUrl + url}");
                    string json = JsonConvert.SerializeObject(content);
                    StringContent jsonStringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await client.PostAsync("/register", jsonStringContent);

                    //response = await client.PostAsJsonAsync(url, content);


                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine($" Patch Response Status: {response.StatusCode} ");
                    return new RequestResult<dynamic>(true, response, null, null);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($" Patch Request error: {e.Message}");
                    return new RequestResult<dynamic>(false, response, null, e);
                }
            }
        }
    }
}
