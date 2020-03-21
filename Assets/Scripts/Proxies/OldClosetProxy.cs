// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Serialization;
using UnityEditor;

[RequiresEntityConversion]
public class OldClosetProxy : PropProxy {
    [FormerlySerializedAs("SpawnInfo")] public EntitySpawnInfoComponent spawnInfo;
    public string spawnPresetPath;
    public string spawnEffectPath;
    public string destroyEffectPath;

    private GameObject _spawnPreset = null;
    private GameObject _spawnEffectPreset = null;
    private GameObject _destroyEffectPreset = null;


    protected override void LoadAssets() {
        base.LoadAssets();

        if (0 < spawnPresetPath.Length) {
            _spawnPreset = AssetDatabase.LoadAssetAtPath<GameObject>(spawnPresetPath);
        }
        if (0 < spawnEffectPath.Length) {
            _spawnEffectPreset = AssetDatabase.LoadAssetAtPath<GameObject>(spawnEffectPath);
        }
        if (0 < destroyEffectPath.Length) {
            _destroyEffectPreset = AssetDatabase.LoadAssetAtPath<GameObject>(destroyEffectPath);
        }
    }


    protected override void SetupPrefabs(List<GameObject> referencedPrefabs) {
        base.SetupPrefabs(referencedPrefabs);

        if (null != _spawnPreset) {
            referencedPrefabs.Add(_spawnPreset);
        }

        if (null != _spawnEffectPreset) {
            referencedPrefabs.Add(_spawnEffectPreset);
        }

        if (null != _destroyEffectPreset) {
            referencedPrefabs.Add(_destroyEffectPreset);
        }
    }


    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new EntitySpawnInfoComponent(ref spawnInfo) {
            preset = conversionSystem.GetPrimaryEntity(_spawnPreset),
            spawnEffect = conversionSystem.GetPrimaryEntity(_spawnEffectPreset),
            destroyEffect = conversionSystem.GetPrimaryEntity(_destroyEffectPreset),
        });
    }
}