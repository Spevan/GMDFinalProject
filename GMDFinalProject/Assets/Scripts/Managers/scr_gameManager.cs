using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class scr_gameManager : NetworkBehaviour
{
    //Sets singleton instance
    public static scr_gameManager instance {  get; private set; }
    //List of player spawns locations to add to
    [SerializeField] private List<Transform> playerSpawns;
    //Player prefab
    public GameObject player;
    //List of active players in scene
    public List<GameObject> players;
    //List of all card data
    public List<scr_card> cardPrefabs;
    //List of all ammo data
    public List<GameObject> ammoPrefabs;
    //List of all clients connected
    List<NetworkClient> clients;

    //Ensuring that this is the only singleton instance
    private void Awake()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //If this is the server
        if (NetworkManager.Singleton.IsServer)
        {
            //Create client list
            clients = NetworkManager.Singleton.ConnectedClientsList.ToList();

            //For each client, create a player list
            for (int i = 0; i < clients.Count; ++i)
            {
                players.Add(NetworkManager.Instantiate(player, playerSpawns[i].position, playerSpawns[i].rotation));
                players[i].GetComponent<NetworkObject>().SpawnWithOwnership(clients[i].ClientId);

                players[i].GetComponent<scr_player>().CreateProductionPlant(scr_dataPersistenceManager.instance.playerData.equippedDeck.productionPlant);
                foreach (scr_card card in scr_dataPersistenceManager.instance.playerData.equippedDeck.cardsInDeck)
                {
                    players[i].GetComponent<scr_player>().Deck.Add(card);
                }
            }
        }
    }

    //Spawn a new network object based on parameters
    public void SpawnNetworkCard(string cardName, Vector3 pos, Quaternion rot, ulong clientID)
    {
        //For each card...
        for(int i = 0; i < cardPrefabs.Count; ++i)
        {
            //...check if the sent name has the same as i's name
            if(cardPrefabs[i].name == (cardName))
            {
                //Instantiate and spawn the network object of that card's unit value
                GameObject unit = Instantiate(cardPrefabs[i].unit, pos, rot);
                unit.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
            }
        }
    }

    //Request the server to...
    [ServerRpc(RequireOwnership = false)]
    public void SpawnNetworkCardServerRpc(string cardName, Vector3 pos, Quaternion rot, ulong clientID)
    {
        //Spawn a new network obj based on parameters
        SpawnNetworkCard(cardName, pos, rot, clientID);
    }
}
