using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    private Robot robot;

    public void SetUp(Robot robot)
    {
        this.robot = robot;
    }

    // Update is called once per frame
    void Update()
    {
        float rate = robot.health*1f / robot.maxHealth;
      
        transform.Find("Bar").localScale = new Vector3(rate, 1);
    }
}
