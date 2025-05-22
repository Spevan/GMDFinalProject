using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using System.Linq;

public class scr_gameManager : MonoBehaviour
{
    public static scr_gameManager instance {  get; private set; }

    [SerializeField] private List<Transform> playerSpawns;

    public GameObject player;
    public List<GameObject> players;

    List<NetworkClient> clients;
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

    [ServerRpc]
    public void SpawnNetworkObjServerRpc(GameObject unit, scr_player player)
    {
        unit.GetComponent<NetworkObject>().Spawn(true);
        unit.GetComponent<scr_heroUnit>().player = player;
    }
}
