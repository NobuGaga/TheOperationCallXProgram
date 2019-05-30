using UnityEngine;
using UnityEngine.UI;
using System;

public class UINumberImage : Image {
    [HideInInspector]
    public int Number;
    private void SetNumber(string numberString) {
        if (numberString == "-") {
            Number = -1;
            return;
        }
        bool isSuccess = int.TryParse(numberString, out Number);
        if (!isSuccess)
            Number = -2;
    }

    public bool IsErrorNumber {
        get {
            return Number == -2;
        }
    }

    [HideInInspector]
    public NumberColor Color = NumberColor.Default;

    public new Sprite sprite {
        set {
            base.sprite = value;
            if (value == null)
                return;
            SetNativeSize();
            string[] spriteName = value.name.Split('_');
            if (spriteName.Length < 2 && spriteName[0] != GameConst.SpriteNumberType) {
                DebugTool.LogError("sprite name is not number type");
                return;
            }
            if (spriteName.Length < 3) {
                SetNumber(spriteName[1]);
                return;
            }
            SetNumber(spriteName[2]);
            foreach (NumberColor color in Enum.GetValues(typeof(NumberColor)))
                if (color.ToString().ToLower() == spriteName[1]) {
                    Color = color;
                    break;
                }
        }
    }
}