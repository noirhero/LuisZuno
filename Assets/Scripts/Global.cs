// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;

namespace GlobalDefine {
    public enum EntityType { None, Player, NonePlayer, Wall }
    public enum AnimationType { Walk, Idle, SomethingDoIt, NyoNyo }

    [Serializable]
    public struct ItemStruct {
        public Int32 chakra;
        public Int32 sane;
    }
}
