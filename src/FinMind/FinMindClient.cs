namespace FinMind.Net
{
    using FinMind.Net.Models;
    using FinMind.Net.Utils;

    using RestSharp;
    using RestSharp.Authenticators;
    using RestSharp.Serializers.SystemTextJson;

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Text.Json;
    using System.Threading;


    public class FinMindClient
    {
        private const string API_ENDPOINT_V4 = "https://api.finmindtrade.com/api/v4/";
        private const string API_WEB_ENDPOINT_V2 = "https://api.web.finmindtrade.com/v2/";

        private readonly string? _token;
        private readonly IRestClient _webClient;
        private readonly IRestClient _apiClient;
        private readonly JsonSerializerOptions _jsonConverterOptions = new()
        {
            Converters = {
                new DateOnlyJsonConverter(),
                new NullableDateOnlyJsonConverter(),
                new TimeOnlyJsonConverter(),
                new NullableTimeOnlyJsonConverter()
            }
        };

        public bool IsAuthenticated => this._token is not null;

        /// <summary>
        /// 登入 <see href="finmindtrade.com">FinMind</see>，並初始化 <see cref="FinMindClient"/> 的類型實體。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <exception cref="FinMindException"></exception>
        public FinMindClient([DisallowNull] string userId, [DisallowNull] string password)
        {
            this._webClient = new RestClient(API_WEB_ENDPOINT_V2)
                .UseSystemTextJson(_jsonConverterOptions);

            this._apiClient = new RestClient(API_ENDPOINT_V4)
                .UseSystemTextJson(_jsonConverterOptions);
            this._apiClient.Authenticator = new SimpleAuthenticator("user_id", userId, "password", password);

            var request = new RestRequest("login", Method.POST);
            var resp = _apiClient.Execute<FinMindResponse<Empty>>(request);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new FinMindException(resp.Data.Message!);
            }
            this._token = resp.Data.AccessToken;
        }

        /// <summary>
        /// 取得使用者資訊。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="FinMindException"></exception>
        // https://api.web.finmindtrade.com/v2/user_info?token=your_token 
        public async Task<UserInfo> GetUserInfo(CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("user_info", Method.GET)
                .AddQueryParameter("token", this._token!);

            var resp = await _webClient.ExecuteAsync<UserInfo>(request, cancellationToken);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new FinMindException(resp.Data.Message!);
            }
            return resp.Data;
        }

        /// <summary>
        /// 取得台股總覽。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="FinMindException"></exception>
        public async Task<List<TaiwanStockInfo>> GetTaiwanStockInfo(CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("data", Method.GET)
               .AddQueryParameter("dataset", "TaiwanStockInfo")
               ;

            var resp = await _apiClient.ExecuteAsync<FinMindResponse<TaiwanStockInfo>>(request, cancellationToken);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new FinMindException(resp.Data.Message!);
            }

            return resp.Data.Data;
        }

        /// <summary>
        /// 台灣股價歷史逐筆資料表。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="FinMindException"></exception>
        public async Task<List<TaiwanStockPriceTick>> GetTaiwanStockPriceTick([DisallowNull] TaiwanStockPriceTickOptions options, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("data", Method.GET)
               .AddQueryParameter("dataset", "TaiwanStockPriceTick")
               .AddQueryParameter("data_id", options.StockId!)
               .AddQueryParameter("start_date", options.Date.ToString("yyyy-MM-dd"))
               .AddQueryParameter("token", this._token!)
               ;

            var resp = await _apiClient.ExecuteAsync<FinMindResponse<TaiwanStockPriceTick>>(request, cancellationToken);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new FinMindException(resp.Data.Message!);
            }

            return resp.Data.Data;
        }
         
    } 



}