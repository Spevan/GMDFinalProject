using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using System.Linq;

public class scr_gameManager : NetworkManager
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
    public void SpawnNetworkObjServerRpc(scr_card cardData, scr_player player, RaycastHit hit)
    {
        //if (IsServer)
        {
            GameObject unit = Instantiate(cardData.unit, hit.point, new Quaternion(0, player.transform.rotation.y, 0, 0));
            unit.GetComponent<scr_heroUnit>().player = player;
            unit.GetComponent<NetworkObject>().Spawn(true);
            
        }
    }
}
