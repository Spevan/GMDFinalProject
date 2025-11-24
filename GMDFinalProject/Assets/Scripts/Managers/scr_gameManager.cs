using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.CanvasScaler;

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
    public List<NetworkClient> clients;

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

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnConnectedToServer()
    {
        Debug.Log("Connected to server");
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //If this is the server
        //NetworkManager.Singleton.StartHost();

        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("GameManager Initialized");
            //Create client list
            clients = NetworkManager.Singleton.ConnectedClientsList.ToList();
            foreach (var client in clients)
            {
                Debug.Log("The following clients are connected to the server: ");
                Debug.Log(client.ToString());
            }

            //For each client, create a player list
            //if (scr_dataPersistenceManager.instance != null)
            {
                for (int i = 0; i < clients.Count; ++i)
                {
                    GameObject tempPlayer = Instantiate(player, playerSpawns[i].position, playerSpawns[i].rotation);
                    tempPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(clients[i].ClientId);
                    //players.Add(tempPlayer);
                }

               /* int count = 0;
                foreach(GameObject player in players)
                {
                    count++;
                    player.GetComponent<scr_player>().ProductionPlant = scr_dataPersistenceManager.instance.playerData.equippedDeck.productionPlant;
                    GameObject obj = NetworkManager.Instantiate(player.GetComponent<scr_player>().ProductionPlant.unit,
                        player.GetComponent<scr_player>().plantPrefab.transform.position, new Quaternion(0, playerSpawns[count].rotation.y, 0, 0));
                    obj.GetComponent<scr_generatorUnit>().cardData = scr_dataPersistenceManager.instance.playerData.equippedDeck.productionPlant;
                    obj.GetComponent<NetworkObject>().SpawnWithOwnership(clients[count].ClientId);
                }*/
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
                GameObject unit = NetworkManager.Instantiate(cardPrefabs[i].unit, pos, rot);
                unit.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
                Debug.Log("Spawned " +  cardName);
                /*if (unit.tag.Equals("Generator") && unit.GetComponent<NetworkObject>().IsSpawned)
                {
                    SpawnGeneratorClientRpc(unit, i, clientID);
                }*/
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

    /*[ClientRpc]
    public void SpawnGeneratorClientRpc(NetworkObjectReference unitRef, int cardPrefab, ulong clientID)
    {
        if (unitRef.TryGet(out NetworkObject unit))
        {
            Debug.Log("Assigning card data: " + cardPrefabs[cardPrefab].name + " to " + unit.name);
            unit.GetComponent<scr_generatorUnit>().cardData = cardPrefabs[cardPrefab];
            foreach (var player in players)
            {
                if (player.GetComponent<NetworkObject>().OwnerClientId == clientID)
                {
                    unit.GetComponent<scr_generatorUnit>().GetTarget(player);
                }
            }
        }
        else
        {
            Debug.Log("Could not get data for this generator.");
        }
    }*/

    [ServerRpc (RequireOwnership = false)]
    public void EndGameServerRpc(ulong losingClient)
    {
        for(int i = 0; i < clients.Count; ++i)
        {
            if (clients[i].ClientId == losingClient)
            {
                players[i].GetComponent<scr_player>().LoseGameClientRpc();
            }
            else
            {
                players[i].GetComponent<scr_player>().WinGameClientRpc();
            }
        }
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    }

    void OnClientDisconnect(ulong clientId)
    {
        if (this.gameObject.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.ServerClientId)
        {
            Debug.Log("You are now disconnecting the server");
            //SceneManager.LoadScene("sce_mainMenu");
        }
    }

    [ClientRpc (RequireOwnership = false)]
    void HostDisconnectedClientRpc()
    {
        Debug.Log("Host Disconnected");
        
        Destroy(NetworkManager.Singleton.gameObject);
    }
}
