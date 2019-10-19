// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class NoneAvatarProxy : EntityProxy {
    public Int32 chakra;
    public Int32 reactedCount;
    public Int32 tempting;

    public override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new NoneAvatarStatusComponent() {
            chakra = chakra,
            reactedCount = reactedCount,
            tempting = tempting
        });
    }
}
