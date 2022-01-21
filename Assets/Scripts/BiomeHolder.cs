using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BiomeHolder : MonoBehaviour
{
    public biome currentBiome;
    public static BiomeHolder biomeHolder;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        biomeHolder = this;
    }

    public void LoadBiome(biome targetBiome)
    {
        currentBiome = targetBiome;
        
        switch (currentBiome)
        {
            case biome.menu:
                SceneManager.LoadScene(0);
                break;
            case biome.mountain:
                SceneManager.LoadScene(1);
                break;
            case biome.ocean:
                SceneManager.LoadScene(1);
                break;
        }
    }
}
