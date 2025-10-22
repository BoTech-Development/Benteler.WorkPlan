using Benteler.WorkPlan.Api.SharedModels.Authentication.Result;
using BitzArt.Blazor.Cookies;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Benteler.WorkPlan.Web.Api
{
    public class HttpRequestHelper
    {
        public LoginToken Token { set; get; }

        private string _baseUrl;
        public HttpRequestHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public void SaveTokenCookies(ICookieService cookieService , LoginToken token)
        {
            Console.Write($"Saving token to cookies... ");
            cookieService.SetAsync(new Cookie("Benteler.WorkPlan.Auth.Cookie.AccessToken", token.AccessToken, DateTimeOffset.Now.AddMinutes(30)));
            cookieService.SetAsync(new Cookie("Benteler.WorkPlan.Auth.Cookie.RefreshToken", token.RefreshToken, DateTimeOffset.Now.AddMinutes(30)));
            cookieService.SetAsync(new Cookie("Benteler.WorkPlan.Auth.Cookie.TokenType", token.TokenType, DateTimeOffset.Now.AddMinutes(30)));
            cookieService.SetAsync(new Cookie("Benteler.WorkPlan.Auth.Cookie.ExpiresIn", token.ExpiresIn.ToString(), DateTimeOffset.Now.AddMinutes(30)));
            Console.WriteLine($"Saved!");
        }
        public async Task<LoginToken?> LoadTokenCookies(ICookieService cookieService)
        {
         
            Console.Write($"Saving token to cookies... ");
            Cookie accessTokenCookie = await cookieService.GetAsync("Benteler.WorkPlan.Auth.Cookie.AccessToken");
            Cookie refreshTokenCookie = await cookieService.GetAsync("Benteler.WorkPlan.Auth.Cookie.RefreshToken");
            Cookie tokenTypeCookie = await cookieService.GetAsync("Benteler.WorkPlan.Auth.Cookie.TokenType");
            Cookie expiresInCookie = await cookieService.GetAsync("Benteler.WorkPlan.Auth.Cookie.ExpiresIn");
            Console.WriteLine($"Saved!");
            if(accessTokenCookie.Value != string.Empty && 
                refreshTokenCookie.Value != string.Empty &&
                tokenTypeCookie.Value != string.Empty &&
                expiresInCookie.Value != string.Empty)
            {
                return new LoginToken()
                {
                    AccessToken = accessTokenCookie.Value,
                    RefreshToken = refreshTokenCookie.Value,
                    TokenType = tokenTypeCookie.Value,
                    ExpiresIn = int.Parse(expiresInCookie.Value)
                };
            }
            else
            {
                return null;
            }
        }
        public async Task<RequestResult<dynamic>> HttpGetFile(string url, string fileName)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? response = null;
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);
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
        public async Task<RequestResult<T>> HttpGetJsonObject<T>(string url, HttpContent? content)
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);
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
        public async Task<RequestResult<dynamic>> HttpPostJson(string url, object? content)
        {
            if(content != null)
                return await HttpPost(url, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
            else 
                return await HttpPost(url, new StringContent(string.Empty));
            /*     using (HttpClient client = new HttpClient())
                 {
                     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);
                     client.BaseAddress = new Uri(_baseUrl);
                     HttpResponseMessage? response = null;
                     try
                     {
                         Console.Write($"Performing Post request: {_baseUrl + url}");
                         string json = JsonConvert.SerializeObject(content);
                         StringContent jsonStringContent = new StringContent(json, Encoding.UTF8, "application/json");
                         response = await client.PostAsync(url, jsonStringContent);

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
                 }*/
        }

        public async Task<RequestResult<T>> HttpPostJsonAndGetJson<T>(string url, object? content)
        {
            try
            {
                RequestResult<dynamic> result = await HttpPostJson(url, content);
                if (result.IsSuccess())
                {
                    string jsonData = await result.ResponseMessage!.Content.ReadAsStringAsync();
                    if (jsonData.Length > 0)
                        return new RequestResult<T>(true, result.ResponseMessage,
                            JsonConvert.DeserializeObject<T>(jsonData), null);
                }

                return new RequestResult<T>(false, result.ResponseMessage, default(T), result.Error);
            }
            catch (Exception e)
            {
                return new RequestResult<T>(false, null, default(T), e);
            }
        }

        public async Task<RequestResult<T>> HttpPostGetJson<T>(string url)
        {
            return await HttpPostJsonAndGetJson<T>(url, null);
        }
    }
}
