// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using GlobalDefine;
using UnityEngine.Serialization;

[Serializable]
public class GUIPreset : MonoBehaviour {
    public Canvas rootCanvas;    
    public Text endingMsg;

    [Header("Taking Info")]
    public Transform bubble;
    public Text bubbleMsg;
    [FormerlySerializedAs("Pivot")] public Vector3 pivot = new Vector3(20.0f, 170.0f, 0.0f);

    [Header("Status Info")]
    public Slider madness;

    [Header("Inventory Info")]
    public Image item1;
    public Image item2;
    public Image item3;
       
    [Header("Customize Info")]
    public Transform customize;

    public void Initialize() {
        HideEnding();
        HideBubble();
        SetMadness(0.0f);

        // items
        item1.gameObject.SetActive(false);
        item2.gameObject.SetActive(false);
        item3.gameObject.SetActive(false);

        customize.gameObject.SetActive(true);
    }


    public void ShowEnding() {
        endingMsg.gameObject.SetActive(true);
    }


    public void HideEnding() {
        endingMsg.gameObject.SetActive(false);
    }


    public void ShowBubble(Vector3 inPos, string inMsg = "...") {
        bubble.gameObject.SetActive(true);
        bubble.position = inPos + pivot;
        bubbleMsg.text = inMsg;
    }


    public void HideBubble() {
        bubble.gameObject.SetActive(false);
    }


    public void SetMadness(float inValue) {
        madness.value = inValue;
    }
}
