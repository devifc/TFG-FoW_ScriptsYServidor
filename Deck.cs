using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Deck : MonoBehaviour
{
    public static Stack<Card> cards = new Stack<Card>();
    public static Stack<Card> ruler = new Stack<Card>();
    public static Stack<Card> stonesDeck = new Stack<Card>();
    public static int cardsInDeck = 40;
    public GameObject prefab;
    private static bool completed = false;
    private static bool stonesCompleted = false;
    private static bool rulerCompleted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CardDBAccess.cards != null && CardDBAccess.cards.Count > 100)
        {
            if (completed == false)
            {
                //Introducing 4 copies
                for (int j = 0; j < 4; j++)
                {                                       //$"ADW-0{UnityEngine.Random.Range(10, 99)}")
                    cards.Push(CardDBAccess.GetCardByName($"Gradius"));//                   x4
                    cards.Push(CardDBAccess.GetCardByName($"Everfrost"));//                 x4
                    cards.Push(CardDBAccess.GetCardByName($"Atomic Bahamut"));//            x4
                    cards.Push(CardDBAccess.GetCardByName($"Charlotte's Protector"));//     x4
                    cards.Push(CardDBAccess.GetCardByName($"Brave Force"));//               x4
                    cards.Push(CardDBAccess.GetCardByName($"Tiny Violet"));//               x4
                }
                //Introducing 3 copies
                for (int j = 0; j < 3; j++)
                {
                    cards.Push(CardDBAccess.GetCardByName($"Improved Healing Robot"));//    x3
                }
                //Introducing 2 copies
                for (int j = 0; j < 2; j++)
                {
                    cards.Push(CardDBAccess.GetCardByName($"Aegis"));//                     x2
                    cards.Push(CardDBAccess.GetCardByName($"Eternal Wind"));//              x2
                    cards.Push(CardDBAccess.GetCardByName($"Dark Prominence"));//           x2
                    cards.Push(CardDBAccess.GetCardByName($"Magical Loveliness"));//        x2
                    cards.Push(CardDBAccess.GetCardByName($"Void"));//                      x2
                    cards.Push(CardDBAccess.GetCardByName($"White Garden"));//              x2
                }
                //Introducing 1 copy
                cards.Push(CardDBAccess.GetCardByName($"Deathscythe, the Life Reaper"));//  x1
                
                if (cards.Count == 40)
                {
                    completed = true;
                }
            }

            if (stonesCompleted == false)
            {
                //Introducing 2 copies
                for (int j = 0; j < 2; j++)
                {
                    stonesDeck.Push(CardDBAccess.GetCardByName($"Magic Stone of Hearth's Core"));//      x2
                }
                //Introducing 4 copies
                for (int j = 0; j < 4; j++)
                {
                    stonesDeck.Push(CardDBAccess.GetCardByName($"Magic Stone of Atoms"));//      x4
                    stonesDeck.Push(CardDBAccess.GetCardByName($"Magic Stone of Flame"));//      x4
                }

                if (stonesDeck.Count == 10)
                {
                    stonesCompleted = true;
                }
            }

            if (rulerCompleted == false)
            {
                ruler.Push(CardDBAccess.GetCardByName($"Violet, Atomic Automaton [J-Ruler]"));//    x1 //judgement form
                ruler.Push(CardDBAccess.GetCardByName($"Violet, Atomic Automaton"));//              x1
                
                if (ruler.Count == 2)
                {
                    rulerCompleted = true;
                }
            }
        }
    }
    public void Shuffle() {
        //Convert to list and shuffle
        List<Card> list = cards.ToList();
        list.Sort();
        //Shuffling algorithm
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(1,i);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
        cards.Clear();
        foreach (Card card in list)
        {
            cards.Push((Card)card);
        }
        
    }
    public void Forsee(int numCards)
    {
        if (numCards == 1)
        {
            cards.Peek().backCard.SetActive(false);
        }
        else
        {
            foreach (Card card in cards.Take(numCards))
            {
                card.backCard.SetActive(false);
                Console.WriteLine("Peeked element: {0}", card.name);
            }
        }
    }

    public void PickCards(int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            cards.Pop();
        }
        foreach (Card card in cards.Take(numCards))
        {
            card.backCard.SetActive(false);
            Console.WriteLine("Peeked element: {0}", card.name);
        }
    }
}
