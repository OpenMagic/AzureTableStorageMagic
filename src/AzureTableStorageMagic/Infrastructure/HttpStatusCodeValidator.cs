namespace AzureTableStorageMagic.Infrastructure
{
    /// <summary>
    /// <see cref="HttpStatusCodeValidator"/> basically exists for unit testing purposes.
    /// </summary>
    public class HttpStatusCodeValidator : IHttpStatusCodeValidator
    {
        public bool IsOK(int httpStatusCode)
        {
            return httpStatusCode < 400;
        }
    }
}