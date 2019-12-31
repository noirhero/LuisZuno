// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatePreset))]
[CanEditMultipleObjects]
public class StatePresetInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if (true == GUILayout.Button("Set states and then click.")) {
            InitializeStatePreset(target as StatePreset);
        }
    }

    private void InitializeStatePreset(StatePreset preset) {
        preset.stateToKeys.Clear();

        foreach (var state in preset.states) {
            int hash = 0;
            foreach (var b in Encoding.ASCII.GetBytes(state)) {
                hash += b;
            }

            preset.stateToKeys.Add(state, hash);
        }
    }
}
