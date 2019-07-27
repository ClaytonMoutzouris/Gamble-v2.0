using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColorSwap {

    public Texture2D mColorSwapTex;
    public SpriteRenderer sR;
    //static Color[] mSpriteColors;

    public ColorSwap(SpriteRenderer renderer)
    {
        mColorSwapTex = new Texture2D(256, 1);
        sR = renderer;
        InitColorSwapTex();
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

        sR.material.SetTexture("_SwapTex", colorSwapTex);

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
