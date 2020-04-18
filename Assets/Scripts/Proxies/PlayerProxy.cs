// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using GlobalDefine;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class PlayerProxy : EntityProxy {
    public PlayerPreset playerPreset;
    public ItemStruct[] defaultInventory = new ItemStruct[0];
    [FormerlySerializedAs("Status")] public PlayerStatusComponent status;

    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new PlayerComponent(1.0f));
        dstManager.AddComponentData(entity, new PlayerStatusComponent(ref status));
        dstManager.AddComponentData(entity, new InventoryComponent(defaultInventory));
        dstManager.AddComponentData(entity, new TargetingComponent());

        var playerSprite = GetComponent<SpriteRenderer>()?.sprite;
        dstManager.AddSharedComponentData(entity, new PlayerPresetComponent() {
            preset = playerPreset,
            sprite = playerSprite
        });
    }
}
