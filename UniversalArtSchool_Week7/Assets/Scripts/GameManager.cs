using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


//The GameManager takes value from the MenuBehaviour class and update the game: mechanics, contents and functionalities
public class GameManager 
{
    //public static AudioSource master;
    public static float actualVolume;
    // Start is called before the first frame update

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public static void ExitGame()
    {
        Application.Quit();
    }

    //Returns the base 20 logarithm of a specified number as the gain is in Db
    /*public static void SetMasterVolume(float volume)
    {
        master.outputAudioMixerGroup.audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20) ;
    }*/

    public static void ChangeVolume(float volume)
    {
        AudioSource[] audioSource = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource aS in audioSource)
        {
            aS.volume = volume;

        }

        actualVolume = volume;
    }

    public static void SetResolution(Resolution [] sR, int i)
    {
        Debug.Log("Size: " + sR[i].width + " x " + sR[i].height);
        Resolution resolution = sR[i];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
