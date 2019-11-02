// Copyrighgt 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using GlobalDefine;

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
    public Image item1;
    public Image item2;
    public Image item3;


    public void Initialize() {
        HideEnding();
        HideBubble();
        SetMadness(0.0f);

        // items
        item1.gameObject.SetActive(false);
        item2.gameObject.SetActive(false);
        item3.gameObject.SetActive(false);
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
}
