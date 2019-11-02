// Copyrighgt 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
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


    private void SetItemSprite(Int64 inID, Image inImg) {
        ItemPresetData data;
        if (_tablePreset.itemDatas.TryGetValue(inID, out data)) {
            inImg.gameObject.SetActive(Utility.IsVaild(inID));
            inImg.sprite = data.sprite;
        }
    }


    private void UpdateBubbleUI() {
        _guiPreset.HideBubble();

        if (EntityManager.HasComponent<AvatarStatusComponent>(_playerEntity)) {
            AvatarStatusComponent avatarStatusComp = EntityManager.GetComponentData<AvatarStatusComponent>(_playerEntity);
            if(avatarStatusComp.InPanic) {
                _guiPreset.ShowBubble(Vector3.zero, "#$%^");
                return;
            }
        }

        if (EntityManager.HasComponent<ReactiveComponent>(_playerEntity)) {
            ReactiveComponent reactiveComp = EntityManager.GetComponentData<ReactiveComponent>(_playerEntity);
            if (reactiveComp.ReactionElapsedTime > 0) {
                _guiPreset.ShowBubble(Vector3.zero, "...");
                return;
            }
        }
    }


    protected override void OnStartRunning() {
        Entities.ForEach((TablePresetComponent presetComp) => {
            _tablePreset = presetComp.preset;
        });

        Entities.ForEach((GUIPresetComponent presetComp) => {
            _guiPreset = presetComp.preset;
        });

        Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp) => {
            if (GlobalDefine.EntityType.Player != reactiveComp.type) {
                return;
            }

            _playerEntity = entity;
        });

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

        // set gui - inventory
        UpdateInventoryUI();

        // set gui - bubble
        UpdateBubbleUI();
    }
}
