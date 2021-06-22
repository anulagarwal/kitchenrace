using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AwesomeText : MonoBehaviour
{
    public float upSpeed;
    public float fadeRate;
    public float destroyDelay;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
        GetComponentInChildren<TextMeshPro>().alpha -= fadeRate;
    }
}
