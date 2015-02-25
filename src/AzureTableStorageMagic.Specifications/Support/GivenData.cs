using System;
using Common.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic.Specifications.Support
{
    public class GivenData
    {
        private static readonly ILog Log = LogManager.GetLogger<GivenData>();
        public string ConnectionString;

        public GivenData()
        {
            TableName = string.Format("specs{0}", DateTime.Now.ToString("yyyyMMddhhmmssfff"));

            Log.DebugFormat("Given.TableName: {0}", TableName);
        }

        public string TableName { get; set; }

        public CloudTable CloudTable
        {
            get { return Table.GetCloudTable(ConnectionString, TableName); }
        }
    }
}