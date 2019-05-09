using UnityEngine;
using System;

public class Loading:Controller {
    protected override void InitModel() {
        m_data = new LoadingData();
    }

    protected override void InitEvent() {
        m_eventList.Add(GameEvent.Type.OpenMainView);
    }

    public override Action GetEvent(GameEvent.Type eventType) {
        switch (eventType) {
            case GameEvent.Type.OpenMainView:
                return OpenMainView;
            default:
                return null;
        }
    }

    public void OpenMainView() {
        AssetBundleLoader.Load(GameManager.m_curLogicScript, "prefabs/moudle/loading/ui", "LoadingMainView",
            delegate (GameObject gameObj) {
                GameObject gbGreenCubeCopy = UnityEngine.Object.Instantiate(gameObj) as GameObject;
                gbGreenCubeCopy.transform.SetParent(GameManager.m_curLogicScript.gameObject.transform, false);
                //gbGreenCubeCopy.transform.localScale = new Vector3(100, 100, 100);

                //AssetBundleLoader.Load(GameManager.m_curLogicScript, "prefabs/moudle/loading/ui", "LoadingMainView1",
                //    delegate (GameObject gameObj1) {
                //        GameObject gbGreenCubeCopy1 = UnityEngine.Object.Instantiate(gameObj1) as GameObject;
                //        gbGreenCubeCopy1.transform.SetParent(GameManager.m_curLogicScript.gameObject.transform, false);
                //        //gbGreenCubeCopy1.transform.localScale = new Vector3(100, 100, 100);
                //    }
                //);
            }
        );
    }
}
