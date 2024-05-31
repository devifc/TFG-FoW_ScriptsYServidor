using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayfabInteractionManager : MonoBehaviour
{
    [SerializeField] TMP_InputField valueName;
    [SerializeField] TMP_InputField value;
    [SerializeField] CardDisplay item;
    [SerializeField] Transform scrollview;
    private void Start()
    {
        PlayfabData.GetUserData(
            OnGotDataSuccess,
            PlayfabFailure);
    }
    public void OnGoPressed()
    {
        PlayfabData.SaveData(new Dictionary<string, string>()
        {
            { valueName.text, value.text}
        },
        successResult =>
        {
            PlayfabData.GetUserData(
                OnGotDataSuccess,
                PlayfabFailure);

            print("Successfully Updated!");
        },
        PlayfabFailure);
    }
    void OnGotDataSuccess(GetUserDataResult result)
    {
        foreach (Transform t in scrollview)
        {
            Destroy(t.gameObject);
        }
        foreach (var info in result.Data)
        {
            var itemInfo = Instantiate(item, scrollview);
            itemInfo.nameText.text = info.Key;
        }
    }
    private void PlayfabFailure(PlayFabError error)
    {
        Debug.Log(error.Error + " : " + error.GenerateErrorReport());
    }
}
