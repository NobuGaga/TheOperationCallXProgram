using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UINumberManager {
    private static Dictionary<NumberColor, Dictionary<int, List<Transform>>> dicNumber;
    private static Dictionary<NumberColor, Dictionary<int, Sprite>> dicSprite;
    private static Stack<Transform> groupPool;
    private static List<Transform> listGroup;
    private static Object[] loadedAssets;
    private static GameObject numberPrefab;
    private static GameObject numberGroup;

    public static void Init(System.Action callback) {
        AssetBundleManager.Load(PathConfig.AssetBundleNumberAtlasPath, 
            (AssetBundle assetBundle) => {
                AssetBundleRequest request = assetBundle.LoadAllAssetsAsync<Sprite>();
                request.completed += (AsyncOperation operation) => {
                    loadedAssets = request.allAssets;
                    InitCallBack(callback);
                };
            }
        );
        AssetBundleManager.Load(PathConfig.AssetBundleNumberPrefabPath,
            (AssetBundle assetBundle) => {
                AssetBundleRequest requestItem = assetBundle.LoadAssetAsync<GameObject>("NumberItem");
                requestItem.completed += (AsyncOperation operation) => {
                    numberPrefab = requestItem.asset as GameObject;
                    InitCallBack(callback);
                };
                AssetBundleRequest requestGroup = assetBundle.LoadAssetAsync<GameObject>("NumberGroup");
                requestGroup.completed += (AsyncOperation operation) => {
                    numberGroup = requestGroup.asset as GameObject;
                    InitCallBack(callback);
                };
            }
        );
    }

    private static void InitCallBack(System.Action callback) {
        if (loadedAssets == null || numberPrefab == null || numberGroup == null)
            return;
        callback();
        InitSpriteCache();
        EventManager.Register(GameEvent.Type.FrameUpdate, FrameUpdate);
    }

    private static void InitSpriteCache() {
        dicNumber = new Dictionary<NumberColor, Dictionary<int, List<Transform>>>();
        dicSprite = new Dictionary<NumberColor, Dictionary<int, Sprite>>();
        groupPool = new Stack<Transform>();
        listGroup = new List<Transform>();
        for (int i = 0; i < loadedAssets.Length; i++) {
            Sprite sprite = loadedAssets[i] as Sprite;
            GameObject obj = GameObject.Instantiate(numberPrefab);
            obj.transform.SetParent(GameManager.LogicScript.transform);
            UINumberImage image = obj.GetComponent<UINumberImage>();
            image.sprite = sprite;
            NumberColor color = image.Color;
            int number = image.Number;
            AddImageData(obj.transform, color, number);
            if (!dicSprite.ContainsKey(color))
                dicSprite.Add(color, new Dictionary<int, Sprite>());
            if (!dicSprite[color].ContainsKey(number))
                dicSprite[color].Add(number, sprite);
        }
    }

    private static void AddImageData(Transform numberTrans, NumberColor color, int number) {
        if (!dicNumber.ContainsKey(color))
            dicNumber.Add(color, new Dictionary<int, List<Transform>>());
        if (!dicNumber[color].ContainsKey(number))
            dicNumber[color].Add(number, new List<Transform>());
        dicNumber[color][number].Add(numberTrans);
        numberTrans.gameObject.SetActive(false);
    }

    private static IEnumerator CreateImage(NumberColor color, int number, System.Action<Transform> callback) {
        if (!dicSprite.ContainsKey(color)) {
            DebugTool.LogError("UINumberManager not exit color\t" + color.ToString());
            yield break;
        }
        if (!dicSprite[color].ContainsKey(number)) {
            DebugTool.LogError("UINumberManager not exit color number\t" + number);
            yield break;
        }
        GameObject obj = GameObject.Instantiate(numberPrefab);
        UINumberImage image = obj.GetComponent<UINumberImage>();
        image.sprite = dicSprite[color][number];
        AddImageData(obj.transform, color, number);
        callback(obj.transform);
        yield break;
    }

    public static void ShowDamageText(Transform parent, Vector3 position, int damage, NumberColor color = NumberColor.Default) {
        Transform group = null;
        if (groupPool.Count == 0 || (group = groupPool.Pop()) == null)
            GameManager.LogicScript.StartCoroutine(CreateGroup(color,
                (Transform groupTrans) => {
                    groupTrans.SetParent(parent);
                    groupTrans.position = position;
                    AddNumberToGroup(groupTrans, damage, color);
                    listGroup.Add(groupTrans);
                })
            );
        else {
            group.position = position;
            AddNumberToGroup(group, damage, color);
            listGroup.Add(group);
        }
    }

    private static IEnumerator CreateGroup(NumberColor color, System.Action<Transform> callback) {
        GameObject obj = GameObject.Instantiate(numberGroup);
        Transform groupTrans = obj.GetComponent<Transform>();
        callback(groupTrans);
        yield break;
    }

    private static void AddSubToList(NumberColor color, System.Action<Transform> callback) {
        List<Transform> list = dicNumber[color][-1];
        Transform node = null;
        for (int i = 0; i < list.Count; i++)
            if (!list[i].gameObject.activeSelf) {
                node = list[i];
                list.RemoveAt(i);
                break;
            }
        if (node == null)
            GameManager.LogicScript.StartCoroutine(
                CreateImage(color, -1, (Transform numberImage) => callback(numberImage)));
        else
            callback(node);
    }

    private static void AddNumberToGroup(Transform group, int damage, NumberColor color) {
        if (!dicNumber.ContainsKey(color)) {
            DebugTool.LogError("UINumberManager not exit color image, color\t" + color.ToString());
            return;
        }
        bool isHaveSub = dicSprite.ContainsKey(color) && dicSprite[color].ContainsKey(-1);
        int length = CalculateDamageLength(damage);
        if (isHaveSub)
            length++;
        Transform[] arrayNumImages = new Transform[length];
        if (isHaveSub)
            AddSubToList(color, (Transform sub) => {
                arrayNumImages[0] = sub;
                LoadCompletedCallBack(color, arrayNumImages, group);
            });
        AddNumberToList(damage, color, (Transform numberImage, int index) => {
            arrayNumImages[index] = numberImage;
            LoadCompletedCallBack(color, arrayNumImages, group);
        }, length - 1);
    }

    private static int CalculateDamageLength(int damage) {
        int newDamage = damage / 10;
        int rest = damage % 10;
        int length = 1;
        while ((damage /= 10) > 0)
            length++;
        return length;
    }

    private static void LoadCompletedCallBack(NumberColor color, Transform[] arrayNumImages, Transform group) {
        for (int i = 0; i < arrayNumImages.Length; i++)
            if (arrayNumImages[i] == null)
                return;
        for (int i = 0; i < arrayNumImages.Length; i++) {
            arrayNumImages[i].SetParent(group);
            arrayNumImages[i].gameObject.SetActive(true);
        }
        TimerManager.Register(1, () => {
            listGroup.Remove(group);
            UINumberImage[] images = group.GetComponentsInChildren<UINumberImage>();
            for (int i = 0; i < images.Length; i++) {
                Transform trans = images[i].transform;
                AddImageData(trans, color, images[i].Number);
                trans.SetParent(group.parent);
            }
            groupPool.Push(group);
        });
    }

    private static void AddNumberToList(int damage, NumberColor color, System.Action<Transform, int> callback, int index) {
        int newDamage = damage / 10;
        int rest = damage % 10;
        if (newDamage > 0)
            AddNumberToList(newDamage, color, callback, index - 1);
        if (!dicNumber[color].ContainsKey(rest)) {
            DebugTool.LogError("UINumberManager not exit color number image, number\t" + rest);
            return;
        }
        List<Transform> list = dicNumber[color][rest];
        Transform node = null;
        for (int i = 0; i < list.Count; i++)
            if (!list[i].gameObject.activeSelf) {
                node = list[i];
                list.RemoveAt(i);
                break;
            }
        if (node == null)
            GameManager.LogicScript.StartCoroutine(
                CreateImage(color, rest, (Transform numberImage) => callback(numberImage, index)));
        else
            callback(node, index);
    }

    private static void FrameUpdate(object arg) {
        for (int i = 0; i < listGroup.Count; i++) {
            listGroup[i].Translate(Vector3.up * Time.deltaTime * 60);
        }
    }
}

public enum NumberColor {
    Default,
}