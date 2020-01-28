// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class SpriteTimeline {
    public float start;
    public float end;
    public Texture2D texture;
}

[Serializable]
public class SpriteAnimData {
    public string name;
    public float length;
    public List<SpriteTimeline> timelines = new List<SpriteTimeline>();
}

[Serializable]
public class SpriteAnimDataDictionary : SerializableDictionaryBase<int, SpriteAnimData> {
}

public class NewSpritePreset : MonoBehaviour {
    public List<AnimationClip> clips = new List<AnimationClip>();
    public SpriteAnimDataDictionary datas = new SpriteAnimDataDictionary();

    public Texture2D GetFrame(int id, float accumTime) {
        if (false == datas.TryGetValue(id, out var animData)) {
            Debug.LogError($"Find animation data failed : {id}");
            return null;
        }

        if (accumTime > animData.length) {
            accumTime %= animData.length;
        }

        foreach (var timeline in animData.timelines) {
            if (accumTime >= timeline.start && accumTime < timeline.end) {
                return timeline.texture;
            }
        }

        return null;
    }
}
