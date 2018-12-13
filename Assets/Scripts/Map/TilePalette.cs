using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePalette {

    public Color[][] tileTextures;

    public TilePalette(Texture2D texture, int tileResolution)
    {
        int numTilesPerRow = texture.width / tileResolution;
        int numRows = texture.height / tileResolution;

        tileTextures = new Color[numTilesPerRow * numRows][];

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numTilesPerRow; x++)
            {
                tileTextures[y * numTilesPerRow + x] = texture.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
            }
        }


    }

    public Color[] GetTile(int index)
    {
        return tileTextures[index];
    }

}
