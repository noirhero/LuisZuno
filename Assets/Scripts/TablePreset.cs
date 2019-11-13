// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public struct ItemPresetData {
    public Sprite sprite;
    public int madness;
}

[Serializable]
public class ItemPresetDataDictionary : SerializableDictionaryBase<Int64, ItemPresetData> {
}

[Serializable]
public class TablePreset : MonoBehaviour {
    [Header("Item Table")]
    public ItemPresetDataDictionary itemDatas = new ItemPresetDataDictionary();
}
