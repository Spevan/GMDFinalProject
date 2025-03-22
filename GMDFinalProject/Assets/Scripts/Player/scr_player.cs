using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class scr_player : MonoBehaviour
{
    public int water;
    public List<scr_card> Deck;
    public GameObject ProductionPlant;
    public Camera main;
    public scr_guiManager GUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.AddComponent<Camera>();
        SceneManager.LoadScene("sce_gui", LoadSceneMode.Additive);
        //DrawCard();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetSceneByName("sce_gui") != null && GUI == null)
        {
            GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        }
    }

    public void DrawCard()
    {
        GUI.DrawCard(Deck[0]);
        Deck.RemoveAt(0);
    }
}
