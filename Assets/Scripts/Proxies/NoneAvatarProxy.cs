// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[RequiresEntityConversion]
public class NoneAvatarProxy : EntityProxy {
    public Int32 chakra;
    public Int32 reactedCount;
    public Int32 temptation;
    public Int64 dropItem;

    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new NoneAvatarStatusComponent() {
            chakra = chakra,
            reactedCount = reactedCount,
            temptation = temptation
        });        
    }
}
