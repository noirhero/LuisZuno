// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[UpdateAfter(typeof(GameSystem))]
public class InventorySystem : ComponentSystem {
    private InventoryComponent _inventoryComp;

    
    protected override void OnStartRunning() {
        _inventoryComp = EntityManager.GetComponentData<InventoryComponent>(Utility.playerEntity);
    }

    
    protected override void OnUpdate() {
        if (false == EntityManager.HasComponent<PendingItemComponent>(Utility.playerEntity)) {
            return;
        }

        var playerComp = EntityManager.GetComponentData<PlayerComponent>(Utility.playerEntity);
        if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
            return;
        }

        PendingItemComponent pendingItemComp = EntityManager.GetComponentData<PendingItemComponent>(Utility.playerEntity);
        Int64 pendingItemID = pendingItemComp.pendingItemID;
        EntityManager.RemoveComponent<PendingItemComponent>(Utility.playerEntity);
        if (false == Utility.IsValid(pendingItemID)) {
            return;
        }

        playerComp.currentBehaviors ^= BehaviorState.pendingItem;
        EntityManager.SetComponentData<PlayerComponent>(Utility.playerEntity, playerComp);

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
        if (Utility.tablePreset.itemDatas.TryGetValue(pendingItemID, out data)) {
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

        EntityManager.SetComponentData<InventoryComponent>(Utility.playerEntity, _inventoryComp);
    }
}