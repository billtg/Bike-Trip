using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject UICanvas;
    public void LoadMountains()
    {
        LoadBiome(biome.mountain);
    }

    public void LoadOcean()
    {
        LoadBiome(biome.ocean);
    }

    public void HideUI()
    {
        UICanvas.SetActive(false);
    }

    public void LoadBiome(biome clickedBiome)
    {
        Debug.Log("Clicked " + clickedBiome.ToString());
        //Load main with the biome selected
        HideUI();
        BiomeHolder.biomeHolder.LoadBiome(clickedBiome);
    }
}
