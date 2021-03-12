using System.IO;
using Microsoft.Extensions.Configuration;

namespace Momentary.Tests.Common.Common
{
    public class Configuration
    {
        public Configuration()
        {
            Current = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets("b6bd41c6-b55f-4858-9aa3-5d29f5623d3b")
                .Build();
        }
        
        public IConfiguration Current { get; }
    }
}