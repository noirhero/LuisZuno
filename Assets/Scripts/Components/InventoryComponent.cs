// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct InventoryComponent : IComponentData {
    public ItemStruct item1;
    public ItemStruct item2;
    public ItemStruct item3;
    public ItemStruct pendingItem { get; private set; }
    public bool bHasPendingItem { get; private set; }

    public InventoryComponent(ItemStruct[] inData) {
        item1 = inData.Length > 1 ? inData[0] : new ItemStruct();
        item2 = inData.Length > 2 ? inData[1] : new ItemStruct();
        item3 = inData.Length > 3 ? inData[2] : new ItemStruct();
        pendingItem = new ItemStruct();
        bHasPendingItem = false;
    }

    public void AddItem(ItemStruct inData) {
        pendingItem = inData;
        bHasPendingItem = true;
    }

    public void ClearPendingItem() {
        bHasPendingItem = false;
    }
}