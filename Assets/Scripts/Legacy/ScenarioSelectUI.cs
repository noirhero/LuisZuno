// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using GlobalDefine;
using System.Collections.Generic;

public class ScenarioSelectUI : MonoBehaviour {
    public Button btnPreset;
    public List<ScenarioStruct> scenarios;
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
            if (null == scenarios[i].uiPivot) {
                continue;
            }

            Button cachedBtn = GameObject.Instantiate<Button>(btnPreset);
            cachedBtn.onClick.AddListener(delegate { OnSelectedInScenario(i); });

            Text cachedText = cachedBtn.GetComponentInChildren<Text>();
            cachedText.text = scenarios[i].name;

            RectTransform cachedTrans = cachedBtn.GetComponentInChildren<RectTransform>();
            cachedTrans.SetParent(this.transform);
            btns.Add(i, cachedTrans);
        }
    }


    void Update() {
        foreach (var btn in btns) {
            int index = btn.Key;
            Vector3 pivot = scenarios[btn.Key].uiPivot.position;
            Vector3 convert2DPos = Camera.main.WorldToScreenPoint(pivot);
            
            RectTransform btnTrans = btn.Value;
            btnTrans.position = convert2DPos;
        }
    }


    public void OnSelectedInScenario(int inType) {

    }
}
