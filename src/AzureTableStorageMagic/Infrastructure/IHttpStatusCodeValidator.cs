namespace AzureTableStorageMagic.Infrastructure
{
    /// <summary>
    /// <see cref="IHttpStatusCodeValidator"/> basically exists for unit testing purposes.
    /// </summary>
    public interface IHttpStatusCodeValidator
    {
        bool IsOK(int httpStatusCode);
    }
}