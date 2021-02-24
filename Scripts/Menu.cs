using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menu, settings, carSelect;
    public Button leftButton, rightButton;
    public float countDown;
    private int current;
    public CarsReferences cars;
    private List<PoolObject> createds;
    public GameObject carPanel;
    public Sequence sequence;
    public float startTime;
    private bool isClickable = true;

    public void Start()
    {
        createds = new List<PoolObject>();

        for (int i = 0; i < cars.cars.Count; i++)
        {
            createds.Add(Instantiate(cars.cars[i], new Vector3(0f, 0f, 0f), Quaternion.Euler(0f,220f,0f)));
            createds[i].transform.Find("Smoke").gameObject.SetActive(false);
            createds[i].GetComponent<PoolPlayer>().enabled = false;
            createds[i].gameObject.GetComponent<PoolCars>().enabled = true;
            createds[i].GetComponent<ObjectType>().enabled = false;
            createds[i].gameObject.GetComponent<PoolCars>().speed = 0;
            createds[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
            
        }

        AddingFeatureToPanel();
        sequence = DOTween.Sequence();
        menu.SetActive(true);
        settings.SetActive(false);
        carSelect.SetActive(false);
        isClickable = true;
        leftButton.onClick.AddListener(LeftButton);
        rightButton.onClick.AddListener(RightButton);
    }

    private void Update()
    {
        if (!isClickable)
        {
            if ((Time.realtimeSinceStartup - startTime) > 1.5f)
            {
                leftButton.enabled = true;
                rightButton.enabled = true;
                isClickable = true;
            }
        }
    }

    public void SelectButton()
    {
        GameDataReferences.Instance.carIndex = current;
        GameDataReferences.Instance.player = cars.cars[current];
        SaveBinary.SaveGameData(GameDataReferences.Instance);
        DestroySequence();
        menu.SetActive(true);
        carSelect.SetActive(false);
    }

    public void RightButton()
    {
        current--;
        if (current == -1)
        {
            current = cars.cars.Count - 1;
            createds[current].transform.position = new Vector3(-30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
            sequence.Append(createds[0].transform.DOMoveX(30f, 1.5f));
            
        }
        else
        {
            sequence.Append(createds[current + 1].transform.DOMoveX(30f, 1.5f));
            createds[current].transform.position = new Vector3(-30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
        }
        startTime = Time.realtimeSinceStartup;
        leftButton.enabled = false;
        rightButton.enabled = false;
        isClickable = false;
        AddingFeatureToPanel();        
    }

    public void LeftButton()
    {
        current++;
        if (current == cars.cars.Count)
        {
            current = 0;
            createds[current].transform.position = new Vector3(30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
            sequence.Append(createds[cars.cars.Count - 1].transform.DOMoveX(-30f, 1.5f));
        }
        else
        {
            sequence.Append(createds[current - 1].transform.DOMoveX(-30f, 1.5f));
            createds[current].transform.position = new Vector3(30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
        }
        startTime = Time.realtimeSinceStartup;
        isClickable = false;
        leftButton.enabled = false;
        rightButton.enabled = false;
        AddingFeatureToPanel();
    }

    public void AddingFeatureToPanel()
    {
        carPanel.GetComponent<CarPanelScript>().carName.text = "Car: " + createds[current].GetComponent<PoolCars>().carName.ToString();
        carPanel.GetComponent<CarPanelScript>().carSpeed.text = "Speed: " + createds[current].GetComponent<PoolCars>().speed.ToString();
        carPanel.GetComponent<CarPanelScript>().carBody.text = "Body: " + createds[current].GetComponent<PoolCars>().carBody.ToString();
    }

    public void DestroySequence()
    {
        sequence.Kill();
    }

    public void BackFromSettings()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void BackFromCarSelect()
    {
        menu.SetActive(true);
        carSelect.SetActive(false);
    }

    public void PlayEndless()
    {
        SceneManager.LoadScene(1);
    }

    public void CarSelectMenu()
    {
        menu.SetActive(false);
        carSelect.SetActive(true);
        current = GameDataReferences.Instance.carIndex;
        for (int i = 0; i < current; i++)
        {
            createds[i].gameObject.transform.position = new Vector3(-30f, 0f, 0f);
        }
        createds[current].gameObject.transform.position = new Vector3(0f, 0f, 0f);
        for (int i = current + 1; i < cars.cars.Count; i++) 
        {
            createds[i].gameObject.transform.position = new Vector3(30f, 0f, 0f);
        }
    }

    public void OptionMenu()
    {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
