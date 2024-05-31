using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class CardDBAccess : MonoBehaviour
{
    public static List<Card> cards = new List<Card>();
    public static int cardsCounter = 0;

    void Awake()
    {
        StartCoroutine(CopyAndLoadJSON());
    }

    IEnumerator CopyAndLoadJSON()
    {
        string fileName = "DB/cards.json";
        string sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);

        // Crear el directorio si no existe
        string destinationDir = Path.GetDirectoryName(destinationPath);
        if (!Directory.Exists(destinationDir))
        {
            Directory.CreateDirectory(destinationDir);
        }

        // Copiar el archivo desde StreamingAssets a persistentDataPath
        if (sourcePath.Contains("://") || sourcePath.Contains(":///"))
        {
            using (WWW www = new WWW(sourcePath))
            {
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    File.WriteAllBytes(destinationPath, www.bytes);
                }
                else
                {
                    Debug.LogError("Error al copiar el archivo: " + www.error);
                    yield break;
                }
            }
        }
        else
        {
            File.Copy(sourcePath, destinationPath, true);
        }

        // Leer el archivo desde persistentDataPath
        string jsonContent = File.ReadAllText(destinationPath);


        FOWData data = JsonConvert.DeserializeObject<FOWData>(jsonContent);

        //PROCESS DATA TO CREATE CARD OBJECTS

        foreach (var cluster in data.fow.clusters)
        {
            foreach (var set in cluster.sets)
            {
                foreach (var cardData in set.cards)
                {
                    cards.Add(cardData);
                }
            }
        }
        //ADDING IMAGES AND TEMPLATES TO EACH CARD
        foreach (var card in cards)
        {

            //ARTWORK

            string imageName = $"{card.name.ToLower()}.png"; //Name of the file. Format: [atomic bahamut.png]

            TextAsset fileNamesTextAsset = Resources.Load<TextAsset>("filelist");
            if (fileNamesTextAsset == null)
            {
                Debug.LogError("Failed to load file list.");
                yield break;
            }

            string[] fileNames = fileNamesTextAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            //LOOP FOUND FILES

            foreach (string file in fileNames)
            {
                string image = Path.GetFileNameWithoutExtension(imageName);
                Sprite sprite = Resources.Load<Sprite>($"{image}");
                if (imageName.Contains("violet, atomic automaton"))
                {
                    Debug.Log("image: " + image + " imageName: " + imageName + " -file: " + file);
                }
                if (file.Equals(imageName))
                {
                    if (sprite == null)
                    {
                        Debug.LogError($"Failed to load image: {image}");
                    }
                    else
                    {
                        card.artwork = Resources.Load<Sprite>(Path.GetFileNameWithoutExtension(imageName));

                        break; //Break loop if finds the file
                    }
                }

            }

            imageName = ""; //Reset to use the variable with another image
            int R = 0, B = 0, W = 0, U = 0, G = 0;
            string num = "";

            /*COSTS
            "0"
            {R} Red
            {B} Black/Purple
            {W} White/Yellow
            {U} Blue
            {G} Green
            {1},{2},{3} ... additional cost
             */

            /*COLOURS
            "R" Red
            "B" Black
            "W" White
            "U" Blue
            "G" Green
             */
            //ORDER: W>R>U>G>B
            for (int i = 0; i < card.cost.Length; i++)
            {
                if (card.cost[i] == ('W'))
                {
                    W++;
                }
                if (card.cost[i] == ('R'))
                {
                    R++;
                }
                if (card.cost[i] == ('U'))
                {
                    U++;
                }
                if (card.cost[i] == ('G'))
                {
                    G++;
                }
                if (card.cost[i] == ('B'))
                {
                    B++;
                }
            }

            string p = @"\d+";
            bool conNum = Regex.IsMatch(card.cost, p);

            if (card.cost != null && card.cost.Length >= 3 && conNum == true)
            {
                num = card.cost.Substring(card.cost.Length - 3, 2);
            }
            else
            {
                num = "0";
            }

            //SET WILL


            //HERE IT'S IMPORTANT THE ORDER W>R>U>G>B
            if (W != 0)
            {
                for (int i = 0; i < W; i++)
                {
                    imageName += "W";
                }
            }
            if (R != 0)
            {
                for (int i = 0; i < R; i++)
                {
                    imageName += "R";
                }
            }
            if (U != 0)
            {
                for (int i = 0; i < U; i++)
                {
                    imageName += "U";
                }
            }
            if (G != 0)
            {
                for (int i = 0; i < G; i++)
                {
                    imageName += "G";
                }
            }
            if (B != 0)
            {
                for (int i = 0; i < B; i++)
                {
                    imageName += "B";
                }
            }

            string pattern = @"\d+";
            bool containsNumbers = Regex.IsMatch(num, pattern);

            if (containsNumbers == true && !num.Equals("0"))
            {
                //EXTRA COST
                string splittedCost = "";

                if (num.Contains("{") || num.Contains("}"))
                {
                    splittedCost = num.Split("{")[1].Split("}")[0];
                }

                if (splittedCost.Equals("0"))
                {
                    card.extraCost = "0";
                }
                else
                {
                    card.extraCost = "" + splittedCost;
                }

                // 0  1  2
                // {  10  }
            }
            else
            {
                card.extraCost = "0";
            }
            imageName += ".png"; //example: RRR.png if it repeats 3 times the red colour

            fileNames = fileNamesTextAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in fileNames)
            {
                string image = Path.GetFileNameWithoutExtension(imageName);
                Sprite sprite = Resources.Load<Sprite>($"{image}");
                if (imageName.Contains("violet, atomic automaton"))
                {
                    Debug.Log("image: " + image + " imageName: " + imageName + " -file: " + file);
                }
                if (file.Equals(imageName))
                {
                    if (sprite == null)
                    {
                        Debug.LogError($"Failed to load image: {image}");
                    }
                    else
                    {
                        card.will = Resources.Load<Sprite>($"{Path.GetFileNameWithoutExtension(imageName)}");

                        break;
                    }
                }
                else
                {
                    sprite = Resources.Load<Sprite>($"0");
                    if (sprite != null)
                    {
                        card.will = sprite;
                    }

                }
            }
        }
    }

    public static Card GetCardById(string id)
    {
        foreach (var card in cards)
        {

            if (card.id.Equals(id))
            {
                return card;
            }
        }
        return null;
    }
    public static Card GetCardByName(string name)
    {
        foreach (var card in cards)
        {

            if (card.name.ToLower().Equals(name.ToLower()))
            {
                return card;
            }
        }
        return null;
    }
}
