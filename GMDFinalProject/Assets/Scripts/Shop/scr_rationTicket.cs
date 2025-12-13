using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class scr_rationTicket : MonoBehaviour
{
    public scr_ration ticketList;
    public GameObject smlRationPrefab, medRationPrefab, lrgRationPrefab;
    public List<Transform> ticketSpawns;
    List<scr_rationSpawn> rationSpawns = new List<scr_rationSpawn>();

    private void Start()
    {
        foreach(Transform ticketSpawn in ticketSpawns)
        {
            rationSpawns.Add(ticketSpawn.gameObject.GetComponent<scr_rationSpawn>());
        }

        ticketList.cards = ticketList.cards.OrderByDescending(c => c.rarity).ToList();

        GenerateTicket();
    }

    public void GenerateTicket()
    {
        foreach (scr_card card in ticketList.cards)
        {
            switch(card.rarity)
            {
                case scr_card.rarityType.Common:
                case scr_card.rarityType.Uncommon:
                    int count = 0;
                    foreach (scr_rationSpawn ticketSpawn in rationSpawns)
                    {
                        Debug.Log(count);
                        if (!ticketSpawn.spawned)
                        {
                            ticketSpawn.SpawnRation(Instantiate(smlRationPrefab, ticketSpawn.gameObject.transform), card);
                            break;
                        }
                        count++;
                    }
                    break;
                case scr_card.rarityType.Rare:
                    count = 0;
                    foreach (scr_rationSpawn ticketSpawn in rationSpawns)
                    {
                        Debug.Log(count);
                        if (!ticketSpawn.spawned && count > 0 && count < rationSpawns.Count - 1)
                        {
                            if (!rationSpawns[count - 1].spawned && !rationSpawns[count - 1].corner)  
                            {
                                ticketSpawn.SpawnRation(Instantiate(medRationPrefab, ticketSpawn.gameObject.transform.position - new Vector3(ticketSpawn.gameObject.GetComponent<RectTransform>().offsetMin.x, 0, 0), Quaternion.identity, ticketSpawn.gameObject.transform), card);
                                rationSpawns[count - 1].spawned = true;
                                break;
                            }
                            else if(!rationSpawns[count + 1].spawned && !ticketSpawn.corner)
                            {
                                ticketSpawn.SpawnRation(Instantiate(medRationPrefab, ticketSpawn.gameObject.transform.position + new Vector3(ticketSpawn.gameObject.GetComponent<RectTransform>().offsetMax.x, 0, 0), Quaternion.identity, ticketSpawn.gameObject.transform), card);
                                rationSpawns[count + 1].spawned = true;
                                break;
                            }
                        }
                        count++;
                    }
                    break;
                case scr_card.rarityType.Legendary:
                    count = 0;
                    foreach (scr_rationSpawn ticketSpawn in rationSpawns)
                    {
                        Debug.Log(count);
                        if (!ticketSpawn.spawned && count > 0 && count < rationSpawns.Count - 1)
                        {
                            if (!rationSpawns[count - 1].spawned && !rationSpawns[count + 1].spawned && !ticketSpawn.corner)
                            {
                                ticketSpawn.SpawnRation(Instantiate(lrgRationPrefab, ticketSpawn.gameObject.transform), card);
                                rationSpawns[count - 1].spawned = true;
                                rationSpawns[count + 1].spawned = true;
                                break;
                            }
                        }
                        count++;
                    }
                    break;
            }
        }

    }

    public void RedeemCard()
    {

    }
}
