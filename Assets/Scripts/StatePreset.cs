// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[Serializable]
public class NameToKeyDictionary : SerializableDictionaryBase<string, int> {
}

[Serializable]
public class StatePreset : MonoBehaviour {
    [Header("To to insert states.")] public List<string> states = new List<string>();

    [Header("Do not touch!")] public NameToKeyDictionary stateToKeys = new NameToKeyDictionary();

    public int StateToKey(string state) {
        return (false == stateToKeys.TryGetValue(state, out var stateKey)) ? 0 : stateKey;
    }
}
