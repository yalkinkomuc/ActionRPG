using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;
    public virtual void adjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePosition.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }

    public virtual void AdjustFontsize(TextMeshProUGUI text)
    {
        if (text.text.Length > 12)
            text.fontSize = text.fontSize * .8f;

    }
}
