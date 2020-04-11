// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct GameStartComponent : IComponentData {

}

[Serializable]
public struct GameResumeComponent : IComponentData {

}

[Serializable]
public struct GamePauseComponent : IComponentData {

}

[Serializable]
public struct GameOverComponent : IComponentData {

}

[Serializable]
public struct GameClearComponent : IComponentData {

}
