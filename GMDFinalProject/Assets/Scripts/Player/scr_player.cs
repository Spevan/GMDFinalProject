using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System.Collections;

public class scr_player : NetworkBehaviour
{
    public static scr_player instance { get; private set; }

    public string playerName;
    public NetworkVariable<int> water = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        //steel = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public List<scr_card> Deck;
    public scr_productionPlant ProductionPlant;
    public Camera cam;
    public scr_guiManager GUI;
    public GameObject plantPrefab, alertText;
    public int timeRemain, discountPerLevel = 5, cardsPlayed;
    private void Awake()
    {
        //NetworkManager.Singleton.SceneManager.LoadScene("sce_gui", LoadSceneMode.Additive);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!IsOwner)
        {
            cam.enabled = false;
            return;
        }
        else
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            foreach (scr_card card in scr_dataPersistenceManager.instance.playerData.equippedDeck.cardsInDeck)
            {
                Deck.Add(card);
            }
        }
        //gameObject.AddComponent<Camera>();
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        playerName = scr_dataPersistenceManager.instance.playerData.username;
        GUI.ChangeCardCount(Deck.Count);
        ChangeWater(10);
        //GUI.UpdateWater(water.Value);
        //foreach (scr_card card in Deck)
        {
            //card.player = this;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
        {
            return;
        }

        if(timeRemain <= 0)
        {
            timeRemain -= (int)Time.deltaTime;
        }

        if (IsOwner)
        {
            DrawCard();
        }
        else
        {
            cam.enabled = false;
        }
    }

    public void DrawCard()
    {
        if (Input.GetKeyUp(KeyCode.Space) && Deck.Count > 0)
        {
            GUI.DrawCard(Deck[0], this);
            Deck.RemoveAt(0);
            GUI.ChangeCardCount(Deck.Count);
        }
    }

    public void PlayCard(scr_card card)
    {
        int discount = 0;
        foreach (scr_status status in card.statuses)
        {
            if(status.statusType == scr_status.statusTypes.Frugal)
            {
                discount = status.statusAmnt * discountPerLevel;
            }
            else if(status.statusType == scr_status.statusTypes.Greedy)
            {
                discount = -status.statusAmnt * discountPerLevel;
            }
        }
        ChangeWater(-card.cost + discount);
        cardsPlayed++;
    }

    public void ChangeWater(int waterDelta)
    {
        water.Value += waterDelta;
        GUI.UpdateWater(water.Value);
        Debug.Log("added " + waterDelta + " to water count");
    }

    [ClientRpc]
    public void WinGameClientRpc()
    {
        //GameObject temp = Instantiate(alertText, GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center), Quaternion.identity, GUI.gameObject.transform);
        alertText.transform.localPosition = GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center);
        alertText.SetActive(true);
        alertText.GetComponent<scr_alertText>().ChangeText("You won! Thanks for all your help General.\n" +
            "Please collect your reward and return to the barracks.");
        Time.timeScale = 0;
        WaitForSeconds(5);
        SceneManager.LoadScene("sce_mainMenu");
    }

    [ClientRpc]
    public void LoseGameClientRpc()
    {
        //GameObject temp = Instantiate(alertText, GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center), Quaternion.identity, GUI.gameObject.transform);
        alertText.transform.localPosition = GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center);
        alertText.SetActive(true);
        alertText.GetComponent<scr_alertText>().ChangeText("You lost... what a disgrace.\n" +
            "Your pay will be docked. Return to the barracks ashamed.");
        Time.timeScale = 0;
        WaitForSeconds(5);
        SceneManager.LoadScene("sce_mainMenu");
    }

    IEnumerator WaitForSeconds(int time)
    {
        yield return new WaitForSeconds(time);
    }
}
