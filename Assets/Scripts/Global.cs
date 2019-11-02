// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;

namespace GlobalDefine {
    public enum EntityType { None, Player, NonePlayer, Wall }
    public enum AnimationType { Walk, Idle, SomethingDoIt, NyoNyo }

    [Serializable]
    public struct ItemStruct {
        public Int64 id;
        public Int32 madness;
        public Int64 AddedTime;
        
        public void Empty() {
            id = 0;
            madness = 0;
            AddedTime = 0;
        }
    }

    public static class Utility {
        public static bool IsVaild(int index) {
            return index != 0;
        }
        public static bool IsVaild(Int64 index) {
            return index != 0;
        }
    }
}
