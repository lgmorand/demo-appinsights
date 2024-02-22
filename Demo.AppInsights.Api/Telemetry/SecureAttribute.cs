namespace Demo.AppInsights.Api
{
    public enum ObfuscationType
    {
        HeadVisible,
        MaskChar
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SecureAttribute : Attribute
    {
        private SensitiveBase Sensitive { get; }
        public int TruncateAfter { get; }

        public SecureAttribute(ObfuscationType obfuscationType, int truncateAfter)
        {
            TruncateAfter = truncateAfter;
            Sensitive = GetSensitive(obfuscationType);
        }

        public SecureAttribute(ObfuscationType obfuscationType)
        {
            TruncateAfter = 0;
            Sensitive = GetSensitive(obfuscationType);
        }

        private SensitiveBase GetSensitive(ObfuscationType obfuscationType)
        {
            return obfuscationType switch
            {
                ObfuscationType.HeadVisible => new SensitiveHeadVisible(TruncateAfter),
                ObfuscationType.MaskChar => new SensitiveMaskChar(TruncateAfter),
                _ => throw new NotImplementedException($"None type {nameof(obfuscationType)}: {obfuscationType}")
            };
        }

        internal string Obfuscate(string strValue)
        {
            return Sensitive.Obfuscate(strValue);
        }

        internal string Obfuscate(Guid guid) => Sensitive.Obfuscate(guid);

        internal string Obfuscate(DateTime dateTime) => Sensitive.Obfuscate(dateTime);

        internal string Obfuscate(Enum @enum) => Sensitive.Obfuscate(@enum);

        internal virtual string ObfuscateDefault(object value) => Sensitive.ObfuscateDefault(value);
    }

}
