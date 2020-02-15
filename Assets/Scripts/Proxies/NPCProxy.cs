// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using UnityEngine.Serialization;

[RequiresEntityConversion]
public class NPCProxy : EntityProxy {
    [FormerlySerializedAs("Status")] public float speed;

    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new NPCComponent(1.0f, speed));

        if (null != preset) {
            var AIPreset = preset.GetComponent<NPCAIPreset>();
            if (null != AIPreset) {
                dstManager.AddSharedComponentData(entity, new NPCAIPresetComponent() {
                    preset = AIPreset
                });
            }
        }
    }
}
