// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Unity.Entities;
using UnityEditor.Events;
using  GlobalDefine;

public class ScenarioSelectUI : LegacyUI {
    public Button btnPreset;
    [FormerlySerializedAs("TeleportTime")] public float teleportTime;
    [FormerlySerializedAs("FadeInOutTime")] public float fadeInOutTime;
    public List<ScenarioSelectInfo> scenarios;
    public Dictionary<int, RectTransform> btns = new Dictionary<int, RectTransform>();

    private Entity _playerEntity = Entity.Null;
    private EntityManager _cachedEntityMng;
    private EntityManager _EntityMng {
        get {
            if (World.DefaultGameObjectInjectionWorld.EntityManager != _cachedEntityMng) {
                _cachedEntityMng = World.DefaultGameObjectInjectionWorld.EntityManager;
            }

            return _cachedEntityMng;
        }
    }


    void Start() {
        var playerEntities = _EntityMng.GetAllEntities()
            .Where(entity => _EntityMng.HasComponent(entity, typeof(PlayerComponent)));

        foreach (var entity in playerEntities) {
            _playerEntity = entity;
            break;
        }

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


    void Update() {
        foreach (var btn in btns) {
            int index = btn.Key;
            Vector3 pivot = scenarios[btn.Key].GetPivot();
            Vector3 convert2DPos = Camera.main.WorldToScreenPoint(pivot);
            
            RectTransform btnTrans = btn.Value;
            btnTrans.position = convert2DPos;
        }
    }


    public void OnSelectedInScenario(int inType) {
        _EntityMng.AddComponentData(_playerEntity, new TeleportInfoComponent(
            scenarios[inType].sceneType, scenarios[inType].curSubSceneType, scenarios[inType].nextSubSceneType, scenarios[inType].pointID, teleportTime, fadeInOutTime));

        var guiComp = _EntityMng.GetComponentData<GUIComponent>(_playerEntity);
        guiComp.currentUI ^= GUIState.scenarioSelect;
        _EntityMng.SetComponentData(_playerEntity, guiComp);
    }
}
