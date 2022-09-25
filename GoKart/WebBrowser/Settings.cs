using Newtonsoft.Json;
using System.IO;

namespace GoKart.WebBrowser
{
    public partial class WebBrowserScriptInterface
    {
        private void ParseConfiguration(string FileName)
        {
            if (this != null)
            {
                string Serialized = JsonConvert.SerializeObject(this, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new InterfaceContractResolver(typeof(IConfiguration))
                });

                if ((Serialized != null) && (!string.IsNullOrEmpty(FileName)))
                {
                    File.WriteAllText(FileName, Serialized);
                }
            }
        }

        private void PopulateConfiguration(string FileName)
        {
            if (File.Exists(FileName))
            {
                string Serialized = File.ReadAllText(FileName);

                JsonConvert.PopulateObject(Serialized, this, new JsonSerializerSettings
                {
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    ContractResolver = new InterfaceContractResolver(typeof(IConfiguration))
                });
            }
        }
    }
}