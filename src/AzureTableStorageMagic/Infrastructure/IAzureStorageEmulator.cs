namespace AzureTableStorageMagic.Infrastructure
{
    public interface IAzureStorageEmulator
    {
        bool IsEmulatorRunning();
        void StartEmulatorIfNotRunning();
        void StopEmulatorIfIsRunning();
    }
}