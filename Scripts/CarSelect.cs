using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelect : MonoBehaviour
{
    //private static readonly string BackgroundPref = "BackgroundPref";
    private int current;
    public List<GameObject> cars;
    private List<GameObject> createds;
    public GameObject carPanel;
    public Sequence sequence;

    public void Start()
    {
        current =  5;//PlayerPrefs.GetInt();
        createds = new List<GameObject>();
        foreach(GameObject obj in cars)
        { 
            createds.Add(Instantiate(obj, new Vector3(-30f, 0f, 0f), Quaternion.identity));
        }
        
        createds[current].transform.DOMoveX(0f, 1.5f);
        AddingFeatureToPanel();
        sequence = DOTween.Sequence();

    }

    public void SelectButton()
    {
        SelectedCarScript.Instance.selected = cars[current];
        SceneManager.LoadScene(0);
        DestroySequence();
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
        DestroySequence();
    }

    public void RightButton()
    {
        current--;
        if (current == -1)
        {
            current = cars.Count-1;
            createds[current].transform.position = new Vector3(-30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
            sequence.Append(createds[0].transform.DOMoveX(30f, 2));
        }
        else
        {
            sequence.Append(createds[current + 1].transform.DOMoveX(30f, 1.5f));
            createds[current].transform.position = new Vector3(-30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
        }
        
        AddingFeatureToPanel();
    }

    public void LeftButton()
    {
        current++;
        if (current == cars.Count)
        {
            current = 0;
            createds[current].transform.position = new Vector3(30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
            sequence.Append(createds[cars.Count-1].transform.DOMoveX(-30f, 1.5f));
        }
        else
        {
            sequence.Append(createds[current - 1].transform.DOMoveX(-30f, 1.5f));
            createds[current].transform.position = new Vector3(30f, 0f, 0f);
            sequence.Append(createds[current].transform.DOMoveX(0f, 1.5f));
        }
        AddingFeatureToPanel();
    }

    public void AddingFeatureToPanel()
    {
        carPanel.GetComponent<CarPanelScript>().carName.text = "Car: "+ createds[current].GetComponent<CloneCars>().carname.ToString();
        carPanel.GetComponent<CarPanelScript>().carSpeed.text = "Speed: " + createds[current].GetComponent<CloneCars>().speed.ToString();
        carPanel.GetComponent<CarPanelScript>().carBody.text = "Body: " + createds[current].GetComponent<CloneCars>().body.ToString();
    }

    public void DestroySequence()
    {
        sequence.Kill();
    }

}
