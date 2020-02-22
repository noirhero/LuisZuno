// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpritePreset))]
[CanEditMultipleObjects]
public class SpritePresetInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        if (GUILayout.Button("Set prefab and then click")) {
            InitializePresetData(target as SpritePreset);
        }
    }

    private void InitializePresetData(SpritePreset preset) {
        preset.datas.Clear();

        foreach (var clip in preset.clips) {
            var animData = new SpriteAnimData();
            animData.length = clip.length;

            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip)) {
                foreach (var frame in AnimationUtility.GetObjectReferenceCurve(clip, binding)) {
                    var sprite = (Sprite) frame.value;

                    animData.timelines.Add(new SpriteTimeline() {
                        start = frame.time,
                        texture = sprite.texture
                    });
                }
            }

            if (0 == animData.timelines.Count) {
                continue;
            }

            for (int i = 0; i < animData.timelines.Count - 1; ++i) {
                animData.timelines[i].end = animData.timelines[i + 1].start;
            }
            animData.timelines[animData.timelines.Count - 1].end = animData.length;

            animData.name = GetCutOffClipName(clip);

            var hash = 0;
            foreach (var c in animData.name) {
                hash += Convert.ToInt32(c);
            }

            preset.datas.Add(hash, animData);
        }
    }

    private string GetCutOffClipName(AnimationClip clip) {
        return clip.name.Substring(clip.name.LastIndexOf(".", StringComparison.Ordinal) + 1);
    }
}
