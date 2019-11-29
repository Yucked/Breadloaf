using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Breadloaf.Converters {
    public sealed class IPEndPointConverter : JsonConverter<IPEndPoint> {
        public override IPEndPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options) {
            writer.WriteStringValue($"{value}");
        }
    }
}