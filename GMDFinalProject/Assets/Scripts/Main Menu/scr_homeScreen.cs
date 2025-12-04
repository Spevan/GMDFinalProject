using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

public class scr_homeScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await UnityServices.InitializeAsync();
        if(!AuthenticationService.Instance.IsSignedIn)
        {
            await InitializeUnityServices();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        if (scr_dataPersistenceManager.instance != null)
        {
            if (scr_dataPersistenceManager.instance.playerData.username == "")
                {
                    scr_dataPersistenceManager.instance.playerData.username = await AuthenticationService.Instance.GetPlayerNameAsync();
                }
            else
                {
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(scr_dataPersistenceManager.instance.playerData.username);
                }
        }
    }

    async Task InitializeUnityServices()
    {
        if(UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
