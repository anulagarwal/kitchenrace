using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance = null;

    public bool isVibrateOn;
    public bool isSoundOn;
    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
       if(PlayerPrefs.GetInt("sound",1) == 0)
        {
            isSoundOn = false;
        }
        else
        {
            isSoundOn = true;
        }

        if (PlayerPrefs.GetInt("vibrate", 1) == 0)
        {
            isVibrateOn = false;
        }
        else
        {
            isVibrateOn = true;
        }

        LevelUIManager.Instance.UpdateSettings(isSoundOn, isVibrateOn);
    }

    public void EnableSound()
    {
        isSoundOn = true;
        PlayerPrefs.SetInt("sound", 1);
    }

    public void DisableSound()
    {
        isSoundOn = false;
        PlayerPrefs.SetInt("sound", 0);

    }

    public void EnableVibrate()
    {
        isVibrateOn = true;
        PlayerPrefs.SetInt("vibrate", 1);

    }

    public void DisableVibrate()
    {
        isVibrateOn = false;
        PlayerPrefs.SetInt("vibrate", 1);

    }


}
