namespace FinMind.Net.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;



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



    public class TaiwanStockPriceTickOptions
    {
        [DisallowNull]
        public string? StockId { get; set; }

        public DateOnly Date { get; set; }
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







    public class TaiwanStockPriceTick
    {

        [JsonPropertyName("date")]
        public DateOnly? Date { get; set; }

        [JsonPropertyName("stock_id")]
        public string? StockId { get; set; }

        [JsonPropertyName("deal_price")]
        public decimal DealPrice { get; set; }

        [JsonPropertyName("volume")]
        public int Volume { get; set; }

        [JsonPropertyName("Time")]
        public TimeOnly? Time { get; set; }

        [JsonPropertyName("TickType")]
        public int TickType { get; set; }
    }

}
