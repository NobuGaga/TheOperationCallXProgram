using UnityEngine;
using System;
using System.Collections.Generic;

public static class TimerManager {
    private static List<DelayCallData> m_listEvent = new List<DelayCallData>();
    private static List<Action> m_listSecondEvent = new List<Action>();
    private static Dictionary<Action, int> m_dicSecondEvent = new Dictionary<Action, int>();
    private static float m_lastTime = 1;

    /// <summary>
    /// 注册延时调用事件 (可重复注册)
    /// </summary>
    /// <param name="delay">延时秒数</param>
    /// <param name="callback">回调</param>
    public static void Register(float delay, Action callback) {
        m_listEvent.Add(new DelayCallData(delay, callback));
    }

    public static void RegisterSecond(Action callback) {
        if (m_dicSecondEvent.ContainsKey(callback))
            return;
        m_dicSecondEvent.Add(callback, m_listSecondEvent.Count);
        m_listSecondEvent.Add(callback);
    }

    public static void UnregisterSecond(Action callback) {
        if (!m_dicSecondEvent.ContainsKey(callback))
            return;
        m_listSecondEvent.RemoveAt(m_dicSecondEvent[callback]);
        m_dicSecondEvent.Remove(callback);
    }

    public static void Update() {
        for (int i = m_listEvent.Count - 1; i >= 0; i--) {
            DelayCallData data = m_listEvent[i];
            m_listEvent[i] = new DelayCallData(data.delay - Time.deltaTime, data.callback);
            DebugTool.Log("m_listEvent[i].delay " + m_listEvent[i].delay);
            if (m_listEvent[i].delay <= 0) {
                m_listEvent[i].callback();
                m_listEvent.RemoveAt(i);
            }
        }
        m_lastTime -= Time.deltaTime;
        if (m_lastTime > 0)
            return;
        m_lastTime = 1;
        for (int i = 0; i < m_listSecondEvent.Count; i++)
            m_listSecondEvent[i]();
    }

    public struct DelayCallData {
        public float delay;
        public Action callback;

        public DelayCallData(float delay, Action callback) {
            this.delay = delay;
            this.callback = callback;
        }
    }
}