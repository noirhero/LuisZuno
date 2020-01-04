// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct InventoryComponent : IComponentData {
    public ItemStruct item1;
    public ItemStruct item2;
    public ItemStruct item3;


    public InventoryComponent(ItemStruct[] inData) {
        ItemStruct cachedItem = new ItemStruct();
        cachedItem.Empty();

        item1 = inData.Length > 0 ? inData[0] : cachedItem;
        item2 = inData.Length > 1 ? inData[1] : cachedItem;
        item3 = inData.Length > 2 ? inData[2] : cachedItem;
    }


    public bool IsEmptySlot1() {
        return false == Utility.IsValid(item1.id);
    }


    public bool IsEmptySlot2() {
        return false == Utility.IsValid(item2.id);
    }


    public bool IsEmptySlot3() {
        return false == Utility.IsValid(item3.id);
    }


    public void SetSlot1(Int64 inID, ItemPresetData inData) {
        item1.id = inID;
        item1.addedTime = DateTime.UtcNow.Ticks;
    }


    public void SetSlot2(Int64 inID, ItemPresetData inData) {
        item2.id = inID;
        item2.addedTime = DateTime.UtcNow.Ticks;
    }


    public void SetSlot3(Int64 inID, ItemPresetData inData) {
        item3.id = inID;
        item3.addedTime = DateTime.UtcNow.Ticks;
    }
}