// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;

[Serializable]
public class GUIPreset : MonoBehaviour {
    public Canvas rootCanvas; 
    
    public EndingUI ending;
    public BubbleUI bubble;
    public MadnessUI madness;
    public InventoryUI inventory;
    public CustomizeUI customize;
    public ScenarioSelectUI scenarioSelect;


    public void Initialize() {
        if (null != ending) {
            ending.Initialize();
        }
        if (null != bubble) {
            bubble.Initialize();
        }
        if (null != madness) {
            madness.Initialize();
        }
        if (null != inventory) {
            inventory.Initialize();
        }
        if (null != customize) {
            customize.Initialize();
        }
        if (null != scenarioSelect) {
            scenarioSelect.Initialize();
        }
    }


    public void Show() {
        rootCanvas.gameObject.SetActive(true);
    }
    
    
    public void Hide() {
        rootCanvas.gameObject.SetActive(false);
    }
}
