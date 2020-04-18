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

        if (Entity.Null != _playerEntity) {
            int presetUI = 0;
            presetUI |= GUIState.customize;
            
            EntityManager.AddComponentData(_playerEntity, new GameStartComponent());
            EntityManager.AddComponentData(_playerEntity, new GUIComponent(){
                currentUI = presetUI,
            });
        }

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

        Entities.ForEach((Entity playerEntity, ref GUIComponent guiComp) => { 
            // Customize
            if (GUIState.HasState(guiComp, GUIState.customize)) {
                if (false == EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
                    EntityManager.AddComponentData(_playerEntity, new CustomizeComponent());
                    EntityManager.AddComponentData(_playerEntity, new GamePauseComponent());
                    _guiPreset.ShowCustomize();
                }
            }
            else {
                if (EntityManager.HasComponent<CustomizeComponent>(_playerEntity)) {
                    EntityManager.RemoveComponent<CustomizeComponent>(_playerEntity);
                    EntityManager.AddComponentData(_playerEntity, new GameResumeComponent());
                    _guiPreset.HideCustomize();
                }
            }
            
            // ScenarioSelect
            if (GUIState.HasState(guiComp, GUIState.scenarioSelect)) {
                _guiPreset.ShowScenarioSelect();
            }
            else {
                _guiPreset.HideScenarioSelect();
            }
            
            // Inventory
            if (GUIState.HasState(guiComp, GUIState.inventory)) {
                if (EntityManager.HasComponent<InventoryComponent>(_playerEntity)) {
                    InventoryComponent inventoryComp = EntityManager.GetComponentData<InventoryComponent>(_playerEntity);
                    SetItemSprite(inventoryComp.item1.id, _guiPreset.item1);
                    SetItemSprite(inventoryComp.item2.id, _guiPreset.item2);
                    SetItemSprite(inventoryComp.item3.id, _guiPreset.item3);
                }
                _guiPreset.ShowInventory();
            }
            else {
                _guiPreset.HideInventory();
            }
            
            // Madness
            if (GUIState.HasState(guiComp, GUIState.madness)) {
                _guiPreset.ShowMadness(GetMadnessRate());
            }
            else {
                _guiPreset.HideMadness();
            }
            
            // Bubble
            if (GUIState.HasState(guiComp, GUIState.bubble)) {
                _guiPreset.ShowBubble();
            }
            else {
                _guiPreset.HideBubble();
            }

            // Ending
            if (GUIState.HasState(guiComp, GUIState.ending)) {
                _guiPreset.ShowEnding();
            }
            else {
                _guiPreset.HideEnding();
            }
        });

        // Teleport
        if (EntityManager.HasComponent<TeleportInfoComponent>(_playerEntity)) {
            Entities.ForEach((Entity playerEntity, ref PlayerComponent playerComp, ref TeleportInfoComponent teleportInfoComp) => {
                if (playerComp.currentBehaviors > 0) {
                    return;
                }
                EntityManager.RemoveComponent<TeleportInfoComponent>(playerEntity);
                EntityManager.AddComponentData(playerEntity, new TeleportComponent(ref teleportInfoComp));
                playerComp.currentBehaviors |= BehaviorState.teleport;
            });
        }
    }


    private void SetItemSprite(Int64 inID, Image inImg) {
        ItemPresetData data;
        if (_tablePreset.itemDatas.TryGetValue(inID, out data)) {
            inImg.gameObject.SetActive(Utility.IsValid(inID));
            inImg.sprite = data.sprite;
        }
    }


    private Vector3 GetPlayerPosToGUIPos() {
        Translation playerPos = EntityManager.GetComponentData<Translation>(_playerEntity);
        Vector3 convert2DPos = Camera.main.WorldToScreenPoint(playerPos.Value);
        return convert2DPos;
    }
    
    
    private string GetBubbleMessage() {
        var playerComp = EntityManager.GetComponentData<PlayerComponent>(_playerEntity);
        if (0 == playerComp.currentBehaviors) {     // walking or doing nothing
            return string.Empty;
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
            return string.Empty;
        }

        // todo - temporary
        int showMessageLength = (int)((float)bubbleMassage.Length * timeRate);

        return bubbleMassage.Substring(0, showMessageLength);
    }


    private float GetMadnessRate() {
        PlayerStatusComponent statusComp = EntityManager.GetComponentData<PlayerStatusComponent>(_playerEntity);
        return statusComp.madness / statusComp.maxMadness;
    }
}
