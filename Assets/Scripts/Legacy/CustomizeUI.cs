// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine.UI;
using UnityEngine;
using Unity.Entities;
using System.Linq;
using Unity.Collections;
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


    void Start() {
        decision.SetActive(false);

        var entities = World.Active.EntityManager.GetAllEntities();
        foreach (var entity in entities) {
            if (World.Active.EntityManager.HasComponent(entity, typeof(PlayerComponent))) {
                World.Active.EntityManager.AddComponentData<CustomizeComponent>(entity, new CustomizeComponent(10));
                _playerEntity = entity;
            }
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

            if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
                continue;
            }

            var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.backgroundType = Utility.ToEnum<BackgroundType>(toggle.name);
            World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInValues() {
        foreach (var toggle in valuesGroup.ActiveToggles()) {
            valuesText.text = toggle.name;

            if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
                continue;
            }

            var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.valuesType = Utility.ToEnum<ValuesType>(toggle.name);
            World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInGoal() {
        foreach (var toggle in goalGroup.ActiveToggles()) {
            goalText.text = toggle.name;

            if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
                continue;
            }

            var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.goalType = Utility.ToEnum<GoalType>(toggle.name);
            World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);
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
            else if (system.GetType() == typeof(ReactiveSystem)) {
                system.Enabled = true;
            }
            else if (system.GetType() == typeof(AutoMovementSystem)) {
                system.Enabled = true;
            }
        }
    }


    public void OnSelectedInCancel() {
        decision.SetActive(false);
    }

    
    public void OnSelectedInMentality(int inValue) {
        if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
            return;
        }

        var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        int pendingValue = customizeComp.mentality + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.mentality = pendingValue;
        World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        mentalityText.text = customizeComp.mentality.ToString();
    }


    public void OnSelectedInAgility(int inValue) {
        if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
            return;
        }

        var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        int pendingValue = customizeComp.agility + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.agility = pendingValue;
        World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        agilityText.text = customizeComp.agility.ToString();
    }


    public void OnSelectedInPhysical(int inValue) {
        if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
            return;
        }

        var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        int pendingValue = customizeComp.physical + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.physical = pendingValue;
        World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        physicalText.text = customizeComp.physical.ToString();
    }


    public void OnSelectedInSearch(int inValue) {
        if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
            return;
        }

        var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        int pendingValue = customizeComp.search + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.search = pendingValue;
        World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        searchText.text = customizeComp.search.ToString();
    }


    public void OnSelectedInLuck(int inValue) {
        if (false == World.Active.EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
            return;
        }

        var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
        if (-inValue < 0 && customizeComp.remain <= 0) {
            return;
        }

        int pendingValue = customizeComp.luck + inValue;
        if (pendingValue < 0) {
            return;
        }

        customizeComp.remain -= inValue;
        customizeComp.luck = pendingValue;
        World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);

        remainText.text = customizeComp.remain.ToString();
        luckText.text = customizeComp.luck.ToString();
    }
}
