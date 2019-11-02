// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

public class InventorySystem : ComponentSystem {
    private TablePreset _tablePreset;
    private Entity _playerEntity = Entity.Null;
    private InventoryComponent _inventoryComp;


    protected override void OnStartRunning() {
        Entities.ForEach((TablePresetComponent presetComp) => {
            _tablePreset = presetComp.preset;
        });

        Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref InventoryComponent inventoryComp) => {
            if (EntityType.Player != reactiveComp.type) {
                return;
            }

            _playerEntity = entity;
            _inventoryComp = inventoryComp;
        });
    }


    protected override void OnUpdate() {
        if(_playerEntity.Equals(Entity.Null)) {
            return;
        }

        if (false == EntityManager.HasComponent<PendingItemComponent>(_playerEntity)) {
            return;
        }

        PendingItemComponent pendingItemComp = EntityManager.GetComponentData<PendingItemComponent>(_playerEntity);
        Int64 pendingItemID = pendingItemComp.pendingItemID;
        EntityManager.RemoveComponent<PendingItemComponent>(_playerEntity);

        //
        if (_inventoryComp.IsEmptySlot0()) {
            ItemPresetData data;
            if (_tablePreset.itemDatas.TryGetValue(pendingItemID, out data)) {
                _inventoryComp.SetSlot0(pendingItemID, data);
            }
        }
        else if (_inventoryComp.IsEmptySlot1()) {
            ItemPresetData data;
            if (_tablePreset.itemDatas.TryGetValue(pendingItemID, out data)) {
                _inventoryComp.SetSlot1(pendingItemID, data);
            }
        }
        else if (_inventoryComp.IsEmptySlot2()) {
            ItemPresetData data;
            if (_tablePreset.itemDatas.TryGetValue(pendingItemID, out data)) {
                _inventoryComp.SetSlot2(pendingItemID, data);
            }
        }
        else {
            // full todo
        }
        EntityManager.SetComponentData<InventoryComponent>(_playerEntity, _inventoryComp);
    }
}
