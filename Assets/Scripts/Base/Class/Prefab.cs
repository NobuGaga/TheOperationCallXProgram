using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Prefab {
    private UIPrefab m_rootNode;
    private GameObject m_gameobject;
    public GameObject Node => m_gameobject;
    private Transform m_transform;
    public Transform Trans => m_transform;
    private Dictionary<string, UIImage> m_dicImage;
    private Dictionary<string, Text> m_dicText;
    private Dictionary<string, Button> m_dicButton;

    public Prefab(UIPrefab prefab) {
        m_rootNode = prefab;
        m_gameobject = prefab.gameObject;
        m_transform = prefab.transform;
    }

    protected T GetNode<T>(string nodeName) where T:class {
        if (typeof(T) == typeof(UIImage) && m_dicImage != null && m_dicImage.ContainsKey(nodeName))
            return m_dicImage[nodeName] as T;
        else if (typeof(T) == typeof(Text) && m_dicText != null && m_dicText.ContainsKey(nodeName))
            return m_dicText[nodeName] as T;
        else if (typeof(T) == typeof(Button) && m_dicButton != null && m_dicButton.ContainsKey(nodeName))
            return m_dicButton[nodeName] as T;
        T component = m_rootNode.GetNode<T>(nodeName);
        if (component is UIImage) {
            if (m_dicImage == null)
                m_dicImage = new Dictionary<string, UIImage>();
            m_dicImage.Add(nodeName, component as UIImage);
        } else if (component is Text) {
            if (m_dicText == null)
                m_dicText = new Dictionary<string, Text>();
            m_dicText.Add(nodeName, component as Text);
        } else if (component is Button) {
            if (m_dicButton == null)
                m_dicButton = new Dictionary<string, Button>();
            m_dicButton.Add(nodeName, component as Button);
        }
        return component;
    }

    protected GameObject GetNode(string nodeName) {
        return m_rootNode.GetNode(nodeName);
    }

    public void Show() {
        m_gameobject.SetActive(true);
    }

    public void Hide() {
        m_gameobject.SetActive(false);
    }

    ~Prefab() {
        if (m_dicImage != null)
            m_dicImage.Clear();
        if (m_dicText != null)
            m_dicText.Clear();
        if (m_dicButton != null) {
            var enumerator = m_dicButton.GetEnumerator();
            while (enumerator.MoveNext())
                enumerator.Current.Value.onClick.RemoveAllListeners();
        }
        m_rootNode = null;
        m_transform = null;
        GameObject.Destroy(m_gameobject);
        m_gameobject = null;
    }
}