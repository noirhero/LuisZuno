// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class NoneAvatarProxy : EntityProxy {
    public Int64 dropItem;
    [FormerlySerializedAs("Status")] public NoneAvatarStatusComponent status;
    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);
        
        dstManager.AddComponentData(entity, new NoneAvatarStatusComponent(ref status));
        if (Utility.IsValid(dropItem)) {
            dstManager.AddComponentData(entity, new DropComponent() {
                dropItemID = dropItem,
            });
        }
    }
}
