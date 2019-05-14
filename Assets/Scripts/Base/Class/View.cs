using UnityEngine;
using System.Collections.Generic;

public abstract class View:Prefab {
    protected GameMoudle m_moudle;
    protected GameMoudle Moudle => m_moudle;
    protected GameView m_view;
    protected GameView ViewType => m_view;

    protected Dictionary<string, List<UIPrefab>> m_dicItem;
    protected bool IsHaveItem {
        set {
            if (value) {
                if (m_dicItem == null)
                    m_dicItem = new Dictionary<string, List<UIPrefab>>();
            } else {
                if (m_dicItem != null)
                    m_dicItem.Clear();
                m_dicItem = null;
            }
        }
    }

    public View(GameMoudle moudle, GameView view, UIPrefab prefab):base(prefab) {
        m_moudle = moudle;
        m_view = view;
    }

    protected void LoadItem(string itemName, Transform parent, int count = 1, System.Action callback = null) {
        IsHaveItem = true;
        ViewManager.LoadItem(GameViewInfo.GetViewName(m_moudle, m_view), itemName, 
            (GameObject gameObject) => {
                for (int i = 0; i < count; i++) {
                    GameObject item = Object.Instantiate(gameObject) as GameObject;
                    item.transform.SetParent(parent);
                    AddItem(itemName, item.GetComponent<UIPrefab>());
                }
                callback?.Invoke();
            }
        );
    }

    private void AddItem(string itemName, UIPrefab item) {
        if (!m_dicItem.ContainsKey(itemName))
            m_dicItem.Add(itemName, new List<UIPrefab>());
        m_dicItem[itemName].Add(item);
    }

    protected UIPrefab GetItem(string itemName, int index) {
        if (m_dicItem.ContainsKey(itemName) && index < m_dicItem[itemName].Count)
            return m_dicItem[itemName][index];
        return null;
    }

    ~View() {
        if (m_dicItem != null)
            m_dicItem.Clear();
    }
}