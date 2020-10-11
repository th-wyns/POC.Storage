using System.IO;
using Microsoft.Extensions.Configuration;

namespace POC.Storage.NetworkShare.Tests
{
    class Connection
    {
        internal readonly string ProjectId = "POC.Storage.Tests";
        IConfigurationRoot Configuration { get; }
        internal string ConnectionString { get; }
        internal readonly static Connection Instance = new Connection();

        Connection()
        {
            Configuration = GetConfiguration();
            ConnectionString = GetConnectionString();
            DeleteBinaryStorage();
        }

        void DeleteBinaryStorage()
        {
            var path = Path.Combine(ConnectionString, ProjectId);
            if (Directory.Exists(path) && path.Contains("POC.Storage.Tests", System.StringComparison.Ordinal))
            {
                Directory.Delete(path, true);
            }
        }

        IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        }

        string GetConnectionString()
        {
            return Configuration.GetConnectionString("POC.Storage.Binary");
        }
    }
}
