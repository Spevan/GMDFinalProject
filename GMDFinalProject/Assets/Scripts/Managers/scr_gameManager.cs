using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using System.Linq;

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
            }
        }
    }

    //Spawn a new network object based on parameters
    public void SpawnNetworkObj(string cardName, Vector3 pos, Quaternion rot)
    {
        //For each card...
        for(int i = 0; i < cardPrefabs.Count; ++i)
        {
            //...check if the sent name has the same as i's name
            if(cardPrefabs[i].name == cardName)
            {
                //Instantiate and spawn the network object of that card's unit value
                GameObject unit = Instantiate(cardPrefabs[i].unit, pos, rot);
                unit.GetComponent<NetworkObject>().Spawn(true);
            }
        }
    }

    //Request the server to...
    [ServerRpc(RequireOwnership = false)]
    public void SpawnNetworkObjServerRpc(string cardName, Vector3 pos, Quaternion rot)
    {
        //Spawn a new network obj based on parameters
        SpawnNetworkObj(cardName, pos, rot);
    }
}
