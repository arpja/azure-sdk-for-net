﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;

namespace Azure.Identity.Samples
{
    public class UserAuthenticationSnippets
    {
        public void Identity_ClientSideUserAuthentication_SimpleInteractiveBrowser()
        {
            #region Snippet:Identity_ClientSideUserAuthentication_SimpleInteractiveBrowser
            var client = new SecretClient(new Uri("https://myvault.azure.vaults.net/"), new InteractiveBrowserCredential());
            #endregion
        }

        public void Identity_ClientSideUserAuthentication_SimpleDeviceCode()
        {
            #region Snippet:Identity_ClientSideUserAuthentication_SimpleDeviceCode
            var credential = new DeviceCodeCredential((deviceCodeInfo, _) => { Console.WriteLine(deviceCodeInfo.Message); return Task.CompletedTask; });

            var client = new BlobClient(new Uri("https://myaccount.blob.core.windows.net/mycontainer/myblob"), credential);
            #endregion
        }

        public async Task Identity_ClientSideUserAuthentication_DisableAutomaticAuthentication()
        {
            #region Snippet:Identity_ClientSideUserAuthentication_DisableAutomaticAuthentication
            var credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions { DisableAutomaticAuthentication = true });

            await credential.AuthenticateAsync();

            var client = new SecretClient(new Uri("https://myvault.azure.vaults.net/"), credential);
            #endregion

            #region Snippet:Identity_ClientSideUserAuthentication_DisableAutomaticAuthentication_ExHandling
            try
            {
                client.GetSecret("secret");
            }
            catch (AuthenticationRequiredException e)
            {
                await EnsureAnimationCompleteAsync();

                await credential.AuthenticateAsync(e.TokenRequestContext);

                client.GetSecret("secret");
            }
            #endregion
        }

        private Task EnsureAnimationCompleteAsync() => Task.CompletedTask;

        private const string AUTH_RECORD_PATH = @".\Data\authrecord.bin";


        public static async Task<TokenCredential> GetUserCredentialAsync()
        {
            if (!File.Exists(AUTH_RECORD_PATH))
            {

                #region Snippet:Identity_ClientSideUserAuthentication_Persist_TokenCache
                var credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions { TokenCache = new PersistentTokenCache() });
                #endregion

                #region Snippet:Identity_ClientSideUserAuthentication_Persist_AuthRecord
                AuthenticationRecord authRecord = await credential.AuthenticateAsync();

                using var authRecordStream = new FileStream(AUTH_RECORD_PATH, FileMode.Create, FileAccess.Write);

                await authRecord.SerializeAsync(authRecordStream);

                await authRecordStream.FlushAsync();
                #endregion

                return credential;
            }
            else
            {
                #region Snippet:Identity_ClientSideUserAuthentication_Persist_SilentAuth
                using var authRecordStream = new FileStream(AUTH_RECORD_PATH, FileMode.Open, FileAccess.Read);

                AuthenticationRecord authRecord = await AuthenticationRecord.DeserializeAsync(authRecordStream);

                var credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions { TokenCache = new PersistentTokenCache(), AuthenticationRecord = authRecord });
                #endregion

                return credential;
            }
        }


        public static async Task Main()
        {
            InteractiveBrowserCredential credential;

            if (!File.Exists(AUTH_RECORD_PATH))
            {
                credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions { TokenCache = new PersistentTokenCache() });

                AuthenticationRecord authRecord = await credential.AuthenticateAsync();

                using var authRecordStream = new FileStream(AUTH_RECORD_PATH, FileMode.Create, FileAccess.Write);

                await authRecord.SerializeAsync(authRecordStream);

                await authRecordStream.FlushAsync();
            }
            else
            {
                using var authRecordStream = new FileStream(AUTH_RECORD_PATH, FileMode.Open, FileAccess.Read);

                AuthenticationRecord authRecord = await AuthenticationRecord.DeserializeAsync(authRecordStream);

                credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions { TokenCache = new PersistentTokenCache(), AuthenticationRecord = authRecord });
            }

            var client = new SecretClient(new Uri("https://myvault.azure.vaults.net/"), credential);
        }
    }
}
