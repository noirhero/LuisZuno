// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct InventoryComponent : IComponentData {
    public ItemStruct item1;
    public ItemStruct item2;
    public ItemStruct item3;


    public InventoryComponent(ItemStruct[] inData) {
        ItemStruct cacehdItem = new ItemStruct();
        cacehdItem.Empty();

        item1 = inData.Length > 0 ? inData[0] : cacehdItem;
        item2 = inData.Length > 1 ? inData[1] : cacehdItem;
        item3 = inData.Length > 2 ? inData[2] : cacehdItem;
    }


    public bool IsEmptySlot1() {
        return false == Utility.IsVaild(item1.id);
    }


    public bool IsEmptySlot2() {
        return false == Utility.IsVaild(item2.id);
    }


    public bool IsEmptySlot3() {
        return false == Utility.IsVaild(item3.id);
    }


    public void SetSlot1(Int64 inID, ItemPresetData inData) {
        item1.id = inID;
        item1.madness = inData.madness;
        item1.AddedTime = DateTime.UtcNow.Ticks;
    }


    public void SetSlot2(Int64 inID, ItemPresetData inData) {
        item2.id = inID;
        item2.madness = inData.madness;
        item2.AddedTime = DateTime.UtcNow.Ticks;
    }


    public void SetSlot3(Int64 inID, ItemPresetData inData) {
        item3.id = inID;
        item3.madness = inData.madness;
        item3.AddedTime = DateTime.UtcNow.Ticks;
    }
}