using UnityEngine;


[CreateAssetMenu(fileName = "SoundItem", menuName = "CustomObjects/Sound")]
public class Sound : ScriptableObject
{
    public SoundType type;
    public AudioClip clip;
}
