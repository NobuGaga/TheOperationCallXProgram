using UnityEngine;
using UnityEngine.UI;

public class UIImage:Image {
    public float FillAmountX {
        set {
            if (type == Type.Sliced) {
                transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
            }else
                fillAmount = value;
        }
    }

    public float FillAmountY {
        set {
            if (type == Type.Sliced) {
                transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
            }else
                fillAmount = value;
        }
    }
}