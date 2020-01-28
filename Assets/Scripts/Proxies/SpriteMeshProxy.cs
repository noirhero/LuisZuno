// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class SpriteMeshProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public Mesh mesh;
    public Material material;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (ReferenceEquals(mesh, null) || ReferenceEquals(material, null)) {
            Debug.LogError("Set mesh and material, now!!!!!");
            dstManager.DestroyEntity(entity);
            return;
        }

        dstManager.RemoveComponent<LocalToWorld>(entity);
        dstManager.RemoveComponent<Rotation>(entity);
        dstManager.RemoveComponent<Translation>(entity);

        dstManager.AddSharedComponentData(entity, new SpriteMeshPresetComponent() {
            mesh = mesh,
            material = material
        });
    }
}
