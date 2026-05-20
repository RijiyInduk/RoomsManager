using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager ins;

    //light
    public GameObject mainDirLight;
    public int timeDay;//Times of Day; 0-Morning, 1-Day, 2-Evening, 3-Night; 
    public string inoutDoor;// indoor - outdoor
    public string nameRoom;
    [Space]
    public GameObject[] lightsMainRooms;//0-Morning, 1-Day, 2-Evening, 3-Night; 
    public GameObject[] lightsFightRooms;//0-Morning, 1-Day, 2-Evening, 3-Night; 
    public GameObject[] lightsOtherRooms;//0-AboundonedHouse, 1-Cave, 2-Fiol, 3-EveningLight
    public GameObject[] lightsFOtherRooms;//0-AboundonedHouse, 1-Cave, 2-Fiol, 3-EveningLight
    [Space]
    public int roomIndexRND = 0;
    public GameObject[] forestFloors;
    public GameObject[] caveFloors;
    public GameObject[] townFloors;
    public GameObject[] towerFloors;
    public GameObject[] cellarFloors;
    public GameObject[] otherFloors;//0-Sawmill, 1-WarCamp, 2-Swamp, 3-Graveyard, 4-AbondHouse1, 5-AbondHouse2, 6-CaveAltar, 7-CavePortal, 8-TownStorage, 9-TownLairOfBandits
    public GameObject[] campFloors;//0-Forest, 1-Cave, 2-Town, 3-Tower
    [Space]
    public GameObject nightWallsMR;
    public GameObject frWallsNight;
    [Space]
    public GameObject[] campBG;
    [Space]
    public GameObject[] campChests;//0-T0chest
    public GameObject[] bossChests;//0-Bossforest
    public GameObject[] enemyElites;//0-eRat, 1-eDog, 2-eWolf, 3-eBoar, 4-eGDagger, 5-eGAxe, 6-eRobberD, 7-eRobberM, 8-eCultist,9-eSkeleton,10-eZombie,11-eDemon,12-eWElem,13-eFElem
    [Space]

    public GameObject dropBagCasual;
    public GameObject dropBagElite;
    [Space]

    public GameObject[] positionsSpawn;
    public GameObject[] roomsInteractives;//0-trader, 1-altarskills, 2-health altar
    public GameObject[] obelisks;//0-forest,1-cave,2-town
    public GameObject[] brokenObelisks;//0-forest,1-cave,2-town
    public GameObject[] brokenRes;//0,1,2 - Wood; 3,4,5 - Stone; 6 - Leather
    public GameObject brokenAltar;
    public GameObject brokenSMQ;
    public GameObject brokenBook;
    public GameObject brokenFountain;
    public GameObject[] forestTiles;
    public GameObject[] caveTiles;
    public GameObject[] townTiles;
    public GameObject[] towerTiles;
    public GameObject[] eliteTiles;//0-Forest,1-Cave,2-Town,3-Tower
    public GameObject[] bossTiles;
    [Space]

    public GameObject[] bosses;//0-ForestBoss, 1-CaveBoss, 2-TownBoss, 3-Lich1Floor, 4-Lich2Floor, 5-Lich3Floor    
    public GameObject[] bossPosition;   
    
    GameObject a1,a2,a3,a4,a5,a6;
    //generate sector
    public GameObject[] aa = new GameObject[9];//room objects
    public int rr, pr, enemyIndex, lootIndex, resIndex, interIndex;
    public List<GameObject> timeIO = new List<GameObject>();//for interactive objects

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (ins != this)
        {
            Destroy(ins.gameObject);
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        timeDay = 0;
        ClearLightmaps();
        OffAllSimpleRooms();
        RenderSettings.fog = false; // off fog
        mainDirLight.SetActive(false);
    }    

    public void Destr()
    {
        Destroy(a1);
        Destroy(a2);
        Destroy(a3);
        Destroy(a4);
        Destroy(a5);
        Destroy(a6);
    }

    public void CreateSupplies()//for camp
    {//0-Forest, 1-Cave, 2-Town, 3-Tower
        Vector3 objUp = new Vector3(0, 4, 0);
        for (int i = 0; i < campBG.Length; i++)
        {
            campBG[i].SetActive(false);
        }

        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {
            //forestFloors[Random.Range(0, forestFloors.Length)].SetActive(true);
            campFloors[0].SetActive(true);
            campBG[Random.Range(0, campBG.Length)].SetActive(true);
            GenerateTileSupplies();
        }
        if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            //caveFloors[Random.Range(0, caveFloors.Length)].SetActive(true);
            campFloors[1].SetActive(true);
            campBG[Random.Range(0, campBG.Length)].SetActive(true);
            GenerateTileSupplies();
        }
        if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            //townFloors[Random.Range(0, townFloors.Length)].SetActive(true);
            campFloors[2].SetActive(true);
            campBG[Random.Range(0, campBG.Length)].SetActive(true);
            GenerateTileSupplies();
        }
        if (GumeManager.ins.mapIndex == 10)
        {
            //towerFloors[Random.Range(0, towerFloors.Length)].SetActive(true);
            campFloors[3].SetActive(true);
            campBG[Random.Range(0, campBG.Length)].SetActive(true);
            GenerateTileSupplies();
        }
        if (GumeManager.ins.mapIndex == 98)
        {
            //cellarFloors[Random.Range(0, towerFloors.Length)].SetActive(true);
            campFloors[3].SetActive(true);
            campBG[Random.Range(0, campBG.Length)].SetActive(true);
            GenerateTileSupplies();
        }

        a1 = Instantiate(campChests[0], positionsSpawn[7].transform.position+objUp, campChests[0].transform.rotation);               

        //create supplies
    }

    public void ButNewGear()
    {        
        GumeManager.ins.DestroyAllChests();
        GumeManager.ins.DestroyAllLiveEnemiesModels();
        InventoryManager.ins.ObnulAllInvs();
        CreateSupplies();
        UIManager.ins.CloseShortInfoPanel();
        GumeManager.ins.newGearBut.SetActive(false);
    }

    public void CreateSimpleRoom()//1E,2N,3H-forest; 4E,5N,6H-cave; 7E,8N,9H-town; 10H-tower; 98 - cellar;
    {        
        //other rooms 0-Sawmill, 1-WarCamp, 2-Swamp, 3-Graveyard, 4-AbondHouse1, 5-AbondHouse2, 6-CaveAltar, 7-CavePortal, 8-TownStorage, 9-TownLairOfBandits
        if (nameRoom == "BossRoom")
        {
            if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
            {
                otherFloors[1].SetActive(true);
            }
            else if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
            {
                otherFloors[7].SetActive(true);
            }
            else if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
            {
                otherFloors[9].SetActive(true);
            }
        }
        else if (nameRoom == "SawmillForest")//forest
        {
            otherFloors[0].SetActive(true);
        }
        else if (nameRoom == "GoblinsCampForest")
        {
            otherFloors[1].SetActive(true);
        }
        else if (nameRoom == "AmbushForest")
        {
            roomIndexRND = Random.Range(0, 3);
            forestFloors[roomIndexRND].SetActive(true); //***
        }
        else if (nameRoom == "AbondHouseForest")
        {
            roomIndexRND = Random.Range(4, 6);
            otherFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "SwampForest")
        {
            otherFloors[2].SetActive(true);
        }
        else if (nameRoom == "GraveyardForest")
        {
            otherFloors[3].SetActive(true);
        }
        else if (nameRoom == "StoneMineCave")//cave
        {
            roomIndexRND = Random.Range(0, 3);
            caveFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "GoldMineCave")
        {
            roomIndexRND = Random.Range(0, 3);
            caveFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "CrystalMineCave")
        {
            roomIndexRND = Random.Range(0, 3);
            caveFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "AmbushCave")
        {
            roomIndexRND = Random.Range(0, 3);
            caveFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "AltarCave")
        {
            otherFloors[6].SetActive(true);
        }
        else if (nameRoom == "PortalCave")
        {
            otherFloors[7].SetActive(true);
        }
        else if (nameRoom == "CapturedStorageTown")//town
        {
            otherFloors[8].SetActive(true);
        }
        else if (nameRoom == "LairBandits")
        {
            otherFloors[9].SetActive(true);
        }
        else if (nameRoom == "AmbushTown")
        {
            roomIndexRND = Random.Range(0, townFloors.Length);
            townFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "AbondHouseTown")
        {
            otherFloors[Random.Range(4, 6)].SetActive(true);
        }
        else if (nameRoom == "GraveyardTown")
        {
            otherFloors[3].SetActive(true);
        }
        else if (nameRoom == "FirstFloorRoomTower")//tower
        {
            roomIndexRND = Random.Range(0, 5);
            towerFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "SecondFloorRoomTower")
        {
            roomIndexRND = Random.Range(0, 5);
            towerFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "ThirdFloorRoomTower")
        {
            roomIndexRND = Random.Range(0, 5);
            towerFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "FirstFloorCellar")
        {
            roomIndexRND = Random.Range(0, 5);
            towerFloors[roomIndexRND].SetActive(true);//***
        }
        else if (nameRoom == "SecondFloorCellar")
        {
            roomIndexRND = Random.Range(0, 5);
            towerFloors[roomIndexRND].SetActive(true);//***
        }
    }

    public void CreateBossRoom()//1E,2N,3H-forest; 4E,5N,6H-cave; 7E,8N,9H-town; 10H-towerr; 98 - cellar;
    {
        //other rooms 0-Sawmill, 1-WarCamp, 2-Swamp, 3-Graveyard, 4-AbondHouse1, 5-AbondHouse2, 6-CaveAltar, 7-CavePortal, 8-TownStorage, 9-TownLairOfBandits
        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {
            otherFloors[1].SetActive(true);            
        }
        else if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            otherFloors[6].SetActive(true);
        }
        else if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            otherFloors[9].SetActive(true);
        }
        else if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
        {
            roomIndexRND = Random.Range(0, 5);
            towerFloors[roomIndexRND].SetActive(true);           
        }
        else if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
        {
            roomIndexRND = Random.Range(0, 5);
            towerFloors[roomIndexRND].SetActive(true);
        }

    }

    #region FOREST
    public void EnterSawmill()//FOREST
    {        
        CreateSimpleRoom();
        RForMan.ins.CreateSawmillObjs();//***
    }

    public void EnterGoblinsCamp()//FOREST
    {
        CreateSimpleRoom();
        RForMan.ins.CreateGoblinsCampObjs();//***
    }

    public void EnterAmbushForest()//FOREST
    {
        CreateSimpleRoom();
        RForMan.ins.CreateAmbushForestObjs();//***
    }

    public void EnterAbondHouse()//FOREST
    {
        CreateSimpleRoom();
        RForMan.ins.CreateAbondHouseObjs();//***
    }
    public void EnterGraveyard()//FOREST
    {
        CreateSimpleRoom();
        RForMan.ins.CreateGraveyardObjs();//***
    }

    public void EnterSwamp()//FOREST
    {
        CreateSimpleRoom();
        RForMan.ins.CreateSwampObjs();//***
    }
    #endregion

    #region CAVE
    public void EnterStoneMine()//Cave
    {
        CreateSimpleRoom();
        RCaveMan.ins.CreateStoneMineObjs();//***       
    }

    public void EnterGoldMine()//Cave
    {
        CreateSimpleRoom();
        RCaveMan.ins.CreateGoldMineObjs();//***       
    }

    public void EnterCrystalMineCave()//Cave
    {
        CreateSimpleRoom();
        RCaveMan.ins.CreateCrystalMineObjs();//***       
    }

    public void EnterAmbushCave()//Cave
    {
        CreateSimpleRoom();
        RCaveMan.ins.CreateAmbushCaveObjs();//***       
    }

    public void EnterAltarCave()//Cave
    {
        CreateSimpleRoom();
        RCaveMan.ins.CreateAltarCaveObjs();//***       
    }

    public void EnterPortalCave()//Cave
    {
        CreateSimpleRoom();
        RCaveMan.ins.CreatePortalCaveObjs();//***       
    }

    #endregion

    #region TOWN
    public void EnterCapturedStorageTown()//Town
    {
        CreateSimpleRoom();
        RTownMan.ins.CreateCapturedStorageTownObjs();  
    }

    public void EnterLairBanditsTown()//Town
    {
        CreateSimpleRoom();
        RTownMan.ins.CreateLairBanditsTownObjs();
    }

    public void EnterAbondHouseTown()//Town
    {
        CreateSimpleRoom();
        RTownMan.ins.CreateAbondHouseTownObjs();
    }

    public void EnterAmbushTown()//Town
    {
        CreateSimpleRoom();
        RTownMan.ins.CreateAmbushTownObjs();
    }

    public void EnterGraveyardTown()//Town
    {
        CreateSimpleRoom();
        RTownMan.ins.CreateGraveyardTownObjs();
    }

    #endregion

    #region TOWER
    public void EnterFirstFloorRoomTower()//Tower
    {
        CreateSimpleRoom();
        RTowerMan.ins.CreateFirstFloorRoomTowerObjs();
    }

    public void EnterSecondFloorRoomTower()//Tower
    {
        CreateSimpleRoom();
        RTowerMan.ins.CreateSecondFloorRoomTowerObjs();
    }

    public void EnterThirdFloorRoomTower()//Tower
    {
        CreateSimpleRoom();
        RTowerMan.ins.CreateThirdFloorRoomTowerObjs();
    }


    #endregion

    #region Cellar
    public void EnterFirstFloorRoomCellar()//Cellar
    {
        CreateSimpleRoom();
        RTowerMan.ins.CreateFirstFloorRoomCellarObjs();
    }

    public void EnterSecondFloorRoomCellar()//Cellar
    {
        CreateSimpleRoom();
        RTowerMan.ins.CreateSecondFloorRoomCellarObjs();
    }
    #endregion

    public void BossSpawn()
    {
        Vector3 standUp = new Vector3(0, 0, 0);
        Vector3 objUp = new Vector3(0, 5, 0);
        CreateBossRoom();

        //1E,2N,3H-forest; 4E,5N,6H-cave; 7E,8N,9H-town; 10E,11N,12H-tower;
        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {      
            Instantiate(bossTiles[Random.Range(0, bossTiles.Length)], bossPosition[2].transform.position + standUp, bossTiles[0].transform.rotation);            
            a5 = Instantiate(bosses[0], bossPosition[2].transform.position + objUp, transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            Instantiate(bossTiles[Random.Range(0, bossTiles.Length)], bossPosition[2].transform.position + standUp, bossTiles[0].transform.rotation);
            a5 = Instantiate(bosses[1], bossPosition[2].transform.position + objUp, transform.rotation);            
        }
        else if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            Instantiate(bossTiles[Random.Range(0, bossTiles.Length)], bossPosition[2].transform.position + standUp, bossTiles[0].transform.rotation);
            a5 = Instantiate(bosses[2], bossPosition[2].transform.position + objUp, transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 10)
        {
            Instantiate(bossTiles[Random.Range(0, bossTiles.Length)], bossPosition[2].transform.position + standUp, bossTiles[0].transform.rotation);
            a5 = Instantiate(bosses[3], bossPosition[2].transform.position + objUp, transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 11)
        {
            Instantiate(bossTiles[Random.Range(0, bossTiles.Length)], bossPosition[2].transform.position + standUp, bossTiles[0].transform.rotation);
            a5 = Instantiate(bosses[4], bossPosition[2].transform.position + objUp, transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 12)
        {
            Instantiate(bossTiles[Random.Range(0, bossTiles.Length)], bossPosition[2].transform.position + standUp, bossTiles[0].transform.rotation);
            a5 = Instantiate(bosses[5], bossPosition[2].transform.position + objUp, transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
        {
            Instantiate(bossTiles[Random.Range(0, bossTiles.Length)], bossPosition[2].transform.position + standUp, bossTiles[0].transform.rotation);
            a5 = Instantiate(bosses[Random.Range(0, 3)], bossPosition[2].transform.position + objUp, transform.rotation);           
        }
    }

    public void SpawnBossTreasure()
    {
        //CleanTiles
        GumeManager.ins.DestroyAllChests();
        Vector3 standUp = new Vector3(0, 0, 0);
        Vector3 objUp = new Vector3(0, 4.25f, 0);

        //1E,2N,3H-forest; 4E,5N,6H-cave; 7E,8N,9H-town; 10E,11N,12H-tower;
        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {
            if (GumeManager.ins.mapIndex == 2 && EnemyManager.ins.nameCreep == "BossForest" && QuestsManager.ins.killForestBossAct == 1)
            {
                Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[2].transform.position + standUp, eliteTiles[0].transform.rotation);
                Instantiate(StoryManager.ins.smallMonolythForest, bossPosition[2].transform.position + objUp, StoryManager.ins.smallMonolythForest.transform.rotation);//****               
            }
            else
            {
                Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[2].transform.position + standUp, eliteTiles[0].transform.rotation);
                a6 = Instantiate(bossChests[0], bossPosition[2].transform.position + objUp, bossChests[0].transform.rotation);
            }
        }
        else if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[2].transform.position + standUp, eliteTiles[0].transform.rotation);
            a6 = Instantiate(bossChests[0], bossPosition[2].transform.position + objUp, bossChests[0].transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[2].transform.position + standUp, eliteTiles[0].transform.rotation);
            a6 = Instantiate(bossChests[0], bossPosition[2].transform.position + objUp, bossChests[0].transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)//******
        {
            Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[1].transform.position + standUp, eliteTiles[0].transform.rotation);
            Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[2].transform.position + standUp, eliteTiles[0].transform.rotation);
            Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[3].transform.position + standUp, eliteTiles[0].transform.rotation);
            a6 = Instantiate(bossChests[1], bossPosition[2].transform.position + objUp, bossChests[0].transform.rotation);
            a2 = Instantiate(roomsInteractives[1], bossPosition[1].transform.position + objUp, bossChests[0].transform.rotation);
            a3 = Instantiate(roomsInteractives[2], bossPosition[3].transform.position + objUp, bossChests[0].transform.rotation);
        }
        else if (GumeManager.ins.mapIndex == 12)
        {
            Instantiate(eliteTiles[Random.Range(0, eliteTiles.Length)], bossPosition[2].transform.position + standUp, eliteTiles[0].transform.rotation);
            Instantiate(StoryManager.ins.finalBook, bossPosition[2].transform.position + objUp, StoryManager.ins.finalBook.transform.rotation);//****
        }
    } 

    public void OffAllSimpleRooms()
    {
        //forestrooms
        for (int i = 0; i < forestFloors.Length; i++)
        {
            forestFloors[i].SetActive(false);
        }
        //caverooms
        for (int i = 0; i < caveFloors.Length; i++)
        {
            caveFloors[i].SetActive(false);
        }
        //townrooms
        for (int i = 0; i < townFloors.Length; i++)
        {
            townFloors[i].SetActive(false);
        }
        //towerrooms
        for (int i = 0; i < towerFloors.Length; i++)
        {
            towerFloors[i].SetActive(false);
        }
        //otherrooms
        for (int i = 0; i <  otherFloors.Length; i++)
        {
            otherFloors[i].SetActive(false);
        }
        //camprooms
        for (int i = 0; i < campFloors.Length; i++)
        {
            campFloors[i].SetActive(false);
        }
        //walls
        nightWallsMR.SetActive(false);
        frWallsNight.SetActive(false);

        for (int i = 0; i < campBG.Length; i++)
        {            
            campBG[i].SetActive(false);
        }      

    }


    //Weather
    public void OnWalls()
    {
        if (timeDay != 3)
        {
            nightWallsMR.SetActive(false);
            frWallsNight.SetActive(false);
        }
        else
        {
            nightWallsMR.SetActive(true);
            frWallsNight.SetActive(true);
        }    
    }

    public void CheckTimeDay()
    {
        timeDay = Random.Range(0, 4);// 0 - Morning, 1 - Day, 2 - Evening, 3 - Night;
        //timeDay = 3;       
    }

    public void SelectTimeDay()//Times of Day; 0-Morning, 1-Day, 2-Evening, 3-Night; 
    {
        OffAllLightsMainRooms();
        mainDirLight.SetActive(true);

        if (inoutDoor == "Indoor")
        {
            //0-AboundonedHouse, 1-Cave, 2-Fiol, 3-EveningLight
            if (nameRoom == "AbondHouseForest" || nameRoom == "CapturedStorageTown" || nameRoom == "LairBandits" || nameRoom == "AbondHouseTown" || nameRoom == "FirstFloorRoomTower")
            {
                lightsOtherRooms[0].SetActive(true);
                lightsFOtherRooms[0].SetActive(true);
                mainDirLight.SetActive(false);
            }
            else if (nameRoom == "StoneMineCave" || nameRoom == "GoldMineCave" || nameRoom == "CrystalMineCave" || nameRoom == "SecondFloorRoomTower" || nameRoom == "FirstFloorCellar" || nameRoom == "AmbushCave")
            {
                lightsOtherRooms[1].SetActive(true);
                lightsFOtherRooms[1].SetActive(true);
                mainDirLight.SetActive(false);
            }
            else if (nameRoom == "AltarCave" || nameRoom == "PortalCave" || nameRoom == "ThirdFloorRoomTower" || nameRoom == "SecondFloorCellar" || nameRoom == "BossRoom")
            {
                lightsOtherRooms[1].SetActive(true);
                lightsFOtherRooms[1].SetActive(true);
                lightsOtherRooms[2].SetActive(true);
                lightsFOtherRooms[2].SetActive(true);
                mainDirLight.SetActive(false);
            }
            else if (nameRoom == "CampBefore")
            {
                if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
                {
                    
                }
                else
                {
                    lightsOtherRooms[0].SetActive(true);
                }
            }
            
            ChangeColorMainLight();
            Vector3 biba = new Vector3(200, 0, 0);
            mainDirLight.transform.rotation = Quaternion.Euler(biba);
            mainDirLight.SetActive(false);            
            Debug.Log("Other___200_0_0");
        }
        else if (inoutDoor == "Outdoor")
        {
            if (timeDay == 0)//Morning
            {
                lightsMainRooms[0].SetActive(true);
                lightsFightRooms[0].SetActive(true);

                mainDirLight.SetActive(true);
                Vector3 biba = new Vector3(10, 0, 0);
                mainDirLight.transform.rotation = Quaternion.Euler(biba);                

                ChangeColorMainLight();
                Debug.Log("Morning___10_0_0");
            }
            else if (timeDay == 1)//Day
            {
                lightsMainRooms[1].SetActive(true);
                lightsFightRooms[1].SetActive(true);

                mainDirLight.SetActive(true);
                Vector3 biba = new Vector3(60, 0, 0);
                mainDirLight.transform.rotation = Quaternion.Euler(biba);

                ChangeColorMainLight();
                Debug.Log("Day___60_0_0");
            }
            else if (timeDay == 2)//Evevning
            {
                lightsMainRooms[2].SetActive(true);
                lightsFightRooms[2].SetActive(true);
                lightsOtherRooms[3].SetActive(true);
                lightsFOtherRooms[3].SetActive(true);
                mainDirLight.SetActive(true);

                Vector3 biba = new Vector3(160, 0, 0);
                mainDirLight.transform.rotation = Quaternion.Euler(biba);                

                ChangeColorMainLight();
                Debug.Log("Evening___155_0_0");               
            }
            else if (timeDay == 3)//Night
            {
                lightsMainRooms[3].SetActive(true);
                lightsFightRooms[3].SetActive(true);

                Vector3 biba = new Vector3(200, 0, 0);
                mainDirLight.transform.rotation = Quaternion.Euler(biba);
                mainDirLight.SetActive(false);

                ChangeColorMainLight();
                Debug.Log("Night___200_0_0");
            }
        }   

        ChangeFog();
    }    

    public void ChangeColorMainLight()
    {
        Light directionalLight = FindDirectionalLight();

        if (inoutDoor == "Indoor")
        {
            //directionalLight.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        }
        else if (inoutDoor == "Outdoor")
        {
            if (directionalLight != null)
            {
                if (timeDay == 0)//Morning
                {
                    directionalLight.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                }
                else if (timeDay == 1)//Day
                {
                    directionalLight.color = new Color(255f / 255f, 235f / 255f, 185f / 255f);
                }
                else if (timeDay == 2)//Evevning
                {
                    directionalLight.color = new Color(255f / 255f, 160f / 255f, 50f / 255f);
                }
                else if (timeDay == 3)//Night
                {
                    directionalLight.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                }
            }
            else
            {
                Debug.LogError("Directional Light не найден в сцене!");
            }
        }
    }

    public void ChangeFog()
    {
        RenderSettings.fog = true; // on fog

        if (inoutDoor == "Indoor")
        {
            if (nameRoom == "AbondHouseForest" || nameRoom == "CapturedStorageTown" || nameRoom == "LairBandits" || nameRoom == "AbondHouseTown")
            {
                RenderSettings.fogColor = new Color(255 / 255f, 200 / 255f, 125 / 255f);
                RenderSettings.fogDensity = 0.001f; // fog density 
            }
            else if (nameRoom == "StoneMineCave" || nameRoom == "GoldMineCave" || nameRoom == "CrystalMineCave")
            {
                RenderSettings.fogColor = new Color(125 / 255f, 255 / 255f, 200 / 255f);
                RenderSettings.fogDensity = 0.001f; // fog density 
            }
            else if (nameRoom == "AltarCave" || nameRoom == "PortalCave")
            {
                RenderSettings.fogColor = new Color(135 / 255f, 70 / 255f, 125 / 255f);
                RenderSettings.fogDensity = 0.003f; // fog density 
            }

        }
        else if (inoutDoor == "Outdoor")
        {
            if (timeDay == 0)// morning
            {
                RenderSettings.fogColor = new Color(255 / 255f, 210 / 255f, 120 / 255f);
                RenderSettings.fogDensity = 0.002f; // fog density 
            }
            else if (timeDay == 1)// day
            {
                RenderSettings.fogColor = new Color(135 / 255f, 220 / 255f, 255 / 255f);
                RenderSettings.fogDensity = 0.001f; // fog density 
            }
            else if (timeDay == 2)// evening
            {
                RenderSettings.fogColor = new Color(120 / 255f, 75 / 255f, 30 / 255f);
                RenderSettings.fogDensity = 0.001f; // fog density 
            }
            else if (timeDay == 3)// night
            {
                RenderSettings.fogColor = new Color(60 / 255f, 60 / 255f, 125 / 255f);
                RenderSettings.fogDensity = 0.002f; // fog density 
            }
        }
    }

    public Light FindDirectionalLight()
    {
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
                return light;
        }
        return null;
    }

    public void OffAllLightsMainRooms()
    {
        for (int i = 0; i < lightsMainRooms.Length; i++)
        {
            lightsMainRooms[i].SetActive(false);                     
        }

        for (int i = 0; i < lightsFightRooms.Length; i++)
        {            
            lightsFightRooms[i].SetActive(false);
        }

        for (int i = 0; i < lightsOtherRooms.Length; i++)
        {
            lightsOtherRooms[i].SetActive(false);           
        }

        for (int i = 0; i < lightsFOtherRooms.Length; i++)
        {
            lightsFOtherRooms[i].SetActive(false);
        }

    }

    //Generate Sector
    public void GenerateRoomEnemy(int rnd, int i, int iE, GameObject[] enemies, GameObject[] enemiesE)
    {
        //Enemies elite rooms manager: 0-ERat, 1-EBoar, 2-EGoblinAxe, 3-ERobberKnife, 4-Ecultist, 5-EDemon
        rr = Random.Range(1, 101);
        Vector3 standUp = new Vector3(0, 0, 0);
        Vector3 objUp = new Vector3(0, 4.25f, 0);   

        if (rr < rnd && GumeManager.ins.mapIndex != 0 && GumeManager.ins.mapIndex != 3 && GumeManager.ins.mapIndex != 6)
        {
            if (EnemyManager.ins.eliteNumber == 1)//chtobi ne spawnilo 2 elitki
            {
                //1E,2N,3H-forest; 4E,5N,6H-cave; 7E,8N,9H-town; 10,11,12-tower; 98,99-cellar;
                if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
                {
                    Instantiate(eliteTiles[0], positionsSpawn[i].transform.position + standUp, eliteTiles[0].transform.rotation);
                }
                if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
                {
                    Instantiate(eliteTiles[1], positionsSpawn[i].transform.position + standUp, eliteTiles[0].transform.rotation);
                }
                if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
                {
                    Instantiate(eliteTiles[2], positionsSpawn[i].transform.position + standUp, eliteTiles[0].transform.rotation);
                }
                if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
                {
                    Instantiate(eliteTiles[3], positionsSpawn[i].transform.position + standUp, eliteTiles[0].transform.rotation);
                }
                if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
                {
                    Instantiate(eliteTiles[3], positionsSpawn[i].transform.position + standUp, eliteTiles[0].transform.rotation);
                }
                aa[i] = Instantiate(enemiesE[iE], positionsSpawn[i].transform.position+objUp, enemiesE[iE].transform.rotation);
                EnemyManager.ins.eliteNumber = 0;
            }
            else
            {                
                if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
                {                    
                    Instantiate(forestTiles[Random.Range(0, forestTiles.Length)], positionsSpawn[i].transform.position + standUp, forestTiles[0].transform.rotation);                    
                }
                if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
                {                    
                    Instantiate(caveTiles[Random.Range(0, caveTiles.Length)], positionsSpawn[i].transform.position + standUp, caveTiles[0].transform.rotation);
                }
                if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
                {                    
                    Instantiate(townTiles[Random.Range(0, townTiles.Length)], positionsSpawn[i].transform.position + standUp, townTiles[0].transform.rotation);
                }
                if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
                {                   
                    Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
                }
                if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
                {
                    Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
                }               
                enemyIndex = Random.Range(0, enemies.Length);
                aa[i] = Instantiate(enemies[enemyIndex], positionsSpawn[i].transform.position+objUp, enemies[enemyIndex].transform.rotation);
            }
        }
        else
        {
            //Instantiate(stands[Random.Range(0, 3)], positionsSpawn[i].transform.position + standUp, ins.stands[0].transform.rotation);
            //Instantiate(stands[Random.Range(0, 3)], positionsSpawn[i].transform.position+standUp, stands[0].transform.rotation);
            if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
            {
                Instantiate(forestTiles[Random.Range(0, forestTiles.Length)], positionsSpawn[i].transform.position + standUp, forestTiles[0].transform.rotation);
            }
            if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
            {
                Instantiate(caveTiles[Random.Range(0, caveTiles.Length)], positionsSpawn[i].transform.position + standUp, caveTiles[0].transform.rotation);
            }
            if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
            {
                Instantiate(townTiles[Random.Range(0, townTiles.Length)], positionsSpawn[i].transform.position + standUp, townTiles[0].transform.rotation);
            }
            if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
            {
                Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
            }
            if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
            {
                Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
            }

            enemyIndex = Random.Range(0, enemies.Length);
            aa[i] = Instantiate(enemies[enemyIndex], positionsSpawn[i].transform.position+objUp, enemies[enemyIndex].transform.rotation);
        }
    }

    public void GenerateRoomChest(int i, GameObject[] chests)
    {
        Vector3 standUp = new Vector3(0, 0, 0);
        Vector3 objUp = new Vector3(0, 4.5f, 0);
        
        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {    
            Instantiate(forestTiles[Random.Range(0, forestTiles.Length)], positionsSpawn[i].transform.position + standUp, forestTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            Instantiate(caveTiles[Random.Range(0, caveTiles.Length)], positionsSpawn[i].transform.position + standUp, caveTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            Instantiate(townTiles[Random.Range(0, townTiles.Length)], positionsSpawn[i].transform.position + standUp, townTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
        }

        lootIndex = Random.Range(0, chests.Length);
        aa[i] = Instantiate(chests[lootIndex], positionsSpawn[i].transform.position+objUp, chests[lootIndex].transform.rotation);
    }

    public void GenerateRoomRes(int i, GameObject[] res)
    {
        Vector3 standUp = new Vector3(0, 0, 0);
        Vector3 objUp = new Vector3(0, 4.5f, 0);

        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {
            Instantiate(forestTiles[Random.Range(0, forestTiles.Length)], positionsSpawn[i].transform.position + standUp, forestTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            Instantiate(caveTiles[Random.Range(0, caveTiles.Length)], positionsSpawn[i].transform.position + standUp, caveTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            Instantiate(townTiles[Random.Range(0, townTiles.Length)], positionsSpawn[i].transform.position + standUp, townTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
        }

        resIndex = Random.Range(0, res.Length);
        aa[i] = Instantiate(res[resIndex], positionsSpawn[i].transform.position+objUp, res[resIndex].transform.rotation);
    }

    public void GenerateRoomInteractive(int i)
    {
        Vector3 standUp = new Vector3(0, 0, 0);
        Vector3 objUp = new Vector3(0, 4.5f, 0);

        //Interactives: 0-Bonfire, 1-AlchmTable, 2-Trader, 3-Altar
        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {        
            Instantiate(forestTiles[Random.Range(0, forestTiles.Length)], positionsSpawn[i].transform.position + standUp, forestTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            Instantiate(caveTiles[Random.Range(0, caveTiles.Length)], positionsSpawn[i].transform.position + standUp, caveTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            Instantiate(townTiles[Random.Range(0, townTiles.Length)], positionsSpawn[i].transform.position + standUp, townTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[i].transform.position + standUp, towerTiles[0].transform.rotation);
        }

        interIndex = Random.Range(0, timeIO.Count);
        aa[i] = Instantiate(timeIO[interIndex], positionsSpawn[i].transform.position+objUp, timeIO[interIndex].transform.rotation);
        timeIO.Remove(timeIO[interIndex]);
    }

    public void GeneratePresset(int a1, int a2)//for enemies
    {
        pr = Random.Range(a1, a2);
    }

    public void GenerateTileSupplies()
    {
        Vector3 standUp = new Vector3(0, -0.5f, 0);

        if (GumeManager.ins.mapIndex == 1 || GumeManager.ins.mapIndex == 2 || GumeManager.ins.mapIndex == 3)
        {   
            Instantiate(forestTiles[Random.Range(0, forestTiles.Length)], positionsSpawn[7].transform.position + standUp, forestTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 4 || GumeManager.ins.mapIndex == 5 || GumeManager.ins.mapIndex == 6)
        {
            Instantiate(caveTiles[Random.Range(0, caveTiles.Length)], positionsSpawn[7].transform.position + standUp, caveTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 7 || GumeManager.ins.mapIndex == 8 || GumeManager.ins.mapIndex == 9)
        {
            Instantiate(townTiles[Random.Range(0, townTiles.Length)], positionsSpawn[7].transform.position + standUp, townTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 10 || GumeManager.ins.mapIndex == 11 || GumeManager.ins.mapIndex == 12)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[7].transform.position + standUp, towerTiles[0].transform.rotation);
        }
        if (GumeManager.ins.mapIndex == 98 || GumeManager.ins.mapIndex == 99)
        {
            Instantiate(towerTiles[Random.Range(0, towerTiles.Length)], positionsSpawn[7].transform.position + standUp, towerTiles[0].transform.rotation);
        }

    }    

    public void Destr0()//destroy room objects
    {
        for (int i = 0; i < aa.Length; i++)
        {
            Destroy(aa[i]);
        }
    }

    // Вызывайте этот метод, когда нужно очистить lightmap
    public void ClearLightmaps()
    {
        // Очищаем массив lightmaps
        LightmapSettings.lightmaps = new LightmapData[0];

        // Очищаем данные ambient probe и light probes
        LightmapSettings.lightProbes = null;

        // Сбрасываем глобальные настройки освещения (если нужно)
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.reflectionIntensity = 0f;

        Debug.Log("Lightmaps and light probes cleared.");
    }
}
