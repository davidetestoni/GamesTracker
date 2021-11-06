using Newtonsoft.Json;

namespace API.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerSettings jsonSerializerOptions = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.None
        };

        public static string Serialize(this object obj)
            => JsonConvert.SerializeObject(obj, jsonSerializerOptions);

        public static T Deserialize<T>(this string str)
            => JsonConvert.DeserializeObject<T>(str);
    }
}
