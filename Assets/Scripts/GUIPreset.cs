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
    public Canvas rootCanvas;    
    public Text endingMsg;

    [Header("Taking Info")]
    public Transform bubble;
    public Text bubbleMsg;

    [Header("Status Info")]
    public Slider madness;

    [Header("Inventory Info")]
    public Image item0;
    public Image item1;
    public Image item2;

    [Header("Item Table")]
    public ItemPresetDataDictionary itemDatas = new ItemPresetDataDictionary();

    public void Initialize() {
        HideEnding();
        HideBubble();
        SetMadness(0.0f);

        // items
        item0.gameObject.SetActive(false);
        item1.gameObject.SetActive(false);
        item2.gameObject.SetActive(false);
    }

    public void ShowEnding() {
        endingMsg.gameObject.SetActive(true);
    }

    public void HideEnding() {
        endingMsg.gameObject.SetActive(false);
    }

    public void ShowBubble(Vector3 inPos, string inMsg = "...") {
        bubble.gameObject.SetActive(true);
        bubbleMsg.text = inMsg;
    }

    public void HideBubble() {
        bubble.gameObject.SetActive(false);
    }

    public void SetMadness(float inValue) {
        madness.value = inValue;
    }

    public void SetItem0(ItemStruct inData) {
        ItemPresetData data;
        if (false == itemDatas.TryGetValue(inData.id, out data))
            return;

        item0.gameObject.SetActive(inData.IsVaild());
        item0.sprite = data.sprite;
    }

    public void SetItem1(ItemStruct inData) {
        ItemPresetData data;
        if (false == itemDatas.TryGetValue(inData.id, out data))
            return;

        item1.gameObject.SetActive(inData.IsVaild());
        item1.sprite = data.sprite;
    }

    public void SetItem2(ItemStruct inData) {
        ItemPresetData data;
        if (false == itemDatas.TryGetValue(inData.id, out data))
            return;

        item2.gameObject.SetActive(inData.IsVaild());
        item2.sprite = data.sprite;
    }
}
