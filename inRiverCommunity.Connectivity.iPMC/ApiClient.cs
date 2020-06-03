using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using inRiverCommunity.Connectivity.iPMC.Services;
using inRiverCommunity.Connectivity.iPMC.Services.Impl;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace inRiverCommunity.Connectivity.iPMC
{
    /// <summary>
    ///     A REST API client that wraps functionality provided by the inRiver REST API.
    /// </summary>
    /// <remarks>
    ///     Currently only wraps Extension Management and Package Management.
    /// </remarks>
    public class ApiClient : IApiClient
    {
        private const string ApiVersion = @"v1.0.0";
        private const string BaseApiUrlFormat = @"https://{0}.productmarketingcloud.com/api/" + ApiVersion + @"/";

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly HttpClient _client;
        private readonly HttpClientHandler _clientHandler;

        /// <summary>
        ///     Create an API client with a custom API base URL and mandatory API key.
        /// </summary>
        /// <param name="apiBaseUrl">Custom API base URL</param>
        /// <param name="apiKey">User specific API key, which is mandatory for authentication with the inRiver REST API</param>
        public ApiClient(string apiBaseUrl, string apiKey)
        {
            _clientHandler = new HttpClientHandler();
            _client = new HttpClient(_clientHandler)
            {
                BaseAddress = new Uri(apiBaseUrl)
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("X-inRiver-APIKey", apiKey);

            PackageService = new ApiPackageService(this);
            ExtensionService = new ApiExtensionWrapper(this);
        }

        /// <summary>
        ///     Create an API client targeting the customer environment with mandatory API key.
        /// </summary>
        /// <param name="apiKey">User specific API key, which is mandatory for authentication with the inRiver REST API</param>
        public ApiClient(string apiKey)
            : this(ApiEnvironment.Customer, apiKey)
        {
            //
        }

        /// <summary>
        ///     Create an API client with for a known API environment and mandatory API key.
        /// </summary>
        /// <param name="environment">Known API environment</param>
        /// <param name="apiKey">User specific API key, which is mandatory for authentication with the inRiver REST API</param>
        public ApiClient(ApiEnvironment environment, string apiKey)
            : this(GetApiBaseUrl(environment), apiKey)
        {
            //
        }

        public IApiPackageService PackageService { get; }

        public IApiExtensionService ExtensionService { get; }

        public string Version => ApiVersion;

        #region IDisposable Implementation

        public void Dispose()
        {
            _client?.Dispose();
            _clientHandler?.Dispose();
        }

        #endregion

        #region HTTP DELETE Wrappers

        internal async Task<bool> DeleteAsync(string path)
        {
            using HttpResponseMessage response = await _client.DeleteAsync(GetRequestUrl(path)).ConfigureAwait(false);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        #endregion

        #region HTTP GET Wrappers

        internal async Task<TResult> GetAsync<TResult>(string path)
            where TResult : class, new()
        {
            using HttpResponseMessage response = await _client.GetAsync(GetRequestUrl(path)).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
                return default;

            response.EnsureSuccessStatusCode();

            return JsonDeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }

        #endregion

        #region HTTP POST Wrappers

        internal async Task<TResult> PostAsync<TResult>(string path)
            where TResult : class, new()
        {
            using HttpResponseMessage response = await _client.PostAsync(GetRequestUrl(path), null).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return JsonDeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }

        internal async Task<bool> PostAsync(string path)
        {
            using HttpResponseMessage response = await _client.PostAsync(GetRequestUrl(path), null).ConfigureAwait(false);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        internal async Task<TResult> PostAsync<TData, TResult>(string path, TData data)
            where TData : class
            where TResult : class, new()
        {
            HttpContent content = BuildStringContent(data);

            using HttpResponseMessage response = await _client.PostAsync(GetRequestUrl(path), content).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return JsonDeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }

        #endregion

        #region HTTP PUT Wrappers

        internal async Task<bool> PutAsync<TData>(string path, TData data)
            where TData : class
        {
            HttpContent content = BuildStringContent(data);

            using HttpResponseMessage response = await _client.PutAsync(GetRequestUrl(path), content).ConfigureAwait(false);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        internal async Task<TResult> PutAsync<TData, TResult>(string path, TData data)
            where TData : class
            where TResult : class, new()
        {
            HttpContent content = BuildStringContent(data);

            using HttpResponseMessage response = await _client.PutAsync(GetRequestUrl(path), content).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return JsonDeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }

        #endregion

        #region Helpers

        private static string JsonSerializeObject(object value) 
            => JsonConvert.SerializeObject(value, SerializerSettings);

        private static TResult JsonDeserializeObject<TResult>(string value) 
            => JsonConvert.DeserializeObject<TResult>(value, SerializerSettings);

        private static string GetApiBaseUrl(ApiEnvironment environment) 
            => string.Format(BaseApiUrlFormat, ApiEnvironmentsMap[environment]);

        private Uri GetRequestUrl(string path)
        {
            // Required without new Uri(baseUri, virtualUrl), because inRiver uses a ':' in some paths, which let Uri think it is a scheme.
            return new Uri($"{_client.BaseAddress}{path}");
        }

        private static readonly IDictionary<ApiEnvironment, string> ApiEnvironmentsMap = new Dictionary<ApiEnvironment, string>
        {
            {ApiEnvironment.Customer, @"apieuw"},
            {ApiEnvironment.Partner, @"partnerapieuw"},
            {ApiEnvironment.Demo, @"demoapieuw"}
        };

        private static StringContent BuildStringContent(object data) 
            => new StringContent(JsonSerializeObject(data), Encoding.UTF8, @"application/json");

        #endregion
    }
}