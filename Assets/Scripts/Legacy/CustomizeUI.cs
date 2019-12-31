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
            if (World.Active.EntityManager != _cachedEntityMng) {
                _cachedEntityMng = World.Active.EntityManager;
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

        foreach (var system in World.Active.Systems) {
            if (system.GetType() == typeof(TargetingSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(AutoMovementSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(MovementSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(IntelligenceSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(EffectSpawnSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(EntitySpawnSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(LifeCycleSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(HoldingSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(SearchingSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(PanicSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(MadnessSystem)) {
                system.Enabled = true;
            }
        }
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
}
