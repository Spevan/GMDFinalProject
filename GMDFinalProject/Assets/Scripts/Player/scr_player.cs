using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class scr_player : MonoBehaviour
{
    public int water;
    public List<scr_card> Deck;
    public GameObject ProductionPlant;
    public Camera main;
    public scr_guiManager GUI;
    int timeRemain;

    private void Awake()
    {
        SceneManager.LoadScene("sce_gui", LoadSceneMode.Additive);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.AddComponent<Camera>();
        GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        GUI.ChangeCardCount(Deck.Count);
        GUI.UpdateWater(water);
        foreach (scr_card card in Deck)
        {
            card.player = this;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(timeRemain <= 0)
        {
            timeRemain -= (int)Time.deltaTime;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (Deck.Count > 0)
        {
            GUI.DrawCard(Deck[0]);
            Deck.RemoveAt(0);
            GUI.ChangeCardCount(Deck.Count);
        }
    }

    public void PlayCard(scr_card card)
    {
        water -= card.cost;
        GUI.UpdateWater(water);
    }
}
