﻿
namespace NHS111.Utils.Helpers {
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class RestfulHelper : IRestfulHelper {

        public RestfulHelper(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public RestfulHelper(HttpClient httpClient, IHttpClientFactory httpClientFactory) {
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
        }

        public RestfulHelper() {
            _webClient = new WebClient();
            _httpClient = new HttpClient();
            _httpClientFactory = new HttpClientFactory();
        }

        public Task<HttpResponseMessage> GetResponseAsync(string url) {
            return _httpClient.GetAsync(url);
        }

        public Task<HttpResponseMessage> GetResponseAsync(string url, string username, string password) {
            var httpClient = _httpClientFactory.Get(username, password);
            return httpClient.GetAsync(url);
        }

        public async Task<string> GetAsync(string url) {
            try {
                return await _webClient.DownloadStringTaskAsync(new Uri(url));
            }
            catch (WebException e) {
                using (var stream = new StreamReader(e.Response.GetResponseStream())) {
                    throw new WebException(
                        string.Format("There was a problem requesting '{0}'; {1}", url, stream.ReadToEnd()), e);
                }
            }
        }

        public async Task<string> GetAsync(string url, string credentials) {
            try {
                _webClient.Headers[HttpRequestHeader.Authorization] = credentials;

                return await _webClient.DownloadStringTaskAsync(new Uri(url));
            }
            catch (WebException e) {
                using (var stream = new StreamReader(e.Response.GetResponseStream())) {
                    throw new WebException(
                        string.Format("There was a problem requesting '{0}'; {1}", url, stream.ReadToEnd()), e);
                }
            }
        }


        public async Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request) {
            var httpRequestMessage = await BuildRequestMessage(url, request);
            return await _httpClient.SendAsync(httpRequestMessage);

        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request,
            Dictionary<string, string> headers) {
            var httpRequestMessage = await BuildRequestMessage(url, request);
            foreach (var header in headers) {
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
            return await _httpClient.SendAsync(httpRequestMessage);
        }

        private async Task<HttpRequestMessage> BuildRequestMessage(string url, HttpRequestMessage request) {
            var data = await request.Content.ReadAsStringAsync();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url)) {
                Content = new StringContent(data, Encoding.UTF8, "application/json"),
                Version = HttpVersion.Version10 //forcing 1.0 to prevent Expect 100 Continue header
            };
            return httpRequestMessage;
        }

        private readonly WebClient _webClient;
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
    }

    public interface IRestfulHelper {
        Task<HttpResponseMessage> GetResponseAsync(string url);
        Task<HttpResponseMessage> GetResponseAsync(string url, string username, string password);
        Task<string> GetAsync(string url);
        Task<string> GetAsync(string url, string credentials);
        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request);
        Task<HttpResponseMessage> PostAsync(string url, HttpRequestMessage request, Dictionary<string, string> headers);
    }
}