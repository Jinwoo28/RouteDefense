using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSettings : MonoBehaviour
{
    public struct tileshpae
    {
        public int x;
        public int y;
    }

    private int Size = 8;

    tileshpae[] tileset;
    int tilenum = 0;

    private int mapNumber = 0;

    private int[,] mapShape = null;
    public int[,] MapShape => mapShape;

    private List<tileshpae> tileshpaes = new List<tileshpae>();

    private void Start()
    {

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                tileshpae shape = new tileshpae();

                if (i < 4 && j < 4)
                {

                    if (i + j < 2) continue;
                    else
                    {
                        shape.x = i;
                        shape.y = j;
                    }
                }
                else if (i >= 4 && j < 4)
                {
                    if (i - j >= 6) continue;
                    else
                    {
                        shape.x = i;
                        shape.y = j;
                    }
                }
                else if (i < 4 && j >= 4)
                {
                    if (j - i >= 6) continue;
                    else
                    {
                        shape.x = i;
                        shape.y = j;
                    }
                }
                else if (i >= 4 && j >= 4)
                {
                    if (i + j >= 13) continue;
                    else
                    {
                        shape.x = i;
                        shape.y = j;
                    }
                }

                tileshpaes.Add(shape);

            }
        }
    }


}
