using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using System.Linq;

public class scr_gameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawns;

    public GameObject player;
    public List<GameObject> players;

    List<NetworkClient> clients;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            clients = NetworkManager.Singleton.ConnectedClientsList.ToList();

            for (int i = 0; i < clients.Count; ++i)
            {
                players.Add(NetworkManager.Instantiate(player, playerSpawns[i].position, playerSpawns[i].rotation));
                players[i].GetComponent<NetworkObject>().SpawnWithOwnership(clients[i].ClientId);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
