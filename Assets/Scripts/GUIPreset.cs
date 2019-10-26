// Copyrighgt 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using RotaryHeart.Lib.SerializableDictionary;
using GlobalDefine;

[Serializable]
public struct ItemPresetData {
    public Sprite sprite;
}

[Serializable]
public class ItemPresetDataDictionary : SerializableDictionaryBase<Int64, ItemPresetData> {
}

[Serializable]
public class GUIPreset : MonoBehaviour {
    [Header("Inventory Info")]
    public GameObject inventory;
    public Image item0;
    public Image item1;
    public Image item2;

    [Header("Item Table")]
    public ItemPresetDataDictionary itemDatas = new ItemPresetDataDictionary();

    public void ShowItem0(ItemStruct inData) {
        item0.gameObject.SetActive(inData.IsVaild());
        item0.sprite = itemDatas[inData.id].sprite;
    }
    public void ShowItem1(ItemStruct inData) {
        item1.gameObject.SetActive(inData.IsVaild());
        item1.sprite = itemDatas[inData.id].sprite;
    }
    public void ShowItem2(ItemStruct inData) {
        item2.gameObject.SetActive(inData.IsVaild());
        item2.sprite = itemDatas[inData.id].sprite;
    }
}
