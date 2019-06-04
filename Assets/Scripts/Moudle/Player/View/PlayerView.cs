using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerView:View {
    private Stack<Transform> m_stackDamageText;
    private Dictionary<string, UIPrefab> m_dicHPProcess;
    private string m_playerModelName;

    public PlayerView(GameMoudle moudle, GameView view, UIPrefab prefab) : base(moudle, view, prefab) {
        if (!GameConfig.isDamageUseTextImage)
            m_stackDamageText = new Stack<Transform>();
        m_dicHPProcess = new Dictionary<string, UIPrefab>();
        GetNode<UIImage>("hpProcess").FillAmountX = 1;
    }

    public void UpdateHP(float hp) {
        GetNode<UIImage>("hpProcess").FillAmountX = hp;
        m_dicHPProcess[m_playerModelName].GetNode<UIImage>("hpProcess").FillAmountX = hp;
    }

    public void ShowDamageText(Vector3 position, int damage) {
        Transform parent = GetNode<Transform>("damageTextGroup");
        if (GameConfig.isDamageUseTextImage) {
            UINumberManager.ShowDamageText(parent, GameSceneManager.ToScreenPoint(position), damage);
            return;
        }
        string itemName = "DamageTextItem";
        LoadItem(itemName, parent, (UIPrefab itemPrefab) => {
            Text textNode = itemPrefab.GetComponent<Text>();
            textNode.text = damage.ToString();
            itemPrefab.transform.position = GameSceneManager.ToScreenPoint(position);
            m_stackDamageText.Push(itemPrefab.transform);
            TimerManager.Register(1, () => {
                m_stackDamageText.Pop();
                GameObject.Destroy(itemPrefab.gameObject);
            });
        });
    }

    public void CreateHPProcess(string nodeName, Vector3 position, float scale = 1, bool isPlayer = false) {
        Transform parent = GetNode<Transform>("hpProcessGroup");
        string itemName = "RoleHPItem";
        LoadItem(itemName, parent, (UIPrefab itemPrefab) => {
            itemPrefab.transform.position = position;
            itemPrefab.transform.localScale = new Vector3(scale, scale);
            itemPrefab.GetNode<UIImage>("hpProcess").FillAmountX = 1;
            m_dicHPProcess.Add(nodeName, itemPrefab);
        });
        if (isPlayer)
            m_playerModelName = nodeName;
    }

    public void MoveHPProcess(string nodeName, Vector3 position, float scale = 1, bool isPlayer = false) {
        if (!m_dicHPProcess.ContainsKey(nodeName) && !isPlayer) {
            DebugTool.LogError("PlayerView MoveHPProcess node is not exit\t" + nodeName);
            return;
        }
        Transform trans = null;
        if (isPlayer)
            trans = m_dicHPProcess[m_playerModelName].transform;
        else
            trans = m_dicHPProcess[nodeName].transform;
        trans.position = position;
        trans.localScale = new Vector3(scale, scale);
    }

    public void UpdateHPProcess(string nodeName, float hpPercent) {
        if (!m_dicHPProcess.ContainsKey(nodeName)) {
            DebugTool.LogError("PlayerView UpdateHPProcess node is not exit\t" + nodeName);
            return;
        }
        UIPrefab prefab = m_dicHPProcess[nodeName];
        UIImage hpProcess = prefab.GetNode<UIImage>("hpProcess");
        hpProcess.FillAmountX = hpPercent;
        if (hpPercent == 0) {
            m_dicHPProcess.Remove(nodeName);
            GameObject.Destroy(prefab.gameObject);
        }
    }

    public void Update() {
        if (GameConfig.isDamageUseTextImage)
            return;
        foreach (Transform trans in m_stackDamageText)
            if (trans)
                trans.Translate(Vector3.up * Time.deltaTime * 60);
    }
}