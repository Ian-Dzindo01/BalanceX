using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace BalanceX.Handlers
{
    public class KeyVaultHelper
    {
        private static string keyVaultUrl = "https://balancex.vault.azure.net/";

        public static string GetApiKey()
        {
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret("X-API-KEY");
            return secret.Value;
        }
    }
}