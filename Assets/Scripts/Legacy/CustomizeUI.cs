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

    public GameObject decision;

    public Text backgroundText;
    public Text valuesText;
    public Text goalText;

    private Entity _playerEntity = Entity.Null;


    void Start() {
        decision.SetActive(false);

        var entities = World.Active.EntityManager.GetAllEntities();
        foreach (var entity in entities) {
            if (true == World.Active.EntityManager.HasComponent(entity, typeof(ReactiveComponent))) {
                var reactiveComp = World.Active.EntityManager.GetComponentData<ReactiveComponent>(entity);
                if (EntityType.Player == reactiveComp.type) {
                    World.Active.EntityManager.AddComponentData<CustomizeComponent>(entity, new CustomizeComponent());
                    _playerEntity = entity;
                }
            }
        }

        OnSelectedInBackground();
        OnSelectedInValues();
        OnSelectedInGoal();
    }


    public void OnSelectedInBackground() {
        foreach (var toggle in backgroundGroup.ActiveToggles()) {
            backgroundText.text = toggle.name;

            var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.backgroundType = Utility.ToEnum<BackgroundType>(toggle.name);
            World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInValues() {
        foreach (var toggle in valuesGroup.ActiveToggles()) {
            valuesText.text = toggle.name;

            var customizeComp = World.Active.EntityManager.GetComponentData<CustomizeComponent>(_playerEntity);
            customizeComp.valuesType = Utility.ToEnum<ValuesType>(toggle.name);
            World.Active.EntityManager.SetComponentData<CustomizeComponent>(_playerEntity, customizeComp);
            break;
        }
    }


    public void OnSelectedInGoal() {
        foreach (var toggle in goalGroup.ActiveToggles()) {
            goalText.text = toggle.name;

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
    }

    public void OnSelectedInCancel() {
        decision.SetActive(false);
    }
}
