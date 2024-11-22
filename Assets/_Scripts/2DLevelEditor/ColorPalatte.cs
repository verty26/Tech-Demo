using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPalatte : MonoBehaviour
{
    public void OnClick()
    {
        PixelLevelEditor.instance.SetCurrentColor(GetComponent<Image>().color);
    }
}
