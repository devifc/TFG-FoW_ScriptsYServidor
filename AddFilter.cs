using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddFilter : MonoBehaviour
{
    public GameObject attribute;
    public Dropdown dpattribute;

    public GameObject race;
    public Dropdown dprace;
    
    public GameObject totalCost;
    public Dropdown dptotalCost;

    public GameObject set;
    public Dropdown dpset;

    public GameObject rarity;
    public Dropdown dprarity;

    public GameObject type;
    public Dropdown dptype;

    public GameObject keyword;
    public Dropdown dpkeyword;

    private string dpName;

    public void OnItemSelect(int index)
    {
        this.dpName = GetComponentInChildren<Dropdown>().name;
        Debug.Log(dpName);

        switch (dpName)
        {
            case "Attribute":
                if (index == 0)
                {
                    attribute.SetActive(false);
                }
                else
                {
                    attribute.GetComponentInChildren<Text>().text = dpattribute.options[index].text;
                    Debug.Log(dpattribute.options[index].text);
                    Debug.Log(attribute.GetComponentInChildren<Text>().text);
                }
                break;
            case "Race":
                if (index == 0)
                {
                    race.SetActive(false);
                }
                else
                {
                    race.GetComponentInChildren<Text>().text = dprace.options[index].text;
                    Debug.Log(dprace.options[index].text);
                }
                break;
            case "TotalCost":
                if (index == 0)
                {
                    totalCost.SetActive(false);
                }
                else
                {
                    totalCost.GetComponentInChildren<Text>().text = dptotalCost.options[index].text;
                    Debug.Log(dptotalCost.options[index].text);
                }
                break;
            case "Set":
                if (index == 0)
                {
                    set.SetActive(false);
                }
                else
                {
                    set.GetComponentInChildren<Text>().text = dpset.options[index].text;
                    Debug.Log(dpset.options[index].text);
                }
                break;
            case "Rarity":
                if (index == 0)
                {
                    rarity.SetActive(false);
                }
                else
                {
                    rarity.GetComponentInChildren<Text>().text = dprarity.options[index].text;
                    Debug.Log(dprarity.options[index].text);
                }
                break;
            case "Type":
                if (index == 0)
                {
                    type.SetActive(false);
                }
                else
                {
                    type.GetComponentInChildren<Text>().text = dptype.options[index].text;
                    Debug.Log(dptype.options[index].text);
                }
                break;
            case "Keyword":
                if (index == 0)
                {
                    keyword.SetActive(false);
                }
                else
                {
                    keyword.GetComponentInChildren<Text>().text = dpkeyword.options[index].text;
                    Debug.Log(dpkeyword.options[index].text);
                }
                break;
        }
    }
}
