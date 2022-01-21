using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum biome { mountain, ocean, menu}
public class ObjectController : MonoBehaviour
{
    public static ObjectController instance;

    public float simSpeed;
    public int frameRate;
    public float minParallax;
    public float maxParallax;
    public float atmosAmount;
    //public float TOD = 12;

    //Spawn Locations
    [Header("Spawn Locations")]
    public float lineSpawn = -3;
    public float postSpawnHeight = 0;
    public float linkageSpawnHeight;

    public float cloudSpawnHeightNear;
    public float cloudSpawnHeightMid;
    public float cloudSpawnHeightFar;

    public float mtnSpawnHeightNear;
    public float mtnSpawnHeightMid;
    public float mtnSpawnHeightFar;

    public float islandSpawnHeightMid;
    public float islandSpawnHeightFar;

    //Spawn timers
    [Header("Spawn Timers")]
    float lineLastSpawn = 0;
    public float lineSpawnTime;

    float postLastSpawn = 0;
    public float postSpawnTime;
    float postTimeRandomizer = 0;

    float linkageLastSpawn = 0;
    public float linkageSpawnTime;
    float linkageTimeRandomizer = 0;

    float cloudLastSpawnNear = 2;
    float cloudLastSpawnMid = 2;
    float cloudLastSpawnFar = 2;
    public float cloudSpawnTimeNear;
    public float cloudSpawnTimeMid;
    public float cloudSpawnTimeFar;

    float mtnLastSpawnNear = 2;
    float mtnLastSpawnMid = 2;
    float mtnLastSpawnFar = 2;
    public float mtnSpawnTimeNear;
    public float mtnSpawnTimeMid;
    public float mtnSpawnTimeFar;

    float islandLastSpawnMid = 2;
    float islandLastSpawnFar = 2;
    public float islandSpawnTimeMid;
    public float islandSpawnTimeFar;


    //prefabs
    [Header("Prefabs")]
    public GameObject linePrefab;
    public GameObject postPrefab;
    public GameObject linkagePrefab;

    public GameObject cloudPrefabNear;
    public GameObject cloudPrefabMid;
    public GameObject cloudPrefabFar;

    public GameObject mtnPrefabNear;
    public GameObject mtnPrefabMid;
    public GameObject mtnPrefabFar;

    public GameObject islandPrefabMid;
    public GameObject islandPrefabFar;

    //Parents
    [Header("Parents")]
    public GameObject cloudParent;
    public GameObject roadParent;
    public GameObject mtnParent;
    public GameObject islandParent;

    //Biomes
    [Header("Biome")]
    public biome currentBiome;
    public biome nextBiome;
    public GameObject ocean;
    public GameObject land;

    //Spawn Indexes
    private List<int> nearCloudIndexes = new List<int>();
    private List<int> midCloudIndexes = new List<int>();
    private List<int> farCloudIndexes = new List<int>();

    private List<int> farIslandIndexes = new List<int>();
    private List<int> midIslandIndexes = new List<int>();

    private List<int> farMountainIndexes = new List<int>();
    private List<int> midMountainIndexes = new List<int>();
    private List<int> nearMountainIndexes = new List<int>();

    public List<int> fakeIndexList = new List<int>();


    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = frameRate;
    }

    private void Start()
    {
        currentBiome = BiomeHolder.biomeHolder.currentBiome;
        switch(currentBiome)
        {
            case biome.menu:
                land.SetActive(true);
                ocean.SetActive(false);
                break;
            case biome.mountain:
                land.SetActive(true);
                ocean.SetActive(false);
                break;
            case biome.ocean:
                ocean.SetActive(true);
                land.SetActive(false);
                break;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        SpawnGuardRailPosts();
        SpawnLines();
        SpawnLinkage();
        //spawn cars/trucks

        //Spawn Clouds
        if (SpawnCloud(cloudLastSpawnNear, cloudSpawnTimeNear, cloudSpawnHeightNear, cloudPrefabNear,1f, nearCloudIndexes))
            cloudLastSpawnNear = Time.time;
        if (SpawnCloud(cloudLastSpawnMid, cloudSpawnTimeMid, cloudSpawnHeightMid, cloudPrefabMid,1f, midCloudIndexes))
            cloudLastSpawnMid = Time.time;
        if (SpawnCloud(cloudLastSpawnFar, cloudSpawnTimeFar, cloudSpawnHeightFar, cloudPrefabFar,.5f, farCloudIndexes))
            cloudLastSpawnFar = Time.time;

        //Spawn biome items
        SpawnBiomeItems();

        //See if it's time to change biomes. Don't know how I'm going to do this.

        //Update Time of Day

    }

    void SpawnLines()
    {
        if (Time.time - lineLastSpawn > lineSpawnTime/simSpeed)
        {
            GameObject newLine = Instantiate(linePrefab, new Vector3(10f, lineSpawn, 0), Quaternion.identity);
            lineLastSpawn = Time.time;
            newLine.transform.parent = roadParent.transform;
        }        
    }

    void SpawnGuardRailPosts()
    {
        if (SpawnItem(postLastSpawn+postTimeRandomizer, postSpawnTime, postSpawnHeight, postPrefab, 0, roadParent, false,fakeIndexList))
        {
            postLastSpawn = Time.time;
            postTimeRandomizer = Random.Range(0f, .5f);

            ClearFakeList();
        }
    }

    void SpawnLinkage()
    {
        if (SpawnItem(linkageLastSpawn + linkageTimeRandomizer, linkageSpawnTime, linkageSpawnHeight, linkagePrefab, 0, roadParent, false, fakeIndexList))
        {
            linkageLastSpawn = Time.time;
            linkageTimeRandomizer = Random.Range(0f, .5f);

            ClearFakeList();
        }
    }

    bool SpawnCloud(float lastSpawn, float spawnTime, float spawnHeight, GameObject prefab, float range, List<int> indexList)
    {
        if (Time.time - lastSpawn > spawnTime / simSpeed)
        {
            //Instantiate the cloud with a randomness in height
            float height = Random.Range(-range, range);
            GameObject newCloud = Instantiate(prefab, new Vector3(10f, spawnHeight + height, 0), Quaternion.identity);
            //cloudLastSpawnNear = Time.time;

            //set a random, nonrepeating sprite
            int i = nonRepeatingSpriteIndex(indexList, newCloud.GetComponent<SpriteRandomizer>().spriteList.Length);
            newCloud.GetComponent<SpriteRenderer>().sprite = newCloud.GetComponent<SpriteRandomizer>().spriteList[i];

            //Adjust speed and y scale for height
            MovingObject cloudMO = newCloud.GetComponent<MovingObject>();
            SetParallaxSpeed(cloudMO, true);
            cloudMO.gameObject.transform.localScale = new Vector3(1f + height / 4f, 1f + height / 4f, 1);

            //Adjust Sorting layer order
            newCloud.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(Mathf.Lerp(0, 100, (height / 2*range) + .5f));

            //Apply Atmospheric perspective
            newCloud.GetComponent<SpriteRenderer>().material.SetFloat("_PerspectiveAmount", Mathf.Lerp(atmosAmount, 0, ((cloudMO.transform.position.y + 1.5f) / 5.5f)));

            //Assign to Cloud container
            newCloud.transform.parent = cloudParent.transform;

            return true;
        }
        return false;
    }

    bool SpawnItem(float lastSpawn, float spawnTime, float spawnHeight, GameObject prefab, float range, GameObject parent, bool hasParallax, List<int> indexList)
    {
        if (Time.time - lastSpawn > spawnTime / simSpeed)
        {
            //Instantiate the item with a randomness in height
            float height = Random.Range(-range, range);
            GameObject newItem = Instantiate(prefab, new Vector3(10f, spawnHeight + height, 0), Quaternion.identity);

            //set a random sprite
            int i = nonRepeatingSpriteIndex(indexList, newItem.GetComponent<SpriteRandomizer>().spriteList.Length);
            //int i = Random.Range(0, newItem.GetComponent<SpriteRandomizer>().spriteList.Length);
            newItem.GetComponent<SpriteRenderer>().sprite = newItem.GetComponent<SpriteRandomizer>().spriteList[i];

            //Adjust speed and y scale for height
            MovingObject itemMO = newItem.GetComponent<MovingObject>();
            //itemMO.speed *= 1f - height / 5;
            if (hasParallax) SetParallaxSpeed(itemMO, false);
            //cloudMO.gameObject.transform.localScale = new Vector3(1, 1f + height / 2, 1);

            //Adjust Sorting layer order
            newItem.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(Mathf.Lerp(100, 0, (height / 2 * range) + .5f));

            //Apply Atmospheric perspective
            newItem.GetComponent<SpriteRenderer>().material.SetFloat("_PerspectiveAmount", Mathf.Lerp(atmosAmount, 0, (-newItem.transform.position.y-.5f) / 4.5f));

            //Assign to Container
            newItem.transform.parent = parent.transform;


            return true;
        }
        return false;
    }

    void SpawnBiomeItems()
    {
        //Yeah, spawn them here.
        switch(currentBiome)
        {
            case biome.ocean:
                //Debug.Log("Spawning Ocean");
                if (SpawnItem(islandLastSpawnFar, islandSpawnTimeFar, islandSpawnHeightFar, islandPrefabFar, 0f, islandParent, true, farIslandIndexes))
                    islandLastSpawnFar = Time.time;
                if (SpawnItem(islandLastSpawnMid, islandSpawnTimeMid, islandSpawnHeightMid, islandPrefabMid, 0.5f, islandParent, true, midIslandIndexes))
                    islandLastSpawnMid = Time.time;
                break;

            case biome.mountain:
                //Debug.Log("Spawning Mountain");
                //Spawn close, mid, and far mountains
                if (SpawnItem(mtnLastSpawnNear, mtnSpawnTimeNear, mtnSpawnHeightNear, mtnPrefabNear, .25f, mtnParent, true, nearMountainIndexes))
                    mtnLastSpawnNear = Time.time;
                if (SpawnItem(mtnLastSpawnMid, mtnSpawnTimeMid, mtnSpawnHeightMid, mtnPrefabMid, .25f, mtnParent, true, midMountainIndexes))
                    mtnLastSpawnMid = Time.time;
                if (SpawnItem(mtnLastSpawnFar, mtnSpawnTimeFar, mtnSpawnHeightFar, mtnPrefabFar, 0f, mtnParent, true, farMountainIndexes))
                    mtnLastSpawnFar = Time.time;
                break;

            case biome.menu:
                //Debug.Log("Spawning Forest");
                break;
        }
    }

    void SetParallaxSpeed(MovingObject movingObject, bool objectInSky)
    {
        float objectHeight = movingObject.gameObject.transform.position.y;
        //scale speed between .001 and .004
        if (objectInSky)
            movingObject.speed = Mathf.Lerp(minParallax, maxParallax, ((objectHeight+1.5f) / 5.5f));
        else
            movingObject.speed = Mathf.Lerp(minParallax, maxParallax, (-objectHeight / 4.5f));
    }

    int nonRepeatingSpriteIndex(List<int> checkList, int spriteListLength)
    {
        int i = Random.Range(0, spriteListLength);
        while (checkList.Contains(i))
        {
            //Debug.Log("found in index list: " + i.ToString() + ". Trying again");
            i = Random.Range(0, spriteListLength);
        }
        checkList.Add(i);
        if (checkList.Count > 3)
        {
            //Debug.Log("more than 3");
            checkList.RemoveAt(0);
        }
        return i;
    }

    void ClearFakeList()
    {
        for (int i = 0; i<5; i++)
        {
            fakeIndexList.Add(100);
            fakeIndexList.RemoveAt(0);
        }
    }
}
