﻿// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

public class InventorySystem : ComponentSystem {
    private InventoryComponent _inventoryComp;

    
    protected override void OnStartRunning() {
        _inventoryComp = EntityManager.GetComponentData<InventoryComponent>(Utility._playerEntity);
    }

    
    protected override void OnUpdate() {
        if (Utility._playerEntity.Equals(Entity.Null)) {
            return;
        }

        if (false == EntityManager.HasComponent<PendingItemComponent>(Utility._playerEntity)) {
            return;
        }

        var playerComp = EntityManager.GetComponentData<PlayerComponent>(Utility._playerEntity);
        if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
            return;
        }

        PendingItemComponent pendingItemComp = EntityManager.GetComponentData<PendingItemComponent>(Utility._playerEntity);
        Int64 pendingItemID = pendingItemComp.pendingItemID;
        EntityManager.RemoveComponent<PendingItemComponent>(Utility._playerEntity);
        if (false == Utility.IsValid(pendingItemID)) {
            return;
        }

        playerComp.currentBehaviors ^= BehaviorState.pendingItem;
        EntityManager.SetComponentData<PlayerComponent>(Utility._playerEntity, playerComp);

        //
        UInt16 slotIndex = 0;
        if (_inventoryComp.IsEmptySlot1()) {
            slotIndex = 1;
        }
        else if (_inventoryComp.IsEmptySlot2()) {
            slotIndex = 2;
        }
        else if (_inventoryComp.IsEmptySlot3()) {
            slotIndex = 3;
        }
        else {
            // condition - time
            if ((_inventoryComp.item1.addedTime < _inventoryComp.item2.addedTime) &&
                (_inventoryComp.item1.addedTime < _inventoryComp.item3.addedTime)) {
                slotIndex = 1;
            }
            else if (_inventoryComp.item2.addedTime < _inventoryComp.item3.addedTime) {
                slotIndex = 2;
            }
            else {
                slotIndex = 3;
            }
        }

        ItemPresetData data;
        if (Utility._tablePreset.itemDatas.TryGetValue(pendingItemID, out data)) {
            switch (slotIndex) {
                case 1:
                    _inventoryComp.SetSlot1(pendingItemID, data);
                    break;
                case 2:
                    _inventoryComp.SetSlot2(pendingItemID, data);
                    break;
                case 3:
                    _inventoryComp.SetSlot3(pendingItemID, data);
                    break;
            }
        }

        EntityManager.SetComponentData<InventoryComponent>(Utility._playerEntity, _inventoryComp);
    }
}