
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    int currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level",1);
        LoadScene("Level " + Mathf.Ceil(currentLevel / 3f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }
}
