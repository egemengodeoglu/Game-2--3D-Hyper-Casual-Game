using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class MenuControlEndless : MonoBehaviour
{
    public ThemaReferences beachThema;
    public ThemaReferences streetThema;
    public CarsReferences cars;
    public CommonUseThemaReferences commonUseThema;
    private PoolObject player;
    public CinemachineVirtualCamera vcam;


    public float countDownTime;
    Random rand = new Random();

    private float[] randomRoad = { -10.3f, -3.3f, 3.8f };
    private float startTime, z_leftside = -120, z_rightside = -120;
    private bool control = true, isChoosed = false;
    public static bool gameIsPaused = true;
    private int coin=0;
    private string choosedThema;

    private PoolPlayer playerController;
    private ThemaChooserScript themaChooser;
    public Action<string, string, string> ChangePanel;

    public void Start()
    {
        player = Instantiate(cars.cars[GameDataReferences.Instance.carIndex], new Vector3(-3f, 0.55f, -90f), Quaternion.identity);
        player.GetComponent<PoolPlayer>().enabled = true;
        player.GetComponent<PoolCars>().enabled = false;
        player.GetComponent<ObjectType>().enabled = false;
        vcam.Follow = player.transform;

        themaChooser = GameObject.FindObjectOfType<ThemaChooserScript>();
        themaChooser.OnThemaEvent += OnThemaEvent;
        playerController = GameObject.FindObjectOfType<PoolPlayer>();
        playerController.OnPlayerEvent += OnPlayerEvent;
        PauseSystem(true);
    }

    public void PauseSystem(bool pause)
    {
        gameIsPaused = pause;
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void OnThemaEvent(string tmp)
    {
        Choosed(tmp);
        ChangePanel.Invoke("Thema", tmp, "");
    }

    private void OnPlayerEvent(string obj,int data)
    {
        switch (obj)
        {
            case "Die":
                PauseSystem(true);
                ChangePanel.Invoke(obj, "", coin.ToString());
                break;

            case "Finish":
                PauseSystem(true);
                ChangePanel.Invoke(obj, "", coin.ToString());
                break;

            case "Coin":
                coin++;
                ChangePanel.Invoke(obj, "", coin.ToString());
                break;

            case "Recycle":
                PoolManager.Instance.NotUsedObjects(data);
                Repeater(data);
                break;
            default:
                break;
        }
    }

    public void Choosed(string thema)
    {
        isChoosed = true;
        startTime = Time.realtimeSinceStartup;
        choosedThema = thema;
        Starter();
        Repeater(-1);
    }

    void Update()
    {
        if (isChoosed)
        {
            if ((Time.realtimeSinceStartup - startTime) <= countDownTime)
            {
                ChangePanel.Invoke("CountDown", (countDownTime - (Time.realtimeSinceStartup - startTime)).ToString("0").ToString(), "");
            }
            else if ((Time.realtimeSinceStartup - startTime) <= countDownTime + 1)
            {
                ChangePanel.Invoke("CountDown", "GO!", "");
            }
            else if (control)
            {
                PauseSystem(false);
                ChangePanel.Invoke("StartGame", "", "");
                control = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }
    


    public void Starter()
    {
        int rnd;
        if(choosedThema == "beach")
        {
            for (int i = 0; i < 6; i++)
            {
                PoolManager.Instance.UseObject(beachThema.boats[i % 3], new Vector3(18f, -3f, -100 + (i * 14)), Quaternion.Euler(new Vector3(0f, -90f, 0f))); // boat
                PoolManager.Instance.UseObject(beachThema.road[2], new Vector3(30, 0f, -20 * i), Quaternion.identity); //sea
                PoolManager.Instance.UseObject(beachThema.road[2], new Vector3(-17, 0f, -20 * i), Quaternion.identity); //sea
                PoolManager.Instance.UseObject(beachThema.road[2], new Vector3(-37, 0f, -20 * i), Quaternion.identity); //sea
                PoolManager.Instance.UseObject(beachThema.boats[rand.Next(0, 3)], new Vector3(-21f, -3f, -100 + (i * 14)), Quaternion.identity); // boat
                PoolManager.Instance.UseObject(beachThema.road[3], new Vector3(10f, 0f, -120 + 20f * i), Quaternion.Euler(new Vector3(0f, -90f, 0f)));//wall
                PoolManager.Instance.UseObject(beachThema.road[3], new Vector3(-17f, 0f, -100 + 20f * i), Quaternion.Euler(new Vector3(0f, 90f, 0f)));//wall
            }
            PoolManager.Instance.UseObject(beachThema.environment[6], new Vector3(15, -3, -115), Quaternion.Euler(new Vector3(0f, -90f, 0f))); //Rock
            PoolManager.Instance.UseObject(beachThema.environment[6], new Vector3(-22, -3, -115), Quaternion.Euler(new Vector3(0f, 180f, 0f))); //Rock
            PoolManager.Instance.UseObject(beachThema.road[1], new Vector3(-17, 0, 0), Quaternion.identity); //beach2
        }
        else if (choosedThema == "street")
        {
            while (z_leftside < 0) //buildings leftside
            {
                rnd = rand.Next(streetThema.buildings.Count);
                PoolManager.Instance.UseObject(streetThema.buildings[rnd], new Vector3( -17 - streetThema.buildings[rnd].GetComponent<PoolBuildings>().distanceToFrontSide, 0, z_leftside + ((streetThema.buildings[rnd].GetComponent<PoolBuildings>().length) /2) ), 
                    Quaternion.Euler(new Vector3(0, -90, 0)));
                z_leftside += streetThema.buildings[rnd].GetComponent<PoolBuildings>().length;
            }

            while (z_rightside < 0) //buildings rightside
            {
                rnd = rand.Next(streetThema.buildings.Count);
                PoolManager.Instance.UseObject(streetThema.buildings[rnd], new Vector3( 10 + streetThema.buildings[rnd].GetComponent<PoolBuildings>().distanceToFrontSide, 0, z_rightside + ((streetThema.buildings[rnd].GetComponent<PoolBuildings>().length) / 2)),
                    Quaternion.Euler(new Vector3(0, 90, 0)));
                z_rightside += streetThema.buildings[rnd].GetComponent<PoolBuildings>().length;
            }
        }
        
        for (int i = 0; i < 6; i++)
        {
            PoolManager.Instance.UseObject(commonUseThema.road[0], new Vector3(0f, 0f, -120 + 20f * i), Quaternion.identity);//Road
        }

        for (int i = 0; i < 3; i++)
        {
            PoolManager.Instance.UseObject(commonUseThema.environment[0], new Vector3(-15, 0, -118 + 40 * i), Quaternion.Euler(new Vector3(0f, -90f, 0f)));//Left street lamb
            PoolManager.Instance.UseObject(commonUseThema.environment[1], new Vector3(8f, 0f, -100 + 40 * i), Quaternion.Euler(new Vector3(0f, 90f, 0f)));//right street lamb
        }
        PoolManager.Instance.UseObject(commonUseThema.environment[3], new Vector3(8, 0, rand.Next(-118, -2)), Quaternion.identity);//hydrant
        PoolManager.Instance.UseObject(commonUseThema.environment[2], new Vector3(-15, 0, -80 + rand.Next(-118, -2)), Quaternion.identity);//trash
        PoolManager.Instance.UseObject(commonUseThema.repeater, new Vector3(-8f, 0f, 0f), Quaternion.identity);//repeater
    }

    public void Repeater(int playerZ)                                                                                                                       //Endless Runner !!!!! 
    {
        PoolObject car;
        int tmp, rnd;
        float tmpRand;
        if (choosedThema == "beach")
        {
            for (int i = 0; i < 6; i++) //beach, see, wall, tree, beachseat
            {
                PoolManager.Instance.UseObject(beachThema.road[0], new Vector3(-17f, 0f, (playerZ * 120) + 120 + 20f * i), Quaternion.identity);//beach
                PoolManager.Instance.UseObject(beachThema.road[2], new Vector3(-37f, 0f, (playerZ * 120) + 120 + 20f * (i + 1)), Quaternion.identity);//sea
                PoolManager.Instance.UseObject(beachThema.road[3], new Vector3(10f, 0f, (playerZ * 120) + 120 + 20f * i), Quaternion.Euler(new Vector3(0f, -90f, 0f)));//wall
                PoolManager.Instance.UseObject(beachThema.road[2], new Vector3(30f, 0f, (playerZ * 120) + 120 + 20f * (i + 1)), Quaternion.identity);//sea
                PoolManager.Instance.UseObject(beachThema.environment[rand.Next(0, 3)], new Vector3(-20f, 0f, (playerZ * 120) + 120 + 20f * i + rand.Next(4, 16)), Quaternion.identity);// tree
                PoolManager.Instance.UseObject(beachThema.environment[rand.Next(3, 6)], new Vector3(-26.5f, 0, (playerZ * 120) + 120 + 20f * i + rand.Next(4, 16)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));//Beachseat 
            } //beach, see, wall, tree, beachseat

            for (int i = 0; i < 3; i++) //boat
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        PoolManager.Instance.UseObject(beachThema.boats[rand.Next(0, 3)], new Vector3(18f, -4f, (playerZ * 120) + 120 + rand.Next(5, 35) + i * 33), Quaternion.Euler(new Vector3(0f, -90f, 0f)));//boat
                        break;
                    case 1:
                        PoolManager.Instance.UseObject(beachThema.boats[rand.Next(0, 3)], new Vector3(14f, -4f, (playerZ * 120) + 120 + rand.Next(9, 31) + i * 33), Quaternion.Euler(new Vector3(0f, 180f, 0f)));//boat
                        break;
                    case 2:
                        PoolManager.Instance.UseObject(beachThema.boats[rand.Next(0, 3)], new Vector3(14f, -4f, (playerZ * 120) + 120 + rand.Next(9, 31) + i * 33), Quaternion.identity);//boat
                        break;
                }
            }//boat
        } 
        else if (choosedThema == "street")
        {
            while (z_leftside < (playerZ * 120)+240) //buildings leftside
            {
                rnd = rand.Next(streetThema.buildings.Count);
                PoolManager.Instance.UseObject(streetThema.buildings[rnd], new Vector3(-17 - streetThema.buildings[rnd].GetComponent<PoolBuildings>().distanceToFrontSide, 0, z_leftside + ((streetThema.buildings[rnd].GetComponent<PoolBuildings>().length) / 2)),
                    Quaternion.Euler(new Vector3(0, -90, 0)));
                z_leftside += streetThema.buildings[rnd].GetComponent<PoolBuildings>().length;
            } //buildings leftside

            while (z_rightside < (playerZ * 120) + 240) //buildings rightside
            {
                rnd = rand.Next(streetThema.buildings.Count);
                PoolManager.Instance.UseObject(streetThema.buildings[rnd], new Vector3(10 + streetThema.buildings[rnd].GetComponent<PoolBuildings>().distanceToFrontSide, 0, z_rightside + ((streetThema.buildings[rnd].GetComponent<PoolBuildings>().length) / 2)),
                    Quaternion.Euler(new Vector3(0, 90, 0)));
                z_rightside += streetThema.buildings[rnd].GetComponent<PoolBuildings>().length;
            } //buildings rightside
        }

        //common usage
        
        if (playerZ % 2 == 1) //lambs
        {
            for (int i = 0; i < 3; i++)
            {
                PoolManager.Instance.UseObject(commonUseThema.environment[0], new Vector3(-15f, 0f, (playerZ * 120) + 120 + 40 * i), Quaternion.Euler(new Vector3(0f, -90f, 0f)));//Left street lamb
                PoolManager.Instance.UseObject(commonUseThema.environment[1], new Vector3(8f, 0f, (playerZ * 120) + 140 + 40 * i), Quaternion.Euler(new Vector3(0f, 90f, 0f)));//right street lamb
            }
        } //lambs
        else 
        {
            for (int i = 0; i < 3; i++)
            {
                PoolManager.Instance.UseObject(commonUseThema.environment[0], new Vector3(-15f, 0f, (playerZ * 120) + 140 + 40 * i), Quaternion.Euler(new Vector3(0f, -90f, 0f)));//Left street lamb
                PoolManager.Instance.UseObject(commonUseThema.environment[1], new Vector3(8f, 0f, (playerZ * 120) + 120 + 40 * i), Quaternion.Euler(new Vector3(0f, 90f, 0f)));//right street lamb
            }
        } //lambs
        

        tmp = rand.Next(10, 90);
        tmpRand = randomRoad[rand.Next(0, 3)];
        for (int i = 0; i < 6; i++) //road, coin
        {
            PoolManager.Instance.UseObject(commonUseThema.road[0], new Vector3(0f, 0f, (playerZ * 120) + 120 + 20f * i), Quaternion.identity);//Road
            PoolManager.Instance.UseObject(commonUseThema.coinObject, new Vector3(tmpRand, 0.75f, (playerZ * 120) + 120 + tmp + i * 2), Quaternion.Euler(new Vector3(90f, 0f, 0f)));//coin
        } //road, coin
        
        for (int i = 0; i < 4; i++) //car
        {
            car = PoolManager.Instance.UseObject(cars.cars[rand.Next(18)], new Vector3(randomRoad[rand.Next(0, 3)], 0f, (playerZ * 120) + 130 + 30f * i),
                Quaternion.Euler(new Vector3(0f, 180f, 0f)));//Car
            car.GetComponent<PoolPlayer>().enabled = false;
            car.GetComponent<ObjectType>().enabled = true;
            car.GetComponent<PoolCars>().enabled = true;
            car.GetComponent<PoolCars>().speed = 6;
        } //car
        
        for(int i = 0; i < 2; i++)
        {
            PoolManager.Instance.UseObject(commonUseThema.environment[3], new Vector3(8f, 0f, (playerZ * 120) + 120 + rand.Next(60) + i*60), Quaternion.identity);//hydrant
            PoolManager.Instance.UseObject(commonUseThema.environment[2], new Vector3(-15f, 0f, (playerZ * 120) + 120 + rand.Next(60) + i * 60), Quaternion.identity);//trash
        }
        PoolManager.Instance.UseObject(commonUseThema.repeater, new Vector3(-8f, 0f, (playerZ * 120) + 240f), Quaternion.identity);//repeater
    }

    public void Resume()
    {
        ChangePanel.Invoke("Resume","","");
        PauseSystem(false);
    }
    public void Pause()
    {
        ChangePanel.Invoke("Pause","","");
        PauseSystem(true);
    }

    public void Retry()
    {
        ChangePanel.Invoke("Retry", "", "");
        PauseSystem(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void StartGame()
    {
        ChangePanel.Invoke("StartGame","","");
        PauseSystem(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        PauseSystem(true);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        PauseSystem(true);
        Debug.Log("Quiting Game...");
    }
}
