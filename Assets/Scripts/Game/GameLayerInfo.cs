﻿public enum GameLayerInfo {
    Default       = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Water         = 4,
    UI            = 5,
    Player        = 8,
    Enemy         = 9,
    Scene         = 10,
}

public enum GameTagInfo {
    Respawn,
    Finish,
    EditorOnly,
    MainCamera,
    Player,
    GameController,
    Enemy,
}