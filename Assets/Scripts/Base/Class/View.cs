using UnityEngine;
using System.Collections.Generic;

public abstract class View {
    private UIPrefab m_view;
    private GameObject m_gameobject;
    public GameObject Node => m_gameobject;
    private Transform m_transform;
    public Transform Trans => m_transform;
    private Dictionary<string, UIImage> m_dicImage;

    public View(UIPrefab view) {
        m_view = view;
        m_gameobject = view.gameObject;
        m_transform = view.transform;
    }

    protected T GetNode<T>(string nodeName) where T:class {
        bool isImage = typeof(T) == typeof(UIImage);
        if (isImage && m_dicImage != null && m_dicImage.ContainsKey(nodeName))
            return m_dicImage[nodeName] as T;
        T component = m_view.GetNode<T>(nodeName);
        if (component is UIImage) {
            if (m_dicImage == null)
                m_dicImage = new Dictionary<string, UIImage>();
            m_dicImage.Add(nodeName, component as UIImage);
        }
        return component;
    }

    protected GameObject GetNode(string nodeName) {
        return m_view.GetNode(nodeName);
    }

    public void Show() {
        m_gameobject.SetActive(true);
    }

    public void Hide() {
        m_gameobject.SetActive(false);
    }

    ~View() {
        m_view = null;
        if (m_dicImage != null) {
            m_dicImage.Clear();
            m_dicImage = null;
        }
    }
}