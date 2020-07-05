using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtensions
{
    public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect instance, Transform child)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );
        return result;
    }

    public static void ScrollRepositionY(this ScrollRect instance, RectTransform obj)
    {
        Canvas.ForceUpdateCanvases();

        var objPosition = (Vector2)instance.transform.InverseTransformPoint(obj.position);
        var scrollHeight = instance.GetComponent<RectTransform>().rect.height;
        var objHeight = obj.rect.height;

        if (objPosition.y > scrollHeight / 2 - 5)
        {
            instance.content.localPosition = new Vector2(instance.content.localPosition.x,
                instance.content.localPosition.y - objHeight - 10);
        }

        if (objPosition.y < -scrollHeight / 2 + 5)
        {
            instance.content.localPosition = new Vector2(instance.content.localPosition.x,
            instance.content.localPosition.y + objHeight + 10);
        }
    }

    public static void ScrollRepositionX(this ScrollRect instance, RectTransform obj)
    {
        Canvas.ForceUpdateCanvases();

        var objPosition = (Vector2)instance.transform.InverseTransformPoint(obj.position);
        var scrollWidth = instance.GetComponent<RectTransform>().rect.width;
        var objWidth = obj.rect.width;

        if (objPosition.x > scrollWidth / 2 - 15)
        {
            instance.content.localPosition = new Vector2(instance.content.localPosition.x - objWidth - 10,
                instance.content.localPosition.y);
        }

        if (objPosition.x < -scrollWidth / 2 + 15)
        {
            instance.content.localPosition = new Vector2(instance.content.localPosition.x + objWidth + 10,
            instance.content.localPosition.y);
        }
    }
}