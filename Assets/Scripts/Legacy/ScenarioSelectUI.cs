// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.Events;
using  GlobalDefine;

public class ScenarioSelectUI : LegacyUI {
    public Button btnPreset;
    public float teleportTime;
    public float fadeInOutTime;
    public List<ScenarioSelectInfo> scenarios;
    public Dictionary<int, RectTransform> btns = new Dictionary<int, RectTransform>();


    private void Start() {
        for (int i=0; i< scenarios.Count; ++i) {
            Button cachedBtn = GameObject.Instantiate<Button>(btnPreset);
            
            UnityAction<int> action = Delegate.CreateDelegate(typeof(UnityAction<int>), this, "OnSelectedInScenario") as UnityAction<int>;
            UnityEventTools.AddIntPersistentListener(cachedBtn.onClick, action, i);

            Text cachedText = cachedBtn.GetComponentInChildren<Text>();
            cachedText.text = scenarios[i].GetDisplayName();

            RectTransform cachedTrans = cachedBtn.GetComponentInChildren<RectTransform>();
            cachedTrans.SetParent(this.transform);
            btns.Add(i, cachedTrans);
        }
    }


    private void Update() {
        foreach (var btn in btns) {
            var index = btn.Key;
            var pivot = scenarios[btn.Key].GetPivot();
            var convert2DPos = Camera.main.WorldToScreenPoint(pivot);
            
            var btnTrans = btn.Value;
            btnTrans.position = convert2DPos;
        }
    }


    public void OnSelectedInScenario(int inType) {
        var guiComp = Utility.entityMng.GetComponentData<GUIComponent>(Utility.playerEntity);
        guiComp.currentUI ^= GUIState.scenarioSelect;
        Utility.entityMng.SetComponentData(Utility.playerEntity, guiComp);
        
        Utility.entityMng.AddComponentData(Utility.playerEntity, new TeleportInfoComponent(
            scenarios[inType].sceneType, scenarios[inType].curSubSceneType, scenarios[inType].nextSubSceneType, scenarios[inType].pointID, teleportTime, fadeInOutTime));
    }
}
