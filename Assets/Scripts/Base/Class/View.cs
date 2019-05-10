using UnityEngine;
using System.Collections.Generic;

public abstract class View {
    private UIView m_view;
    private Dictionary<string, UIImage> m_dicImage;

    public View(UIView view) {
        m_view = view;
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
}
