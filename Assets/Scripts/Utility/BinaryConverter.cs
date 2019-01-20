using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BinaryConverter
{
    public static BitArray ToBinary(this int numeral)
    {
        return new BitArray(new[] { numeral });
    }

    public static int ToNumeral(this BitArray binary)
    {
        if (binary == null)
            throw new ArgumentNullException("binary");
        if (binary.Length > 32)
            throw new ArgumentException("must be at most 32 bits long");

        var result = new int[1];
        binary.CopyTo(result, 0);
        //Debug.Log("Converted " + binary + " to Int " + result[0]);
        
        return result[0];
    }
}
