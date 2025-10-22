using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class scr_changeName : MonoBehaviour
{
    private void OnEnable()
    {
        this.GetComponentInChildren<TMP_InputField>().text = scr_dataPersistenceManager.instance.playerData.username;
    }

    public void ChangeName()
    {
        scr_dataPersistenceManager.instance.playerData.username = this.GetComponent<TMP_InputField>().text;
        scr_dataPersistenceManager.instance.SaveGame();
        AuthenticationService.Instance.UpdatePlayerNameAsync(scr_dataPersistenceManager.instance.playerData.username);
    }
}
