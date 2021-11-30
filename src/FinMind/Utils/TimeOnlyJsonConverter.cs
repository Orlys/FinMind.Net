namespace FinMind.Net.Utils
{

    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal sealed class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.ParseExact(reader.GetString()!, "hh:mm:ss.fff");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            var time = value.ToString("hh:mm:ss.fff");
            writer.WriteStringValue(time);
        }
    }
}