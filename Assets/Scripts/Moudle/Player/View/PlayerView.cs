using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerView:View {
    private Stack<Transform> m_damageText = new Stack<Transform>();

    public PlayerView(GameMoudle moudle, GameView view, UIPrefab prefab) : base(moudle, view, prefab) {
        GetNode<UIImage>("hpProcess").FillAmountX = 1;
    }

    public void UpdateHP(float hp) {
        GetNode<UIImage>("hpProcess").FillAmountX = hp;
    }

    public void ShowDamageText(Transform target, string damage) {
        string itemName = "DamageTextItem";
        Transform parent = GetNode<Transform>("damageTextGroup");
        LoadItem(itemName, parent, (UIPrefab itemPrefab) => {
            Text textNode = itemPrefab.GetComponent<Text>();
            textNode.text = damage;
            itemPrefab.transform.position = Camera.main.WorldToScreenPoint(target.position);
            m_damageText.Push(itemPrefab.transform);
            TimerManager.Register(1, () => {
                m_damageText.Pop();
                Object.Destroy(itemPrefab.gameObject);
            });
        });
    }

    public void Update() {
        foreach (Transform trans in m_damageText)
            if (trans)
                trans.Translate(Vector3.up * Time.deltaTime * 60);
    }
}