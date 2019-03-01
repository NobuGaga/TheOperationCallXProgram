using UnityEngine;

public class StartGame : MonoBehaviour {
    [SerializeField] private Canvas m_Canvas;

    void Start() {
        GameObject loadGreenCubeCopy = Resources.Load<GameObject>("Prefabs/GreenCubeCopy");
        GameObject gbGreenCubeCopy = Instantiate(loadGreenCubeCopy) as GameObject;
        gbGreenCubeCopy.transform.SetParent(gameObject.transform, false);
    }
}
