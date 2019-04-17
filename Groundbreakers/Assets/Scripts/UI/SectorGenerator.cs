using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class SectorGenerator : MonoBehaviour
{
    public GameObject UI;
    public GameObject Prefab;
    public GameObject Dummy;
    public int Height;

    private int heightMultiplier = 120;
    private int depthMultiplier = 50;

    private int[,] sectorBool;
    private GameObject[,] sectors;

    void Start()
    {
        this.Initialize();
    }

    public void Initialize()
    {
        this.sectorBool = new int[this.Height, 5];

        if (this.Height > 0)
        {
            this.sectorBool[0, 1] = this.sectorBool[0, 2] = this.sectorBool[0, 3] = 1;
        }

        for (int i = 0; i < this.Height - 1; i++)
        {
            int sectorCount = this.sectorBool[i, 0] + this.sectorBool[i, 1] + this.sectorBool[i, 2]
                              + this.sectorBool[i, 3] + this.sectorBool[i, 4];
            int nextSectorCount = 0;
            while (nextSectorCount == 0)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (this.sectorBool[i, j] != 0)
                    {
                        if (j == 0)
                        {
                            if (Random.Range(0, sectorCount) == 0)
                            {
                                this.sectorBool[i + 1, 0] = 1;
                                nextSectorCount++;
                            }

                            if (Random.Range(0, sectorCount) == 0)
                            {
                                this.sectorBool[i + 1, 1] = 1;
                                nextSectorCount++;
                            }
                        }
                        else if (j == 4)
                        {
                            if (Random.Range(0, sectorCount) == 0)
                            {
                                this.sectorBool[i + 1, 3] = 1;
                                nextSectorCount++;
                            }

                            if (Random.Range(0, sectorCount) == 0)
                            {
                                this.sectorBool[i + 1, 4] = 1;
                                nextSectorCount++;
                            }
                        }
                        else
                        {
                            if (Random.Range(0, sectorCount) == 0)
                            {
                                this.sectorBool[i + 1, j - 1] = 1;
                                nextSectorCount++;
                            }

                            if (Random.Range(0, sectorCount) == 0)
                            {
                                this.sectorBool[i + 1, j] = 1;
                                nextSectorCount++;
                            }

                            if (Random.Range(0, sectorCount) == 0)
                            {
                                this.sectorBool[i + 1, j + 1] = 1;
                                nextSectorCount++;
                            }
                        }
                    }
                }
            }
        }

        this.UI.GetComponent<RectTransform>().sizeDelta = new Vector2(this.UI.GetComponent<RectTransform>().sizeDelta.x, this.heightMultiplier);

        this.sectors = new GameObject[this.Height, 5];

        for (int i = 0; i < this.Height; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (this.sectorBool[i, j] == 1)
                {
                    this.sectors[i, j] = (GameObject)Instantiate(this.Prefab, this.UI.transform);
                    this.sectors[i, j].GetComponent<Sector>().Setup(i, j, "Battle", (i + 1) * this.depthMultiplier);
                    if (i == 0)
                    {
                        this.sectors[i, j].GetComponent<Sector>().ShowButton();
                    }
                }
                else
                {
                    this.sectors[i, j] = (GameObject)Instantiate(this.Dummy, this.UI.transform);
                }
            }
        }
    }

    public void ShowPath(int row, int column)
    {
        if (row == this.Height - 1)
        {
            return;
        }

        if (this.UI.GetComponent<RectTransform>().sizeDelta.y < this.heightMultiplier * (row + 3))
        {
            this.UI.GetComponent<RectTransform>().sizeDelta = new Vector2(this.UI.GetComponent<RectTransform>().sizeDelta.x, this.heightMultiplier * (row + 3));
        }

        if (column == 0)
        {
            if (this.sectors[row + 1, column].GetComponent<Sector>() != null)
            {
                this.sectors[row + 1, column].GetComponent<Sector>().ShowPath('M');
            }

            if (this.sectors[row + 1, column + 1].GetComponent<Sector>() != null)
            {
                this.sectors[row + 1, column + 1].GetComponent<Sector>().ShowPath('L');
            }
        }
        else if (column == 4)
        {
            if (this.sectors[row + 1, column].GetComponent<Sector>() != null)
            {
                this.sectors[row + 1, column].GetComponent<Sector>().ShowPath('M');
            }

            if (this.sectors[row + 1, column - 1].GetComponent<Sector>() != null)
            {
                this.sectors[row + 1, column - 1].GetComponent<Sector>().ShowPath('R');
            }
        }
        else
        {
            if (this.sectors[row + 1, column - 1].GetComponent<Sector>() != null)
            {
                this.sectors[row + 1, column - 1].GetComponent<Sector>().ShowPath('R');
            }

            if (this.sectors[row + 1, column].GetComponent<Sector>() != null)
            {
                this.sectors[row + 1, column].GetComponent<Sector>().ShowPath('M');
            }

            if (this.sectors[row + 1, column + 1].GetComponent<Sector>() != null)
            {
                this.sectors[row + 1, column + 1].GetComponent<Sector>().ShowPath('L');
            }
        }
            
    }
}
