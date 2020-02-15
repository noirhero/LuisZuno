// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using GlobalDefine;

[Serializable]
public struct NPCAIPresetData {
    public AnimationType animation;
    public float time;
    private float _elapsedTime;

    public float ElapsedTime {
        get => _elapsedTime;
        set => _elapsedTime = value;
    }
}

[Serializable]
public class NPCAIPresetDataDictionary : SerializableDictionaryBase<Int64, NPCAIPresetData> {
}

[Serializable]
public class NPCAIPreset : MonoBehaviour {
    public NPCAIPresetDataDictionary AIDatas = new NPCAIPresetDataDictionary();
}
