using Newtonsoft.Json;
using System;

namespace AppStoreService
{
    public class ObjectIdJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Name == "ObjectId";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string id = ((dynamic)value).ToString();
            writer.WriteRawValue($"\"{id}\"");
        }
    }
}