using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class scr_player : NetworkBehaviour
{
    public static scr_player instance { get; private set; }

    public NetworkVariable<int> water = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner),
        steel = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public List<scr_card> Deck;
    public GameObject ProductionPlant;
    public Camera cam;
    public scr_guiManager GUI;
    int timeRemain;
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
        }
        //gameObject.AddComponent<Camera>();
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
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
        for (int i = 0; i < card.costType.Length; i++)
        {
            if (card.costType[i] == scr_card.costTypes.water)
            {
                ChangeWater(-card.cost[i]);
            }
            else if (card.costType[i] == scr_card.costTypes.steel)
            {
                steel.Value -= card.cost[i];
            }
        }
    }

    public void ChangeWater(int waterDelta)
    {
        water.Value += waterDelta;
        GUI.UpdateWater(water.Value);
        Debug.Log("added " + waterDelta + " to water count");
    }

    [ServerRpc]
    public void ChangeWaterServerRpc(int waterDelta)
    {
        ChangeWater(waterDelta);
        GUI.UpdateWater(water.Value);
        Debug.Log("added " + waterDelta + " to water count");
    }
}
