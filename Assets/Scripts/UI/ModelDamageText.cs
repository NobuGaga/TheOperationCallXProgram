using UnityEngine;
using UnityEngine.UI;
using System;

public class ModelDamageText:MonoBehaviour {
    [SerializeField]
    private Text m_text;
    public Text text => m_text;
    [SerializeField]
    private int m_moveDis = 100;
    private float m_originY;
    private Transform m_camera;

    private void Awake() {
        Canvas canvas = GetComponent<Canvas>();
        m_camera = GameSceneManager.GetNode<Transform>("PlayerCamera");
        canvas.worldCamera = m_camera.GetComponent<Camera>();
        if (m_text != null)
            return;
        Transform[] nodes = GetComponentsInChildren<Transform>();
        if (nodes == null)
            return;
        for (int i = 0; i < nodes.Length; i++)
            if (nodes[i].name == "text") {
                m_text = nodes[i].GetComponent<Text>();
                break;
            }
        m_originY = transform.localPosition.y;
        gameObject.SetActive(false);
    }

    public void SetText(string text = "") {
        if (text == string.Empty || m_text == null) {
            DestroyImmediate(gameObject, false);
            return;
        }
        m_text.text = text;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (m_text == null)
            return;
        if (Math.Abs(transform.localPosition.y - m_originY) > m_moveDis)
            DestroyImmediate(gameObject, false);
        else {
            transform.Translate(Vector3.up * Time.deltaTime);
            transform.LookAt(m_camera);
        }
    }
}