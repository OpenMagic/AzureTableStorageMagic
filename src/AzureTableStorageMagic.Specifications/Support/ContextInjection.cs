using AzureTableStorageMagic.Infrastructure;
using BoDi;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Support
{
    [Binding]
    public class ContextInjection
    {
        private readonly IObjectContainer _objectContainer;

        public ContextInjection(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void InitializeObjectContainer()
        {
            _objectContainer.RegisterInstanceAs(new AzureStorageEmulator(),typeof(IAzureStorageEmulator));
            _objectContainer.RegisterInstanceAs(new EntityValidator(), typeof(IEntityValidator));
        }
    }
}