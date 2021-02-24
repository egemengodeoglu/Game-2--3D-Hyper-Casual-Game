using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerEndless : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gamePanel;
    public GameObject dieMenuUI;
    public GameObject finishMenuUI;
    public GameObject themaChooser;
    private float startTime;

    private MenuControlEndless panelManagement;

    public void Start()
    {
        panelManagement = GameObject.FindObjectOfType<MenuControlEndless>();
        panelManagement.ChangePanel += ChangePanel;
        startTime = Time.time;
        pauseMenuUI.SetActive(false);
        gamePanel.SetActive(false);
        dieMenuUI.SetActive(false);
        finishMenuUI.SetActive(false);
        themaChooser.SetActive(true);

    }

    public void Update()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        gamePanel.GetComponent<GamePanelScript>().timeText.text = "Time: " + minutes + " . " + seconds;
    }

    public string DieTimer()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        string tmp = "Time: " + minutes + "." + seconds;
        return tmp;
    }

    private void ChangePanel(string chooser, string data1, string data2)
    {
        switch (chooser)
        {
            case "Die":
                dieMenuUI.SetActive(true);
                gamePanel.SetActive(false);
                dieMenuUI.GetComponent<DieMenuScript>().dieTimeText.text = DieTimer().ToString();
                dieMenuUI.GetComponent<DieMenuScript>().cointText.text = "Coin = " + data2.ToString();
                break;

            case "Finish":
                finishMenuUI.SetActive(true);
                gamePanel.SetActive(false);
                finishMenuUI.GetComponent<FinishMenuScript>().finishTimeText.text = DieTimer().ToString();
                finishMenuUI.GetComponent<FinishMenuScript>().finishCoinText.text = data2;
                break;

            case "Coin":
                gamePanel.GetComponent<GamePanelScript>().coinText.text = "Coin = " + data2;
                break;

            case "Resume":
                pauseMenuUI.SetActive(false);
                break;

            case "Pause":
                pauseMenuUI.SetActive(true);
                break;

            case "Retry":
                dieMenuUI.SetActive(false);
                gamePanel.SetActive(true);

                break;

            case "StartGame":
                gamePanel.GetComponent<GamePanelScript>().coinText.enabled = true;
                gamePanel.GetComponent<GamePanelScript>().timeText.enabled = true;
                gamePanel.GetComponent<GamePanelScript>().countDownText.enabled = false;
                gamePanel.SetActive(true);
                break;

            case "CountDown":
                gamePanel.GetComponent<GamePanelScript>().coinText.enabled = false;
                gamePanel.GetComponent<GamePanelScript>().timeText.enabled = false;
                gamePanel.GetComponent<GamePanelScript>().powerUpText.enabled = false;
                gamePanel.GetComponent<GamePanelScript>().countDownText.enabled = true;
                gamePanel.GetComponent<GamePanelScript>().countDownText.text = data1;
                gamePanel.SetActive(true);
                break;

            case "PowerUp":
                gamePanel.GetComponent<GamePanelScript>().powerUpText.enabled = true;
                gamePanel.GetComponent<GamePanelScript>().powerUpText.text = data1;
                break;

            case "Thema":
                themaChooser.SetActive(false);
                break;

            default:
                break;
        }

    }

}
