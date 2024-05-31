using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class PlayfabData
{
    static Dictionary<string, UserDataRecord> userData;
    static bool isGettingUserData = false;

    public static void SaveData(Dictionary<string, string> Data,
        Action<UpdateUserDataResult> onSuccess,
        Action<PlayFabError> onFail)
        
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = Data
        },
        successResult => 
        {
            if (userData != null)
                foreach (var key in Data.Keys)
                {
                    UserDataRecord Value = new() { Value = Data[key] };

                    if(userData.ContainsKey(key))
                    { 
                        userData[key] = Value;
                    }   
                    else
                    {
                        userData.Add(key, Value);
                    }
                }
            onSuccess(successResult);
        },
        onFail);
    }

    public static void GetUserData(
        Action<GetUserDataResult> onSuccess,
        Action<PlayFabError> onFail)
    {
        while (isGettingUserData)
        {
            Task.Delay(100);
        }

        if (userData != null)
        {
            onSuccess(new GetUserDataResult() { Data = userData });
            return;
        }

        isGettingUserData = true;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            onSuccessResult => 
            {
                userData = onSuccessResult.Data;
                isGettingUserData = false;
                onSuccess(onSuccessResult);
            }, 
            onFailResult =>
            {
                isGettingUserData = false;
                onFail(onFailResult);
            });
    }
}
