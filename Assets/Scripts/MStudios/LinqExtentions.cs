using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MStudios
{
    public static class LinqExtentions
    {
        public static IEnumerable<Vector2Int> Where<T>(this T[,] source, Func<T, bool> predicate)
        {
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    if (predicate(source[i, j]))
                    {
                        yield return new Vector2Int(j,i);
                    }
                }    
            }
        }
    }
}