// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;

namespace GlobalDefine {
    public enum EntityType { None, Player, NonePlayer, Wall }
    public enum AnimationType { Walk, Idle, SomethingDoIt, NyoNyo }

    [Serializable]
    public struct ItemStruct {
        public Int64 id;
        public Int32 madness;

        public bool IsVaild() {
            return id != 0;
        }
        public bool Empty() {
            return id != 0;
        }
    }
}
