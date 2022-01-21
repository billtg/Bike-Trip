using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject UICanvas;
    public static InputManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //singleton
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    void Update()
    {
        //Check for 'Esc'
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Go to menu or quit
            if(BiomeHolder.biomeHolder.currentBiome == biome.menu)
            {
                //Debug.Log("Menu");
                if (UICanvas.activeSelf)
                    Application.Quit();
                else
                    UICanvas.SetActive(true);
            } else
            {
                BiomeHolder.biomeHolder.LoadBiome(biome.menu);
                UICanvas.SetActive(true);
            }
        }

        //Check for sim speed
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ObjectController.instance.simSpeed = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ObjectController.instance.simSpeed = 10;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ObjectController.instance.simSpeed = 50;
    }
}
