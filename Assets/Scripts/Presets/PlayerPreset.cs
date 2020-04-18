// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using GlobalDefine;

[Serializable]
public class PlayerPrefabPathDictionary : SerializableDictionaryBase<BackgroundType, string> {
}


[Serializable]
public class PlayerPreset : MonoBehaviour {
    public PlayerPrefabPathDictionary prefabPaths;
}
