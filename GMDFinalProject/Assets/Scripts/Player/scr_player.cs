using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.Networking;
using System.Collections;

public class scr_player : NetworkBehaviour
{
    public static scr_player instance { get; private set; }

    public string playerName;
    public NetworkVariable<int> water = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner), 
        cardsPlayed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner), 
        destroyCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server),
        deckCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        //steel = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public List<scr_card> Deck;
    public scr_productionPlant ProductionPlant;
    public const int STARTING_WATER = 10, MAX_HAND_SIZE = 7;

    public Camera cam;
    public scr_guiManager GUI;

    public GameObject plantPrefab, alertText;
    const string winMsg = "You won! Thanks for all your help.\n" +
            "Please collect your reward and return to the barracks.", loseMsg = "You lost... what a disgrace.\n" +
            "Your pay will be docked. Return to the barracks ashamed.";
    public int timeRemain, discountPerLevel = 5;

    private void Awake()
    {
        //NetworkManager.Singleton.SceneManager.LoadScene("sce_gui", LoadSceneMode.Additive);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*scr_gameManager.instance.players.Add(this.gameObject);
        if (IsServer)
        {
            Debug.Log("Server should start here.");
            //NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }*/

        scr_gameManager.instance.players.Add(this.gameObject);

        if (!IsOwner)
        {
            cam.enabled = false;
            return;
        }
        else
        {
            /*if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }*/

            if (scr_dataPersistenceManager.instance != null)
            {
                Deck.Clear();
                foreach (scr_card card in scr_dataPersistenceManager.instance.playerData.equippedDeck.cardsInDeck)
                {
                    Deck.Add(card);
                }
                deckCount.Value = Deck.Count;

                playerName = scr_dataPersistenceManager.instance.playerData.username;
                ProductionPlant = scr_dataPersistenceManager.instance.playerData.equippedDeck.productionPlant;
            }
        }

        //gameObject.AddComponent<Camera>();
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        if(NetworkManager.Singleton.IsServer)
        {
            scr_gameManager.instance.SpawnNetworkCard(ProductionPlant.name, plantPrefab.transform.position, plantPrefab.transform.rotation, OwnerClientId);
            Debug.Log("Spawned Generator on Server-side.");
        }
        else
        {
            scr_gameManager.instance.SpawnNetworkCardServerRpc(ProductionPlant.name, plantPrefab.transform.position, plantPrefab.transform.rotation, OwnerClientId);
            Debug.Log("Spawned Generator on Client-side.");
        }

        GUI.ChangeCardCount(Deck.Count);
        ChangeWater(STARTING_WATER);
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
            cam.enabled = false;
            return;
        }

        if (timeRemain <= 0)
        {
            timeRemain -= (int)Time.deltaTime;
        }

        if (IsOwner && Input.GetKeyUp(KeyCode.Space))
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (Deck.Count > 0 && GUI.cardsInHand.Count < MAX_HAND_SIZE)
        {
            GUI.DrawCard(Deck[0], this);
            Deck.RemoveAt(0);
            deckCount.Value = Deck.Count;
            GUI.ChangeCardCount(deckCount.Value);
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
        }
        ChangeWater(-card.cost + discount);
        cardsPlayed.Value += 1;
    }

    public void ChangeWater(int waterDelta)
    {
        water.Value += waterDelta;
        GUI.UpdateWater(water.Value);
        Debug.Log("added " + waterDelta + " to water count");
    }

    public void AddKillCount()
    {
        destroyCount.Value += 1;
        GUI.UpdateAnalytics();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddKillCountServerRpc()
    {
        AddKillCount();
    }

    [ClientRpc]
    public void WinGameClientRpc()
    {
        //GameObject temp = Instantiate(alertText, GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center), Quaternion.identity, GUI.gameObject.transform);
        //alertText.transform.localPosition = GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center);
        GUI.endingTab.gameObject.SetActive(true);
        GUI.endingTab.GetComponent<scr_ending>().Ending(winMsg, water.Value, 0, true);
        /*if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }*/
        Time.timeScale = 0;
    }

    [ClientRpc]
    public void LoseGameClientRpc()
    {
        //GameObject temp = Instantiate(alertText, GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center), Quaternion.identity, GUI.gameObject.transform);
        //alertText.transform.localPosition = GUI.gameObject.GetComponent<RectTransform>().TransformPoint(GUI.gameObject.GetComponent<RectTransform>().rect.center);
        GUI.GetComponent<scr_guiManager>().endingTab.gameObject.SetActive(true);
        GUI.GetComponent<scr_guiManager>().endingTab.gameObject.GetComponent<scr_ending>().Ending(loseMsg, water.Value, 0, false);
        /*if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }*/
        Time.timeScale = 0;
    }

    IEnumerator WaitForSeconds(int time)
    {
        yield return new WaitForSeconds(time);
    }
}
