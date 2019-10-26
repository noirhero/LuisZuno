// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct InventoryComponent : IComponentData {
    public ItemStruct item0;
    public ItemStruct item1;
    public ItemStruct item2;
    public ItemStruct pendingItem { get; private set; }
    public bool bHasPendingItem { get; private set; }

    public InventoryComponent(ItemStruct[] inData) {
        ItemStruct cacehdItem = new ItemStruct();
        cacehdItem.Empty();

        item0 = inData.Length > 0 ? inData[0] : cacehdItem;
        item1 = inData.Length > 1 ? inData[1] : cacehdItem;
        item2 = inData.Length > 2 ? inData[2] : cacehdItem;
        pendingItem = inData.Length > 3 ? inData[3] : cacehdItem;
        bHasPendingItem = false;
    }

    public void AddItem(ItemComponet inItem) {
        pendingItem = inItem.data;
        bHasPendingItem = true;
    }

    public void ClearPendingItem() {
        bHasPendingItem = false;
    }
}