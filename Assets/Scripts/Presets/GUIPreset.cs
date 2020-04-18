// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

[Serializable]
public class GUIPreset : MonoBehaviour {
    public Canvas rootCanvas;    
    public Text endingMsg;

    [Header("Taking UI")]
    public BubbleUI bubble;
    
    [Header("Status UI")]
    public Slider madness;

    [Header("Inventory UI")] 
    public Transform inventory;
    public Image item1;
    public Image item2;
    public Image item3;
       
    [Header("Customize UI")]
    public Transform customize;

    [Header("SceneSelect UI")]
    public Transform sceneSelect;


    public void Initialize() {
    }


    public void ShowCustomize() {
        customize.gameObject.SetActive(true);
    }

    
    public void HideCustomize() {
        customize.gameObject.SetActive(false);
    }


    public void ShowScenarioSelect() {
        sceneSelect.gameObject.SetActive(true);
    }

    
    public void HideScenarioSelect() {
        sceneSelect.gameObject.SetActive(false);
    }


    public void ShowInventory() {
        inventory.gameObject.SetActive(true);

        // items
        item1.gameObject.SetActive(true);
        item2.gameObject.SetActive(true);
        item3.gameObject.SetActive(true);
    }


    public void HideInventory() {
        inventory.gameObject.SetActive(false);

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


    public void ShowBubble() {
        bubble.gameObject.SetActive(true);
    }


    public void HideBubble() {
        bubble.gameObject.SetActive(false);
    }


    public void ShowMadness(float inValue) {
        madness.gameObject.SetActive(true);
        madness.value = inValue;
    }


    public void HideMadness() {
        madness.gameObject.SetActive(false);
    }
}
