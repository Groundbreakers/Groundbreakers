using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class SectorGenerator : MonoBehaviour
{
    public GameObject UI;
    public GameObject Prefab;
    public GameObject Dummy;
    public GameObject Base;
    public int Height;

    private int heightMultiplier = 100;
    private int depthMultiplier = 50;

    private int[,] sectorBool;
    private GameObject[,] sectors;

    public void Initialize()
    {
        this.sectorBool = new int[this.Height + 1, 5];

        if (this.Height > 1)
        {
            this.sectorBool[0, 2] = 1;
            this.sectorBool[1, 1] = this.sectorBool[1, 2] = this.sectorBool[1, 3] = 1;
        }

        for (int i = 1; i < this.Height; i++)
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

        this.sectors = new GameObject[this.Height + 1, 5];

        this.sectors[0, 0] = (GameObject)Instantiate(this.Dummy, this.UI.transform);
        this.sectors[0, 1] = (GameObject)Instantiate(this.Dummy, this.UI.transform);
        this.sectors[0, 2] = (GameObject)Instantiate(this.Base, this.UI.transform);
        this.sectors[0, 3] = (GameObject)Instantiate(this.Dummy, this.UI.transform);
        this.sectors[0, 4] = (GameObject)Instantiate(this.Dummy, this.UI.transform);

        for (int i = 1; i < this.Height + 1; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (this.sectorBool[i, j] == 1)
                {
                    this.sectors[i, j] = (GameObject)Instantiate(this.Prefab, this.UI.transform);
                    this.sectors[i, j].GetComponent<Sector>().Setup(i, j, "Battle", i * this.depthMultiplier);
                    if (i == 1)
                    {
                        this.sectors[i, j].GetComponent<Sector>().ShowButton();
                        if (j == 1)
                        {
                            this.sectors[i, j].GetComponent<Sector>().ShowPath('R');
                        }
                        else if (j == 2)
                        {
                            this.sectors[i, j].GetComponent<Sector>().ShowPath('M');
                        }
                        else if (j == 3)
                        {
                            this.sectors[i, j].GetComponent<Sector>().ShowPath('L');
                        }
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
        if (row == this.Height)
        {
            return;
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
