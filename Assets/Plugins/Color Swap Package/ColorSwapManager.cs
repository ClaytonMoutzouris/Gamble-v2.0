using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSwapNode
{
    public int rIndex;
    [HideInInspector]
    public Color trueColour;
    public Color baseColour;
}

public class ColorSwapper
{

    public Texture2D baseTexture;
    public Texture2D mColorSwapTex;
    public Material material;


    public ColorSwapper(Material mat)
    {
        baseTexture = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        mColorSwapTex = baseTexture;
        material = mat;
        //InitColorSwapTex();
    }

    public void InitColorSwapTex()
    {
        Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        colorSwapTex.filterMode = FilterMode.Point;

        for (int i = 0; i < colorSwapTex.width; ++i)
            colorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        colorSwapTex.Apply();

        material.SetTexture("_SwapTex", colorSwapTex);

        // mSpriteColors = new Color[colorSwapTex.width];
        mColorSwapTex = colorSwapTex;
        baseTexture = colorSwapTex;
    }

    public void SwapColor(ColorSwapNode node)
    {
        // mSpriteColors[(int)index] = color;
        mColorSwapTex.SetPixel(node.rIndex, 0, node.trueColour);
        mColorSwapTex.Apply();

    }

    public void SwapColorToBase(ColorSwapNode node)
    {
        mColorSwapTex.SetPixel(node.rIndex, 0, node.baseColour);
        mColorSwapTex.Apply();
    }


    public void SwapColors(List<ColorSwapNode> nodes)
    {

        for (int i = 0; i < nodes.Count; ++i)
        {
            //mSpriteColors[(int)indexes[i]] = colors[i];
            mColorSwapTex.SetPixel((int)nodes[i].rIndex, 0, nodes[i].trueColour);
        }
        mColorSwapTex.Apply();
    }

    public void SwapColorsToBase(List<ColorSwapNode> nodes)
    {
        for (int i = 0; i < nodes.Count; ++i)
        {
            //mSpriteColors[(int)indexes[i]] = colors[i];
            mColorSwapTex.SetPixel((int)nodes[i].rIndex, 0, nodes[i].trueColour);
        }
        mColorSwapTex.Apply();
    }

    public void SwapAllSpritesColors(Color color)
    {
        for (int i = 0; i < mColorSwapTex.width; ++i)
            mColorSwapTex.SetPixel(i, 0, color);
        mColorSwapTex.Apply();
    }

}
