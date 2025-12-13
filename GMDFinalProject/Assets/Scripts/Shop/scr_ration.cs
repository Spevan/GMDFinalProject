using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Ration Ticket", menuName = "Scriptable Sheets/Ration Ticket")]
public class scr_ration : ScriptableObject
{
    public List<scr_card> cards;
}
