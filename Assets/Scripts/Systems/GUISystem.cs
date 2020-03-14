// Copyrighgt 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using GlobalDefine;

public class GUISystem : ComponentSystem {
    private TablePreset _tablePreset;
    private GUIPreset _guiPreset;
    private Entity _playerEntity = Entity.Null;
    

    private void UpdateInventoryUI() {
        if (EntityManager.HasComponent<InventoryComponent>(_playerEntity)) {
            InventoryComponent inventoryComp = EntityManager.GetComponentData<InventoryComponent>(_playerEntity);
            SetItemSprite(inventoryComp.item1.id, _guiPreset.item1);
            SetItemSprite(inventoryComp.item2.id, _guiPreset.item2);
            SetItemSprite(inventoryComp.item3.id, _guiPreset.item3);
        }
    }


    public void ActiveCustomize(bool inActive) {
        if (inActive) {
            EntityManager.AddComponentData<GamePauseComponent>(_playerEntity, new GamePauseComponent());
        }
        else {
            EntityManager.AddComponentData<GameResumeComponent>(_playerEntity, new GameResumeComponent());
        }

        if (null != _guiPreset) {
            _guiPreset.ActiveCustomize(inActive);
        }
    }


    public void ActiveScenarioSelect(bool inActive) {
        if (null != _guiPreset) {
            _guiPreset.ActiveScenarioSelect(inActive);
        }
    }


    private void SetItemSprite(Int64 inID, Image inImg) {
        ItemPresetData data;
        if (_tablePreset.itemDatas.TryGetValue(inID, out data)) {
            inImg.gameObject.SetActive(Utility.IsValid(inID));
            inImg.sprite = data.sprite;
        }
    }


    private void UpdateBubbleUI() {
        _guiPreset.HideBubble();

        var playerComp = EntityManager.GetComponentData<PlayerComponent>(_playerEntity);
        if (0 == playerComp.currentBehaviors) {     // walking or doing nothing
            return;
        }

        // bubble default
        string bubbleMassage = "... ";
        float timeRate = 0.0f;

        // Searching
        if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
            var searchingComp = EntityManager.GetComponentData<SearchingComponent>(_playerEntity);
            if (0 < searchingComp.elapsedSearchingTime) {
                timeRate = searchingComp.elapsedSearchingTime / searchingComp.searchingTime;
            }
        }
        // Panic
        else if (BehaviorState.HasState(playerComp, BehaviorState.panic)) {
            var panicComp = EntityManager.GetComponentData<PanicComponent>(_playerEntity);
            if (0 < panicComp.elapsedPanicTime) {
                bubbleMassage = "#$%^";
                timeRate = panicComp.elapsedPanicTime / panicComp.panicTime;
            }
        }

        if (0.0f >= timeRate) {
            return;
        }

        // bubble position
        Translation playerPos = EntityManager.GetComponentData<Translation>(_playerEntity);
        Vector3 convert2DPos = Camera.main.WorldToScreenPoint(playerPos.Value);
        convert2DPos.y += 30.0f;

        // todo - temporary
        int showMessageLength = (int)((float)bubbleMassage.Length * timeRate);

        // set
        _guiPreset.ShowBubble(convert2DPos, bubbleMassage.Substring(0, showMessageLength));
    }


    private void UpdateGaugeUI() {
        PlayerStatusComponent statusComp = EntityManager.GetComponentData<PlayerStatusComponent>(_playerEntity);
        _guiPreset.SetMadness(statusComp.madness / statusComp.maxMadness);
    }


    protected override void OnStartRunning() {
        Entities.ForEach((TablePresetComponent presetComp) => {
            _tablePreset = presetComp.preset;
        });

        Entities.ForEach((GUIPresetComponent presetComp) => {
            _guiPreset = presetComp.preset;
        });

        Entities.WithAll<PlayerComponent>().ForEach((Entity entity) => {
            _playerEntity = entity;
        });

        EntityManager.AddComponentData<GameStartComponent>(_playerEntity, new GameStartComponent());

        ActiveCustomize(false);
        ActiveScenarioSelect(true);

        if (null != _guiPreset) {
            _guiPreset.Initialize();
        }
    }


    protected override void OnUpdate() {
        if (null == _guiPreset) {
            return;
        }

        if (_playerEntity.Equals(Entity.Null)) {
            return;
        }


        Entities.ForEach((Entity playerEntity, ref PlayerComponent playerComp, ref ScenarioTeleportComponent teleportComp) => {
            if (playerComp.currentBehaviors > 0)
                return;

            //// Teleport
            //bool shouldTeleport = (EntityManager.HasComponent<ScenarioTeleportComponent>(_playerEntity)));
            //if (shouldTeleport) {
            //    var teleportInfo = EntityManager.GetComponentData<TeleportInfoComponent>(targetEntity);
            //    EntityManager.AddComponentData(playerEntity, new TeleportComponent(ref teleportInfo));
            //    playerComp.currentBehaviors |= BehaviorState.teleport;
            //}

            //if (playerComp.currentBehaviors > 0) {
            //    intelComp.isWaitingForOtherSystems = true;
            //}
        });
        // set gui - inventory
        UpdateInventoryUI();

        // set gui - bubble
        UpdateBubbleUI();

        // set gui - madness
        UpdateGaugeUI();
    }
}
