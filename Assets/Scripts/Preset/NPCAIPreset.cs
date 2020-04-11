// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using GlobalDefine;

[Serializable]
public struct NPCAIPresetData {
    public AnimationType animation;
    public float time;
}

[Serializable]
public class NPCAIPresetDataDictionary : SerializableDictionaryBase<Int64, NPCAIPresetData> {
}

[Serializable]
public class NPCAIPreset : MonoBehaviour {
    public NPCAIPresetDataDictionary AIDatas = new NPCAIPresetDataDictionary();
}
