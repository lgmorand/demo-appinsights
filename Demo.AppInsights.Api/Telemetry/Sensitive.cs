namespace Demo.AppInsights.Api
{
    public class SensitiveBase
    {
        internal virtual string Obfuscate(string strValue)
        {
            return strValue;
        }

        internal string Obfuscate(Guid guid) 
        { 
            return guid.ToString(); 
        }

        internal string Obfuscate(DateTime dateTime)
        {
            return dateTime.ToString();
        }

        internal string Obfuscate(Enum @enum) {
            return @enum.ToString();
        }

        internal string ObfuscateDefault(object value) {
            return value.ToString();
        }
    }

    public class SensitiveHeadVisible : SensitiveBase
    {
        public int TruncateAfter { get; set; }

        public SensitiveHeadVisible(int truncateAfter)
        {
            TruncateAfter = truncateAfter;
        }

        private string Truncate(string strValue)
        {
            if (strValue.Length <= TruncateAfter)
            {
                return strValue;
            }

            string visiblePart = strValue.Substring(0, TruncateAfter);
            string obfuscatedPart = new string('*', strValue.Length - TruncateAfter);

            return visiblePart + obfuscatedPart;
        }



        override internal string Obfuscate(string strValue)
        {
            return Truncate(strValue);
        }

        internal string Obfuscate(Guid guid)
        {
            return Truncate(guid.ToString());
        }

        internal string Obfuscate(DateTime dateTime)
        {
            return Truncate(dateTime.ToString());
        }

        internal string Obfuscate(Enum @enum)
        {
            return Truncate(@enum.ToString());
        }

        internal virtual string ObfuscateDefault(object value)
        {
            return Truncate(value.ToString());
        }
    }

    public class SensitiveTailVisible : SensitiveBase
    {
        // todo
    }

    public class SensitiveMaskChar : SensitiveBase
    {
        public int? TruncateAfter { get; set; }

        public SensitiveMaskChar(int? truncateAfter)
        {
            TruncateAfter = truncateAfter;
        }

        override internal string Obfuscate(string strValue)
        {
            return "****";
        }

        internal string Obfuscate(Guid guid)
        {
            return "****";
        }

        internal string Obfuscate(DateTime dateTime)
        {
            return "****";
        }

        internal string Obfuscate(Enum @enum)
        {
            return "****";
        }

        internal virtual string ObfuscateDefault(object value)
        {
            return "****";
        }
    }

}
