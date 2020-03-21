// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using UnityEditor;
using Unity.Entities;
using System.Collections.Generic;

[RequiresEntityConversion]
public class TableProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public string presetPath;

    protected TablePreset _preset = null;
    

    public void Awake() {
        LoadAssets();
    }


    public void DeclareReferencedPrefabs(List<GameObject> referencedPresets) {
        SetupPrefabs(referencedPresets);
    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        SetupComponents(entity, dstManager, conversionSystem);
    }


    protected virtual void LoadAssets() {
        if (0 < presetPath.Length) {
            _preset = AssetDatabase.LoadAssetAtPath<TablePreset>(presetPath);
        }
    }


    protected virtual void SetupPrefabs(List<GameObject> referencedPresets) {
        if (null != _preset) {
            referencedPresets.Add(_preset.gameObject);
        }
    }


    protected virtual void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null == _preset) {
            return;
        }

        dstManager.AddSharedComponentData(entity, new TablePresetComponent(_preset));
    }
}
