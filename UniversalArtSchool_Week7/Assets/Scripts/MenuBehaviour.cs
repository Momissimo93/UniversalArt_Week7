using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using TMPro;

//This class will handle direclty with the menu, but the value are send to the GameMenager

public class MenuBehaviour : MonoBehaviour
{

    bool isSomethingSaved;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [SerializeField] Slider volumeSlider = null;
    [SerializeField] Text volumeTextValue = null;

    [Header("Resolution Dropdowns")]
    [SerializeField] Dropdown m_Dropdown;
    private Resolution[] screenRosolutions;

    // Start is called before the first frame update
    void Start()
    {
        isSomethingSaved = false;
        SetScreenResolutions(m_Dropdown);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGameYes()
    {
        GameManager.LoadScene("Level1");
    }

    //This was done to experiment with script interaction. But i have mostly used Unity native methods to open and close menu windows
    public void LoadGameYes()
    {
        if (isSomethingSaved)
        {
            //GameManager.LoadScene("SceneToLoad");
        }
        else
        {
            Debug.Log("Nothing has been saved");
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton ()
    {
        GameManager.ExitGame();
    }

    public void SetVolume(float volume)
    {
        //GameManager.SetMasterVolume(volume);
        GameManager.ChangeVolume(volume);
        volumeTextValue.text = volume.ToString("0.0");
    }

    // I simply follow the instructions that are in the Unity API
    public void SetScreenResolutions(Dropdown mD)
    {
        screenRosolutions = Screen.resolutions;
        m_Dropdown.ClearOptions();

        List<string> m_DropOptions = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < screenRosolutions.Length; i++)
        {
            string option = screenRosolutions[i].width + " x " + screenRosolutions[i].height;

            m_DropOptions.Add(option);

            if(screenRosolutions[i].width == Screen.width && screenRosolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        m_Dropdown.AddOptions(m_DropOptions);
        m_Dropdown.value = currentResolutionIndex;
        m_Dropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        GameManager.SetResolution(screenRosolutions, resolutionIndex);
    }
}
