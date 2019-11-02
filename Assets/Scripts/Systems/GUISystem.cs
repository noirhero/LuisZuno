// Copyrighgt 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine.UI;
using Unity.Entities;
using GlobalDefine;

public class GUISystem : ComponentSystem {
    private TablePreset _tablePreset;
    private GUIPreset _guiPreset;
    private Entity _playerEntity = Entity.Null;
    

    private void SetItemSprite(Int64 inID, Image inImg) {
        if (null == _tablePreset) {
            return;
        }

        if (false == Utility.IsVaild(inID)) {
            return;
        }

        ItemPresetData data;
        if (false == _tablePreset.itemDatas.TryGetValue(inID, out data)) {
            return;
        }

        inImg.gameObject.SetActive(Utility.IsVaild(inID));
        inImg.sprite = data.sprite;
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

        if (false == EntityManager.HasComponent<InventoryComponent>(_playerEntity)) {
            return;
        }

        // set gui - inventory
        InventoryComponent inventoryComp = EntityManager.GetComponentData<InventoryComponent>(_playerEntity);
        SetItemSprite(inventoryComp.item0.id, _guiPreset.item0);
        SetItemSprite(inventoryComp.item1.id, _guiPreset.item1);
        SetItemSprite(inventoryComp.item2.id, _guiPreset.item2);
    }
}
