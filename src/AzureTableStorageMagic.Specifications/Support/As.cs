namespace AzureTableStorageMagic.Specifications.Support
{
    public static class As
    {
        public static string AsString(this string value)
        {
            switch (value)
            {
                case "null":
                    return null;

                case "empty":
                    return "";

                case "whitespace":
                    return " ";

                case "1025 characters":
                    return "".PadRight(1025, 'a');

                default:
                    return value;
            }
        }
    }
}
