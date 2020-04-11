// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct CustomizeComponent : IComponentData {
    public BackgroundType backgroundType;
    public ValuesType valuesType;
    public GoalType goalType;

    public int remain;
    public int mentality;
    public int agility;
    public int physical;
    public int search;
    public int luck;

    public CustomizeComponent(int inRemain) {
        backgroundType = BackgroundType.BackStreetBoy;
        valuesType = ValuesType.Curiosity;
        goalType = GoalType.CreateOfMasterpiece;
        remain = inRemain;
        mentality = 0;
        agility = 0;
        physical = 0;
        search = 0;
        luck = 0;
    }
}

[Serializable]
public struct CustomizeCompleteComponent : IComponentData {

}
