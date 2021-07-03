using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierWheel : MonoBehaviour
{
    [SerializeField] Transform arrow;
    [SerializeField] protected Vector3 m_from = new Vector3(0.0F, 45.0F, 0.0F);
    [SerializeField] protected Vector3 m_to = new Vector3(0.0F, -45.0F, 0.0F);
    [SerializeField] protected float m_frequency = 1.0F;

    private int currentMultiplier;
    private bool isSpinning;
    // Start is called before the first frame update
    void Start()
    {
        Spin();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            Quaternion from = Quaternion.Euler(this.m_from);
            Quaternion to = Quaternion.Euler(this.m_to);

            float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * this.m_frequency));
            arrow.transform.localRotation = Quaternion.Lerp(from, to, lerp);

            if(WrapAngle(arrow.transform.localEulerAngles.z) >= -20 && WrapAngle(arrow.transform.localEulerAngles.z)<= 22f)
            {
                currentMultiplier = 5;
            }
            if ((WrapAngle(arrow.transform.localEulerAngles.z) <= 90f && WrapAngle(arrow.transform.localEulerAngles.z) >= 60f) || (WrapAngle(arrow.transform.localEulerAngles.z) >= -90f && WrapAngle(arrow.transform.localEulerAngles.z) <= -60f))
            {
                currentMultiplier = 2;
            }

            if ((WrapAngle(arrow.transform.localEulerAngles.z) >= -60f && WrapAngle(arrow.transform.localEulerAngles.z) <= -22f) || (WrapAngle(arrow.transform.localEulerAngles.z) <= 60f && WrapAngle(arrow.transform.localEulerAngles.z) >= 22f))
            {
                currentMultiplier = 3;
            }
           GameManager.Instance.UpdateMultiplier(currentMultiplier);
        }

    }
    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

  
    public void Spin()
    {
        isSpinning = true;
    }

    public int GetCurrentMultiplier()
    {
        
        return currentMultiplier;
    }
}
