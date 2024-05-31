using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using Newtonsoft.Json;
//using static UnityEditor.Timeline.TimelinePlaybackControls;

public class FOWData 
{
    public Fow fow;//"fow": {
}
public class Fow
{
    public List<Clusters> clusters;//"clusters": [
}

public class Clusters/*{
                       "name": 
                       "Valhalla",
                       "sets": [    */
{
    public string name;
    public List<Set> sets;//"sets": [
}

public class Set/*{
						"name": "Starter",
						"code": "S",
						"cards": [  */
{
    public string name;
    public string code;
    public List<Card> cards;
}

//[CreateAssetMenu(fileName = "Card", menuName = "Card Template")]
public class Card //: ScriptableObject// : Monobehaviour
{
    public string id;
    public string name;
    public List<string> type;
    public List<string> race;
    public string cost;
    public List<string> colour;
    public string ATK;
    public string DEF;
    public List<string> abilities;
    public string divinity;
    public string flavour;
    public string artist;
    public string rarity;
    public string fowID;
    public string extraCost; //It shows the extra cost behind the will (cost that is not binded to a colour)

    //public GameObject stats; //It contains the ATK and DEF template
    //public GameObject chantBackground;
    public GameObject backCard;
    public Sprite will; //Image that shows the will (cost with colour)
    public Sprite artwork;
    public bool expanded = false;
    public bool cardBack = false;
    /*
    public Card(string cardID)
    {
        id = cardID;
        
    }
    */
}

