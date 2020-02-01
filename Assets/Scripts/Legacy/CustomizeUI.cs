// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Unity.Entities;
using GlobalDefine;

public class CustomizeUI : MonoBehaviour {
    public ToggleGroup backgroundGroup;
    public ToggleGroup valuesGroup;
    public ToggleGroup goalGroup;

    public Text backgroundText;
    public Text valuesText;
    public Text goalText;

    public Text remainText;
    public Text mentalityText;
    public Text agilityText;
    public Text physicalText;
    public Text searchText;
    public Text luckText;

    public GameObject decision;

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

    private bool IsPlayerEntityHasCustomizeComponent() {
        return _EntityMng.HasComponent<CustomizeComponent>(_playerEntity);
    }


    void Start() {
        decision.SetActive(false);

        var playerEntities = _EntityMng.GetAllEntities()
            .Where(entity => _EntityMng.HasComponent(entity, typeof(PlayerComponent)));

        foreach (var entity in playerEntities) {
            _EntityMng.AddComponentData(entity, new CustomizeComponent(10));
            _playerEntity = entity;
            break;
        }

        OnSelectedInBackground();
        OnSelectedInValues();
        OnSelectedInGoal();
        OnSelectedInMentality(0);
        OnSelectedInAgility(0);
        OnSelectedInPhysical(0);
        OnSelectedInSearch(0);
        OnSelectedInLuck(0);
    }


    void Update() {
        
        if (Input.GetKey(KeyCode.Escape)) {
            OnSelectedInConfirm();
        }
    }


    public void OnSelectedInBackground() {
        foreach (var toggle in backgroundGroup.ActiveToggles()) {
            backgroundText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.backgroundType = Utility.ToEnum<BackgroundType>(toggle.name);
            _EntityMng.SetComponentData(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInValues() {
        foreach (var toggle in valuesGroup.ActiveToggles()) {
            valuesText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.valuesType = Utility.ToEnum<ValuesType>(toggle.name);
            _EntityMng.SetComponentData(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInGoal() {
        foreach (var toggle in goalGroup.ActiveToggles()) {
            goalText.text = toggle.name;

            if (false == IsPlayerEntityHasCustomizeComponent()) {
                continue;
            }

            var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.goalType = Utility.ToEnum<GoalType>(toggle.name);
            _EntityMng.SetComponentData(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInDecision() {
        decision.SetActive(true);
    }


    public void OnSelectedInConfirm() {
        decision.SetActive(false);
        this.gameObject.SetActive(false);

        _EntityMng.AddComponentData<GameStartComponent>(_playerEntity, new GameStartComponent());

        // the player must have these components
        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        var playerStatusComp = _EntityMng.GetComponentData<PlayerStatusComponent>(_playerEntity);

        playerStatusComp.luck += customizeComp.luck * 10.0f;
        playerStatusComp.agility += customizeComp.agility * 10.0f;
        playerStatusComp.physical += customizeComp.physical * 10.0f;
        playerStatusComp.search += customizeComp.search * 10.0f;
        playerStatusComp.mentality += customizeComp.mentality * 10.0f;
        _EntityMng.SetComponentData<PlayerStatusComponent>(_playerEntity, playerStatusComp);

        CalcPlayerStatus();
    }


    public void OnSelectedInCancel() {
        decision.SetActive(false);
    }

    
    public void OnSelectedInMentality(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.mentality + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.mentality = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        mentalityText.text = customizeComp.mentality.ToString();
    }


    public void OnSelectedInAgility(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.agility + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.agility = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        agilityText.text = customizeComp.agility.ToString();
    }


    public void OnSelectedInPhysical(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.physical + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.physical = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        physicalText.text = customizeComp.physical.ToString();
    }


    public void OnSelectedInSearch(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.search + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.search = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        searchText.text = customizeComp.search.ToString();
    }


    public void OnSelectedInLuck(int inValue) {
        if (false == IsPlayerEntityHasCustomizeComponent()) {
            return;
        }

        var customizeComp = _EntityMng.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        var pendingValue = customizeComp.luck + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.luck = pendingValue;
        _EntityMng.SetComponentData(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        luckText.text = customizeComp.luck.ToString();
    }


    public void CalcPlayerStatus() {
        var playerStatusComp = _EntityMng.GetComponentData<PlayerStatusComponent>(_playerEntity);

        playerStatusComp.MoveSpeed = Mathf.Clamp(1.0f + playerStatusComp.agility * 0.005f, 1.0f, 1.5f); // 1배 ~ 1.5배
        playerStatusComp.MadnessWeight = Mathf.Clamp(1.0f - playerStatusComp.mentality * 0.005f, 0.5f, 1.0f);    // 1배 ~ 0.5배
        playerStatusComp.SearchingWeight = Mathf.Clamp(1.0f - playerStatusComp.physical * 0.005f, 0.0f, 1.0f);    // 1배 ~ 0.5배

        _EntityMng.SetComponentData<PlayerStatusComponent>(_playerEntity, playerStatusComp);
    }
}
