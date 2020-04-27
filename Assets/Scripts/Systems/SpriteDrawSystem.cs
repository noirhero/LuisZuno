// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public class SpriteDrawSystem : ComponentSystem {
    private static readonly int _mainTex = Shader.PropertyToID("_MainTex");

    private Mesh _mesh;
    private Material _material;
    protected override void OnStartRunning() {
        Entities
            .ForEach((SpriteMeshPresetComponent spriteMesh) => {
                _mesh = spriteMesh.mesh;
                _material = spriteMesh.material;
            });
    }

    protected override void OnUpdate() {
        var propertyBlock = new MaterialPropertyBlock();
        Entities
            .ForEach((SpritePresetComponent spritePreset, ref SpriteStateComponent state, ref LocalToWorld transform) => {
                propertyBlock.SetTexture(_mainTex, spritePreset.preset.GetTexture(state.hash, state.frame));
                Graphics.DrawMesh(_mesh, transform.Value, _material, 0, Camera.current, 0, propertyBlock);
            });
    }
}
