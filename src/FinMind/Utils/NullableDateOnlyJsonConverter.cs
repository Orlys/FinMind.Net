namespace FinMind.Net.Utils
{

    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal sealed class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
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