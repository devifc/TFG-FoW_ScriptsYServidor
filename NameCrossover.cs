using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameCrossover : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Text playernameTextHolder;
    bool updatedName = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateName();
    }
    private void UpdateName()
    {
        //if we dont have a game manager
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        //if we have a game manager
        if (gameManager != null && updatedName == false)
        {
            playernameTextHolder.text = gameManager.playerName;
            print("Hello");
            updatedName = true;
        }
    }
}
