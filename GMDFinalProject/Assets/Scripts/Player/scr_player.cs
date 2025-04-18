using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class scr_player : NetworkBehaviour
{
    public NetworkVariable<int> water = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
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
        //gameObject.AddComponent<Camera>();
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        GUI.ChangeCardCount(Deck.Count);
        water.Value = 10;
        GUI.UpdateWater(water.Value);
        foreach (scr_card card in Deck)
        {
            card.player = this;
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
        water.Value -= card.cost;
        GUI.UpdateWater(water.Value);
    }
}
