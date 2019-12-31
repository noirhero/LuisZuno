// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine.Serialization;

namespace GlobalDefine {
    public enum EntityType { None, Player, Wall, MadnessEnvironment }
    public enum AnimationType { Walk, Idle, SomethingDoIt, NyoNyo }
    public enum BackgroundType { BackStreetBoy, Poem, VeteranSoldier, Priest, Professor, Detective }
    public enum ValuesType { Mercy, Greedy, Curiosity, SenseOfDuty }
    public enum GoalType { TraceOfParents, CreateOfMasterpiece, Rich }

    [Serializable]
    public struct ItemStruct {
        public Int64 id;
        public int madness;
        [FormerlySerializedAs("AddedTime")] public Int64 addedTime;
        
        public void Empty() {
            id = 0;
            madness = 0;
            addedTime = 0;
        }
    }

    public static class Utility {
        public static bool IsValid(Int64 index) {
            return index != 0;
        }
        public static T ToEnum<T>(string inValue) {
            return (T)Enum.Parse(typeof(T), inValue);
        }
    }

    public static class BehaviorState {
        public const int searching = 0x1;
        public const int panic = 0x2;
        public const int pendingItem = 0x4;

        public static bool HasState(PlayerComponent playerComp, int compareState) { return ((playerComp.currentBehaviors & compareState) != 0); }
    }
}
