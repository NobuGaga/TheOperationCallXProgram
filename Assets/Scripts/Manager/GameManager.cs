﻿using UnityEngine;

public static class GameManager {
    public static LogicScript logicScript;
    public static LogicScript LogicScript => logicScript;

    static GameManager() {
        GameSetting.Init();
        GameSceneManager.Init();
        ControllerManager.Init();
        // before AssetBundleManager.Init
        ViewManager.Init();
        ModelRoleManager.Init();
    }

    public static void Start(LogicScript startScript) {
        logicScript = startScript;
        GameObject.DontDestroyOnLoad(startScript.gameObject);
        AssetBundleManager.Init(() => EventManager.Dispatch(GameMoudle.Loading, GameEvent.Type.OpenMainView));
    }

    public static void FrameUpdate() {
        EventManager.Dispatch(GameEvent.Type.FrameUpdate, Time.time);
        TimerManager.Update();
    }

    public static void PhysicsUpdate() {
        EventManager.Dispatch(GameEvent.Type.PhysicsUpdate, Time.time);
    }

    public static void LastUpdate() {
        EventManager.Dispatch(GameEvent.Type.LastUpdate, Time.time);
    }
}