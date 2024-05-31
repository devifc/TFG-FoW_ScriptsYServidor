using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.XR;

public class CardDisplay : MonoBehaviour
{
    public GameObject prefab; //Prefab 
    public float scaleFactor = 3.5f; //Big scale
    public int rotationFactor = 270;
    public bool expanded = false;
    public Vector3 _previousPosition;
    public Vector3 _previousScale;
    public Quaternion _previousRotation;
    public int _previousOrder;
    public static bool alreadyOneExpansion = false;

    public GameObject stats; //Image that shows the ATK and DEF if the card have it
    public GameObject chantBackground; //If type is "Chant" it will show this Image
    public Text nameText; //name
    public Text raceText; //If card has a race, it will show it
    public Text cardTypeText; //Type. Resonator, chant, ruler...
    public Text abilitiesText; //description of the card
    public Image artWork; //artwork
    public Image willCost; //Image displayed depending on the cost of the card
    public Text extraCost; //Extra cost of the card (cost that is a number)
    public Text attackText; //ATK
    public Text defenseText; //DEF

    public bool cardBack;
    public int numberOfCardsInDeck;
    public Card displayCard;
    public string cardExtraCost;//It shows the extra cost behind the will
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
    public GameObject backCard;
    public Sprite will;//image that shows the will
    public Sprite artwork;

    public enum CardStatus { InDeck, InHand, OnBoard, OnStonesBoard, InDb, OnPile, InGY, InExile, IsRuler, InStonesDeck, Destroyed };
    public CardStatus cardStatus;
    public int numCardsToLoad;

    //public static int remainingCardInDeck = Deck.cardsInDeck;
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "Database")
        {
            cardStatus = CardStatus.InDb;
        }
        else//if(SceneManager.GetActiveScene().name == "Game")
        {
            var parentComponent = GetComponentInParent<Deck>();

            switch (parentComponent.name)
            {
                case "PlayerDeck":
                    cardStatus = CardStatus.InDeck;
                    break;
                case "PlayerHand":
                    cardStatus = CardStatus.InHand;
                    break;
                case "PlayerResonatorBoard":
                    cardStatus = CardStatus.OnBoard;
                    break;
                case "PlayerStonesBoard":
                    cardStatus = CardStatus.OnStonesBoard;
                    break;
                case "Pile(SpellZone)":
                    cardStatus = CardStatus.OnPile;
                    break;
                case "PlayerGraveyard":
                    cardStatus = CardStatus.InGY;
                    break;
                case "PlayerExile":
                    cardStatus = CardStatus.InExile;
                    break;
                case "PlayerRulerArea":
                    cardStatus = CardStatus.IsRuler;
                    break;
                case "PlayerStonesDeck":
                    cardStatus = CardStatus.InStonesDeck;
                    break;
                default:
                    break;
            }
        }

        switch (cardStatus)
        {
            case CardStatus.InDeck:
                numCardsToLoad = Deck.cards.Count;
                break;
            case CardStatus.InHand:
                numCardsToLoad = 1;
                break;
            case CardStatus.OnBoard:
                numCardsToLoad = 1;
                break;
            case CardStatus.OnPile:
                numCardsToLoad = 1;
                break;
            case CardStatus.InGY:
                numCardsToLoad = 1;
                break;
            case CardStatus.InExile:
                numCardsToLoad = 1;
                break;
            case CardStatus.IsRuler:
                numCardsToLoad = Deck.ruler.Count;
                break;
            case CardStatus.InStonesDeck:
                numCardsToLoad = Deck.stonesDeck.Count;
                break;
            default:
                numCardsToLoad = 1; //puedo borrar las anteriores si funcionan de la misma forma y dejar solo el default
                break;
        }


        //Card card = new Card();
    }

    void Update()
    {
        if (Deck.cards.Count > 0 && cardStatus == CardStatus.InDeck)
        {
            showCard(cardStatus);
        }
        if (Deck.stonesDeck.Count > 0 && cardStatus == CardStatus.InStonesDeck)
        {
            showCard(cardStatus);
        }
        if (Deck.ruler.Count > 0 && cardStatus==CardStatus.IsRuler)
        {
            showCard(cardStatus);
        }
    }
    public void showCard(CardStatus _cardStatus) 
    {
        Card valueCard = new Card();
        bool success = false; 
        switch (_cardStatus)
        {
            case CardStatus.InDeck:
                success = Deck.cards.TryPop(out valueCard);
                break;
            case CardStatus.InStonesDeck:
                success = Deck.stonesDeck.TryPop(out valueCard);
                break;
            case CardStatus.IsRuler:
                success = Deck.ruler.TryPop(out valueCard);
                break;
            default:
                break;
        }
        
        if (success)
        {
            displayCard = valueCard;

            id = displayCard.id;
            name = displayCard.name;
            race = displayCard.race;
            cost = displayCard.cost;
            colour = displayCard.colour;
            type = displayCard.type;
            ATK = displayCard.ATK;
            DEF = displayCard.DEF;
            abilities = displayCard.abilities;
            divinity = displayCard.divinity;
            flavour = displayCard.flavour;
            artist = displayCard.artist;
            rarity = displayCard.rarity;
            fowID = displayCard.fowID;
            artwork = displayCard.artwork;
            will = displayCard.will;
            cardExtraCost = displayCard.extraCost;

            //SET EMPTY
            nameText.text = "";
            raceText.text = "";
            cardTypeText.text = "";
            abilitiesText.text = "";
            attackText.text = "";
            defenseText.text = "";
            //NAME
            nameText.text = " " + name;
            //ABILITIES
            for (int i = 0; i < abilities.Count; i++)
            {
                abilitiesText.text += " " + abilities[i];
            }
            //RACE
            for (int i = 0; i < race.Count; i++)
            {
                raceText.text += " " + race[i];
            }
            //TYPE
            for (int i = 0; i < type.Count; i++)
            {
                string capsType = type[i].ToUpper();
                cardTypeText.text += "" + type[i];
                
                if (capsType.Contains("RESONATOR"))
                {
                    //ATK and DEF template
                    //ATK & DEF
                    chantBackground.SetActive(false);
                    stats.SetActive(true);
                    attackText.text = "" + ATK;
                    defenseText.text = "" + DEF;
                }
                else
                {
                    //template without ATK and DEF
                    chantBackground.SetActive(true);
                    stats.SetActive(false);
                }
            }
            artWork.sprite = displayCard.artwork;
            extraCost.text = displayCard.extraCost;
            willCost.sprite = displayCard.will;

            if (//_cardStatus == CardStatus.InDeck ||
                //_cardStatus == CardStatus.InStonesDeck ||
                (_cardStatus == CardStatus.IsRuler && displayCard.name.ToLower().Contains("j-ruler")))
            {
                cardBack = true;
                if (cardBack == true)
                {
                    backCard.SetActive(true);
                }
            }
            else
            {
                cardBack = false;
                backCard.SetActive(false);
            }

        }//END_ IF deck has cards
    }
    public void ExpandCard()
    {
        if (alreadyOneExpansion == false && displayCard.cardBack == false)//IF ITS FACEDOWN, IT WONT EXPAND
        {
            if (displayCard.expanded == false && displayCard.cardBack == false)
            {
                _previousPosition = prefab.transform.position;
                _previousScale = prefab.transform.localScale;
                _previousRotation = prefab.transform.rotation;

                Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
                Quaternion rotation = new Quaternion(0, 0, rotationFactor, rotationFactor);

                prefab.transform.position = new Vector3(worldPosition.x, worldPosition.y, 100);
                prefab.transform.rotation = new Quaternion(0, 0, 0, rotation.w);
                prefab.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

                displayCard.expanded = true;
                alreadyOneExpansion = true;
            }
        }
        else if (displayCard.expanded == true && alreadyOneExpansion == true)
        {
            prefab.transform.position = _previousPosition;
            prefab.transform.rotation = _previousRotation;
            prefab.transform.localScale = _previousScale;
            displayCard.expanded = false;
            alreadyOneExpansion = false;
        }

    }
}
