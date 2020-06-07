using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColorSwap {

    public Texture2D mColorSwapTex;
    public Material material;
    public Color[] mBaseColors;
    public List<Color> mCurrentColors;

    public ColorSwap(Material mat)
    {
        mColorSwapTex = new Texture2D(256, 1);
        material = mat;
        InitColorSwapTex();
    }

    public void SetBaseColors(List<Color> colors)
    {
        mBaseColors = new Color[7];
        mBaseColors[0] = colors[0];
        mBaseColors[1] = colors[1];
        mBaseColors[2] = colors[2];
        mBaseColors[3] = colors[3];
        mBaseColors[4] = colors[4];
        mBaseColors[5] = colors[5];
        mBaseColors[6] = colors[6];

        SwapToBase();
    }

    public void SwapIndexToBase(SwapIndex index)
    {
        switch (index)
        {
            case SwapIndex.Skin:
                SwapColor(index, mBaseColors[0]);
                break;
            case SwapIndex.HoodPrimary:
                SwapColor(index, mBaseColors[1]);
                break;
            case SwapIndex.HoodSecondary:
                SwapColor(index, mBaseColors[2]);
                break;
            case SwapIndex.ShirtPrimary:
                SwapColor(index, mBaseColors[3]);
                break;
            case SwapIndex.ShirtSecondary:
                SwapColor(index, mBaseColors[4]);
                break;
            case SwapIndex.Shoes:
                SwapColor(index, mBaseColors[5]);
                break;
            case SwapIndex.Pants:
                SwapColor(index, mBaseColors[6]);
                break;
        }

        mColorSwapTex.Apply();
    }

    public void SwapToBase()
    {
      //~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        SwapColor(SwapIndex.Skin, mBaseColors[0]);
        //SwapColor(SwapIndex.Eyes, Color.red);
        SwapColor(SwapIndex.HoodPrimary, mBaseColors[1]);
        SwapColor(SwapIndex.HoodSecondary, mBaseColors[2]);
        SwapColor(SwapIndex.ShirtPrimary, mBaseColors[3]);
        SwapColor(SwapIndex.ShirtSecondary, mBaseColors[4]);
        SwapColor(SwapIndex.Shoes, mBaseColors[5]);
        SwapColor(SwapIndex.Pants, mBaseColors[6]);
        mColorSwapTex.Apply();
    }

    public void LoadSwapTexture(){

    }

    // Use this for initialization
    public void SwapSpritesTexture (List<Color> colors) {

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        SwapColor(SwapIndex.Skin, colors[0]);
        //SwapColor(SwapIndex.Eyes, Color.red);
        SwapColor(SwapIndex.HoodPrimary, colors[1]);
        SwapColor(SwapIndex.HoodSecondary, colors[2]);
        SwapColor(SwapIndex.ShirtPrimary, colors[3]);
        SwapColor(SwapIndex.ShirtSecondary, colors[4]);
        SwapColor(SwapIndex.Shoes, colors[5]);
        SwapColor(SwapIndex.Pants, colors[6]);
        mColorSwapTex.Apply();


    }

    public void SwapSpritesTexture(SwapIndex index, Color color)
    {

        //InitColorSwapTex(sR);
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        SwapColor(index, color);

        mColorSwapTex.Apply();


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
    }

    public void SwapColor(SwapIndex index, Color color)
    {
       // mSpriteColors[(int)index] = color;
        mColorSwapTex.SetPixel((int)index, 0, color);
        mColorSwapTex.Apply();

    }


    public void SwapColors(List<SwapIndex> indexes, List<Color> colors)
    {
        for (int i = 0; i < indexes.Count; ++i)
        {
            //mSpriteColors[(int)indexes[i]] = colors[i];
            mColorSwapTex.SetPixel((int)indexes[i], 0, colors[i]);
        }
        mColorSwapTex.Apply();

        mCurrentColors = colors;
    }

    public void ClearColor(SwapIndex index)
    {
        Color c = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        //mSpriteColors[(int)index] = c;
        mColorSwapTex.SetPixel((int)index, 0, c);
    }

    public void SwapAllSpritesColors(Color color)
    {
        for (int i = 0; i < mColorSwapTex.width; ++i)
            mColorSwapTex.SetPixel(i, 0, color);
        mColorSwapTex.Apply();
    }
}
