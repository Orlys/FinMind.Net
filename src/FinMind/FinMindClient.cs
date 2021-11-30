namespace FinMind.Net
{
    using FinMind.Net.Helpers;
    using FinMind.Net.Models;

    using Models;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Web;

    public class FinMindException : Exception
    {
        public FinMindException(string message) : base(message)
        {

        }
    }

    public interface IFinMindClient
    {
        bool IsAuthenticated { get; }
        Task Login(string userId, string password, CancellationToken cancellationToken = default);
    }

    public class FinMindClient
    {
        private const string API_ENDPOINT_V4 = "https://api.finmindtrade.com/api/v4/";
        private const string API_WEB_ENDPOINT_V2 = "https://api.web.finmindtrade.com/v2/";

        private volatile string? _token;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonConverterOptions;

        public bool IsAuthenticated => this._token is not null;

        public FinMindClient([NotNull] HttpClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client)); 

            this._jsonConverterOptions = new();
            this._jsonConverterOptions.Converters.Add(new DateOnlyJsonConverter());
            this._jsonConverterOptions.Converters.Add(new NullableDateOnlyJsonConverter());
        }

        public FinMindClient() : this(new HttpClient())
        {

        }


        public async Task Login([DisallowNull] string userId, [DisallowNull] string password, CancellationToken cancellationToken = default)
        {
            var response = await _client.PostAsync(API_ENDPOINT_V4 +  "login", new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("user_id", userId),
                    new KeyValuePair<string, string>("password", password)
                }), cancellationToken);

            var tokenResp = (await response
                .Content
                .ReadFromJsonAsync<FinMindResponse<Empty>>(cancellationToken: cancellationToken))!;

            this._token = response.IsSuccessStatusCode
                ? tokenResp.AccessToken
                : throw new FinMindException(tokenResp.Message!);
        }

        // https://api.web.finmindtrade.com/v2/user_info?token=your_token

        public async Task<UserInfo> GetUserInfo(CancellationToken cancellationToken = default)
        {

            var uri = API_WEB_ENDPOINT_V2 + "user_info?token=" + this._token;
            var response = await _client.GetFromJsonAsync<UserInfo>(
               uri,
               _jsonConverterOptions,
               cancellationToken);

            return response!;
        }




        //public Task GetTaiwanStockPrice([AllowNull] TaiwanStockInfoRequest? request = default, CancellationToken cancellationToken = default)
        //{
        //    var param = new Dictionary<string, string?>();
        //    if (request is not null)
        //    {
        //        param["stock_id"] = request.StockId;
        //        param["start_date"] = request.StartDate?.ToString("yyyy-MM-dd");
        //        param["end_date"] = request.EndDate?.ToString("yyyy-MM-dd");
        //    }

        //    var response = await _client.GetFromJsonAsync<FinMindResponse<TaiwanStockInfo>>(
        //        QueryHelper.ToDataQueryString("TaiwanStockInfo", param!),
        //        _jsonConverterOptions,
        //        cancellationToken);

        //    var data = response!.StatusCode == HttpStatusCode.OK
        //        ? response.Data
        //        : throw new FinMindException(response.Message!);
        //    return data;
        //}
        public async Task<List<TaiwanStockInfo>> GetTaiwanStockInfo([AllowNull] TaiwanStockInfoRequest? request = default, CancellationToken cancellationToken = default)
        {
            var param = new Dictionary<string, string?>();
            if (request is not null)
            {
                param["stock_id"] = request.StockId;
                param["start_date"] = request.StartDate?.ToString("yyyy-MM-dd");
                param["end_date"] = request.EndDate?.ToString("yyyy-MM-dd");
            }

            var response = await _client.GetFromJsonAsync<FinMindResponse<TaiwanStockInfo>>(
                API_ENDPOINT_V4 + QueryHelper.ToDataQueryString("TaiwanStockInfo", param!),
                _jsonConverterOptions,
                cancellationToken);

            var data = response!.StatusCode == HttpStatusCode.OK
                ? response.Data
                : throw new FinMindException(response.Message!);
            return data;
        }
    }

    namespace Helpers
    {
        internal static class QueryHelper
        {
            public static string ToDataQueryString(string dataset, Dictionary<string, string?> param)
            {
                param["dataset"] = dataset;
                return "data?" + string.Join("&", param.Select(pair => string.Format("{0}={1}", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value))));
            }
        }

        public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
        {
            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateOnly.FromDateTime(reader.GetDateTime());
            }

            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            {
                var isoDate = value.ToString("yyyy/MM/dd");
                writer.WriteStringValue(isoDate);
            }
        }
        public sealed class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
        {
            public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TryGetDateTime(out var dateTime))
                    return DateOnly.FromDateTime(dateTime);
                return null;
            }

            public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
            {
                var isoDate = value?.ToString("yyyy/MM/dd");
                writer.WriteStringValue(isoDate);
            }
        }
    }
    namespace Models
    {


        #region User Info
        public class UserInfo
        {
            [JsonPropertyName("msg")] 
            public string? Message { get; set; }

            [JsonPropertyName("status")]
            public HttpStatusCode StatusCode { get; set; }

            [JsonPropertyName("email_verify")] 
            public bool IsEmailVerified { get; set; }

            [JsonPropertyName("user_id")] 
            public string? UserId { get; set; }

            [JsonPropertyName("email")] 
            public string? Email { get; set; }

            [JsonPropertyName("level")] 
            public int Level { get; set; }

            [JsonPropertyName("end_date")]
            public DateOnly? EndDate { get; set; }

            [JsonPropertyName("user_count")]
            public int ApiRequestedCount { get; set; }

            [JsonPropertyName("api_request_limit")]
            public int ApiRequestLimit { get; set; }

            [JsonPropertyName("api_request_limit_hour")] 
            public int ApiRequestLimitHour { get; set; }

            [JsonPropertyName("api_request_limit_day")]
            public string? ApiRequestLimitDay { get; set; }

            [JsonPropertyName("level_title")] 
            public string? LevelTitle { get; set; }

            [JsonPropertyName("BackerInfo")]
            public SubscriptionInfo? BackerInfo { get; set; }

            [JsonPropertyName("SponsorInfo")]
            public SubscriptionInfo? SponsorInfo { get; set; }
        }

        public class SubscriptionInfo
        {
            [JsonPropertyName("subscription_begin_date")]
            public DateOnly? SubscriptionBeginDate { get; set; }

            [JsonPropertyName("subscription_expired_date")]
            public DateOnly? SubscriptionExpiredDate { get; set; }

            [JsonPropertyName("api_request_limit")]
            public int ApiRequestLimit { get; set; }

            [JsonPropertyName("msg")]
            public string? Message { get; set; }

            [JsonPropertyName("status_code")]
            public HttpStatusCode StatusCode { get; set; }
        }


        #endregion



        public class TaiwanStockInfoRequest
        {
            [AllowNull]
            public string? StockId { get; set; }

            [AllowNull]
            public DateOnly? StartDate { get; set; }

            [AllowNull]
            public DateOnly? EndDate { get; set; }
        }

        public class TaiwanStockInfo
        {
            [JsonPropertyName("industry_category")]
            public string? IndustryCategory { get; set; }

            [JsonPropertyName("stock_id")]
            public string? StockId { get; set; }

            [JsonPropertyName("stock_name")]
            public string? StockName { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; }

            [JsonPropertyName("date")]
            public DateOnly? Date { get; set; }
        }


        internal class FinMindResponse<TItem> where TItem : new()
        {
            [JsonPropertyName("msg")]
            public string? Message { get; set; }

            [JsonPropertyName("status")]
            public HttpStatusCode? StatusCode { get; set; }

            [JsonPropertyName("token")]
            public string? AccessToken { get; set; }

            [JsonPropertyName("data")]
            public List<TItem> Data { get; set; } = new();
        }

        public sealed class Empty
        {
        }
    }
}