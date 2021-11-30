namespace FinMind.Net.Utils
{

    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
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
}