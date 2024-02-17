using Newtonsoft.Json.Serialization;

namespace Demo.AppInsights.Api.Helpers
{
    internal class ObfuscatorValueProvider : IValueProvider
    {
        private readonly IValueProvider _valueProvider;
        private readonly SecureAttribute _sensitiveAttribute;

        public ObfuscatorValueProvider(
            IValueProvider valueProvider,
            SecureAttribute sensitiveAttribute)
        {
            _valueProvider = valueProvider;
            _sensitiveAttribute = sensitiveAttribute;
        }

        public object GetValue(object target)
        {
            var originalValue = _valueProvider.GetValue(target);

            var result = originalValue switch
            {
                null => null,
                string strValue => _sensitiveAttribute.Obfuscate(strValue),
                Guid guid => _sensitiveAttribute.Obfuscate(guid),
                Enum @enum => _sensitiveAttribute.Obfuscate(@enum),
                DateTime dateTime => _sensitiveAttribute.Obfuscate(dateTime),
                _ => _sensitiveAttribute.ObfuscateDefault(originalValue),
            };

            return result;
        }

        public void SetValue(object target, object value)
        {
            // we don't care
        }
    }
}
