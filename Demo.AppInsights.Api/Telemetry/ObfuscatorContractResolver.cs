using Demo.AppInsights.Api.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Demo.AppInsights.Api
{
    public class ObfuscatorContractResolver : DefaultContractResolver
    {
        protected override IList<Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> baseProperties = base.CreateProperties(type, memberSerialization);

            var sensitiveData = new Dictionary<string, SecureAttribute>();

            foreach (PropertyInfo p in type.GetProperties())
            {
                var customAttributes = p.GetCustomAttributes(false);

                var jsonPropertyAttribute = customAttributes
                    .OfType<JsonPropertyAttribute>()
                    .FirstOrDefault();

                if (jsonPropertyAttribute is null)
                    continue;

                var sensitiveAttribute = customAttributes
                    .OfType<SecureAttribute>()
                    .FirstOrDefault();

                if (sensitiveAttribute is null)
                    continue;

                var propertyName = jsonPropertyAttribute.PropertyName.ToUpperInvariant();

                sensitiveData.Add(propertyName, sensitiveAttribute);
            }

            if (!sensitiveData.Any())
                return baseProperties;

            var processedProperties = new List<JsonProperty>();

            // là on fait la magie en retraitant les champs sensibles
            foreach (JsonProperty baseProperty in baseProperties)
            {
                if (sensitiveData.TryGetValue(baseProperty.PropertyName.ToUpperInvariant(), out SecureAttribute secureAttribute))
                {
                    baseProperty.PropertyType = typeof(string);
                    baseProperty.ValueProvider = new ObfuscatorValueProvider(baseProperty.ValueProvider, secureAttribute);
                }

                processedProperties.Add(baseProperty);
            }

            return processedProperties;
        }
    }
}
