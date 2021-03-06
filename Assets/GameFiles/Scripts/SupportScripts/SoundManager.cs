using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance = null;

    [Header("Component References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private List<Sound> sounds;

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


    public void PlaySound(SoundType type)
    {
        if (SettingsManager.Instance.isSoundOn && type != SoundType.UI)
        {
            audioSource.clip = sounds.Find(x => x.type == type).clip;
            audioSource.Play();
        }
        else if (SettingsManager.Instance.isSoundOn)
        {
            audioSource2.clip = sounds.Find(x => x.type == type).clip;
            audioSource2.Play();
        }
    }
}
