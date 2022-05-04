namespace nwsAPI
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Rootobject
    {
        public object[] context { get; set; }
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public float[][][] coordinates { get; set; }
    }

    public class Properties
    {
        public DateTime updated { get; set; }
        public string units { get; set; }
        public string forecastGenerator { get; set; }
        public DateTime generatedAt { get; set; }
        public DateTime updateTime { get; set; }
        public string validTimes { get; set; }
        public Elevation elevation { get; set; }
        public Period[] periods { get; set; }
    }

    public class Elevation
    {
        public string unitCode { get; set; }
        public float value { get; set; }
    }

    public class Period
    {
        public int number { get; set; }
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public bool isDaytime { get; set; }
        public int temperature { get; set; }
        public string temperatureUnit { get; set; }
        public object temperatureTrend { get; set; }
        public string windSpeed { get; set; }
        public string windDirection { get; set; }
        public string icon { get; set; }
        public string shortForecast { get; set; }
        public string detailedForecast { get; set; }
    }
}

//namespace nwsAPI
//{
//    using System;
//    using System.Collections.Generic;

//    using System.Globalization;
//    using Newtonsoft.Json;
//    using Newtonsoft.Json.Converters;

//    public partial class ForecastHourly
//    {
//        [JsonProperty("@context")]
//        public List<ContextElement> Context { get; set; }

//        [JsonProperty("type")]
//        public string Type { get; set; }

//        [JsonProperty("geometry")]
//        public Geometry Geometry { get; set; }

//        [JsonProperty("properties")]
//        public Properties Properties { get; set; }
//    }

//    public partial class ContextClass
//    {
//        [JsonProperty("@version")]
//        public string Version { get; set; }

//        [JsonProperty("wx")]
//        public Uri Wx { get; set; }

//        [JsonProperty("geo")]
//        public Uri Geo { get; set; }

//        [JsonProperty("unit")]
//        public Uri Unit { get; set; }

//        [JsonProperty("@vocab")]
//        public Uri Vocab { get; set; }
//    }

//    public partial class Geometry
//    {
//        [JsonProperty("type")]
//        public string Type { get; set; }

//        [JsonProperty("coordinates")]
//        public List<List<List<double>>> Coordinates { get; set; }
//    }

//    public partial class Properties
//    {
//        [JsonProperty("updated")]
//        public DateTimeOffset Updated { get; set; }

//        [JsonProperty("units")]
//        public string Units { get; set; }

//        [JsonProperty("forecastGenerator")]
//        public string ForecastGenerator { get; set; }

//        [JsonProperty("generatedAt")]
//        public DateTimeOffset GeneratedAt { get; set; }

//        [JsonProperty("updateTime")]
//        public DateTimeOffset UpdateTime { get; set; }

//        [JsonProperty("validTimes")]
//        public string ValidTimes { get; set; }

//        [JsonProperty("elevation")]
//        public Elevation Elevation { get; set; }

//        [JsonProperty("periods")]
//        public List<Period> Periods { get; set; }
//    }

//    public partial class Elevation
//    {
//        [JsonProperty("unitCode")]
//        public string UnitCode { get; set; }

//        [JsonProperty("value")]
//        public double Value { get; set; }
//    }

//    public partial class Period
//    {
//        [JsonProperty("number")]
//        public long Number { get; set; }

//        [JsonProperty("name")]
//        public string Name { get; set; }

//        [JsonProperty("startTime")]
//        public DateTimeOffset StartTime { get; set; }

//        [JsonProperty("endTime")]
//        public DateTimeOffset EndTime { get; set; }

//        [JsonProperty("isDaytime")]
//        public bool IsDaytime { get; set; }

//        [JsonProperty("temperature")]
//        public long Temperature { get; set; }

//        [JsonProperty("temperatureUnit")]
//        public TemperatureUnit TemperatureUnit { get; set; }

//        [JsonProperty("temperatureTrend")]
//        public object TemperatureTrend { get; set; }

//        [JsonProperty("windSpeed")]
//        public string WindSpeed { get; set; }

//        [JsonProperty("windDirection")]
//        public string WindDirection { get; set; }

//        [JsonProperty("icon")]
//        public Uri Icon { get; set; }

//        [JsonProperty("shortForecast")]
//        public ShortForecast ShortForecast { get; set; }

//        [JsonProperty("detailedForecast")]
//        public string DetailedForecast { get; set; }
//    }

//    public enum ShortForecast { ChanceShowersAndThunderstorms, Clear, MostlyClear, MostlySunny, ShowersAndThunderstorms, ShowersAndThunderstormsLikely, SlightChanceShowersAndThunderstorms, Sunny };

//    public enum TemperatureUnit { F };

//    public partial struct ContextElement
//    {
//        public ContextClass ContextClass;
//        public Uri PurpleUri;

//        public static implicit operator ContextElement(ContextClass ContextClass) => new ContextElement { ContextClass = ContextClass };
//        public static implicit operator ContextElement(Uri PurpleUri) => new ContextElement { PurpleUri = PurpleUri };
//    }

//    internal static class Converter
//    {
//        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
//        {
//            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
//            DateParseHandling = DateParseHandling.None,
//            Converters =
//            {
//                ContextElementConverter.Singleton,
//                ShortForecastConverter.Singleton,
//                TemperatureUnitConverter.Singleton,
//                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
//            },
//        };
//    }

//    internal class ContextElementConverter : JsonConverter
//    {
//        public override bool CanConvert(Type t) => t == typeof(ContextElement) || t == typeof(ContextElement?);

//        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//        {
//            switch (reader.TokenType)
//            {
//                case JsonToken.String:
//                case JsonToken.Date:
//                    var stringValue = serializer.Deserialize<string>(reader);
//                    try
//                    {
//                        var uri = new Uri(stringValue);
//                        return new ContextElement { PurpleUri = uri };
//                    }
//                    catch (UriFormatException) { }
//                    break;
//                case JsonToken.StartObject:
//                    var objectValue = serializer.Deserialize<ContextClass>(reader);
//                    return new ContextElement { ContextClass = objectValue };
//            }
//            throw new Exception("Cannot unmarshal type ContextElement");
//        }

//        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//        {
//            var value = (ContextElement)untypedValue;
//            if (value.PurpleUri != null)
//            {
//                serializer.Serialize(writer, value.PurpleUri.ToString());
//                return;
//            }
//            if (value.ContextClass != null)
//            {
//                serializer.Serialize(writer, value.ContextClass);
//                return;
//            }
//            throw new Exception("Cannot marshal type ContextElement");
//        }

//        public static readonly ContextElementConverter Singleton = new ContextElementConverter();
//    }

//    internal class ShortForecastConverter : JsonConverter
//    {
//        public override bool CanConvert(Type t) => t == typeof(ShortForecast) || t == typeof(ShortForecast?);

//        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//        {
//            if (reader.TokenType == JsonToken.Null) return null;
//            var value = serializer.Deserialize<string>(reader);
//            switch (value)
//            {
//                case "Chance Showers And Thunderstorms":
//                    return ShortForecast.ChanceShowersAndThunderstorms;
//                case "Clear":
//                    return ShortForecast.Clear;
//                case "Mostly Clear":
//                    return ShortForecast.MostlyClear;
//                case "Mostly Sunny":
//                    return ShortForecast.MostlySunny;
//                case "Showers And Thunderstorms":
//                    return ShortForecast.ShowersAndThunderstorms;
//                case "Showers And Thunderstorms Likely":
//                    return ShortForecast.ShowersAndThunderstormsLikely;
//                case "Slight Chance Showers And Thunderstorms":
//                    return ShortForecast.SlightChanceShowersAndThunderstorms;
//                case "Sunny":
//                    return ShortForecast.Sunny;
//            }
//            throw new Exception("Cannot unmarshal type ShortForecast");
//        }

//        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//        {
//            if (untypedValue == null)
//            {
//                serializer.Serialize(writer, null);
//                return;
//            }
//            var value = (ShortForecast)untypedValue;
//            switch (value)
//            {
//                case ShortForecast.ChanceShowersAndThunderstorms:
//                    serializer.Serialize(writer, "Chance Showers And Thunderstorms");
//                    return;
//                case ShortForecast.Clear:
//                    serializer.Serialize(writer, "Clear");
//                    return;
//                case ShortForecast.MostlyClear:
//                    serializer.Serialize(writer, "Mostly Clear");
//                    return;
//                case ShortForecast.MostlySunny:
//                    serializer.Serialize(writer, "Mostly Sunny");
//                    return;
//                case ShortForecast.ShowersAndThunderstorms:
//                    serializer.Serialize(writer, "Showers And Thunderstorms");
//                    return;
//                case ShortForecast.ShowersAndThunderstormsLikely:
//                    serializer.Serialize(writer, "Showers And Thunderstorms Likely");
//                    return;
//                case ShortForecast.SlightChanceShowersAndThunderstorms:
//                    serializer.Serialize(writer, "Slight Chance Showers And Thunderstorms");
//                    return;
//                case ShortForecast.Sunny:
//                    serializer.Serialize(writer, "Sunny");
//                    return;
//            }
//            throw new Exception("Cannot marshal type ShortForecast");
//        }

//        public static readonly ShortForecastConverter Singleton = new ShortForecastConverter();
//    }

//    internal class TemperatureUnitConverter : JsonConverter
//    {
//        public override bool CanConvert(Type t) => t == typeof(TemperatureUnit) || t == typeof(TemperatureUnit?);

//        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//        {
//            if (reader.TokenType == JsonToken.Null) return null;
//            var value = serializer.Deserialize<string>(reader);
//            if (value == "F")
//            {
//                return TemperatureUnit.F;
//            }
//            throw new Exception("Cannot unmarshal type TemperatureUnit");
//        }

//        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//        {
//            if (untypedValue == null)
//            {
//                serializer.Serialize(writer, null);
//                return;
//            }
//            var value = (TemperatureUnit)untypedValue;
//            if (value == TemperatureUnit.F)
//            {
//                serializer.Serialize(writer, "F");
//                return;
//            }
//            throw new Exception("Cannot marshal type TemperatureUnit");
//        }

//        public static readonly TemperatureUnitConverter Singleton = new TemperatureUnitConverter();
//    }
//}
