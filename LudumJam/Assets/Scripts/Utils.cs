using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class Utils  
{
    public static void SetOpacity(this Image image, float opacity)
    {
        var color = image.color;
        color.a = opacity;
        image.color = color;
    }
}
