// Copyrigth 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct ItemComponet : IComponentData {
    public ItemStruct data;
    
    public ItemComponet(ItemStruct inData) {
        data = inData;
    }
}
