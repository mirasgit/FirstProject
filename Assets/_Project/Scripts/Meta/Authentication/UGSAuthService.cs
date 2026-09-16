using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace FirstProject.Meta.Authentication
{
    public class UGSAuthService
    {
        public async UniTask LoginAnonymouslyAsync(CancellationToken token)
        {
            await UnityServices.InitializeAsync().AsUniTask().AttachExternalCancellation(token);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync().AsUniTask().AttachExternalCancellation(token);

                Debug.Log($"Auth verified! PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
        }
    }
}
