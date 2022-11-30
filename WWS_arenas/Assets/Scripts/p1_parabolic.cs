using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1_parabolic : MonoBehaviour
{
    public float speed = 0;
    public float gravity = 0;
    public float angle = 0;
    private float rad = 0;
    private float Vx = 0;
    private float Vy = 0;
    Vector2 launchDir;

    bool shoot = false;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ///move = Time.deltaTime * speed;
            Angle();
            shoot = true;
        }

        if (shoot)
        {
            launchDir.y += gravity;

            transform.position += new Vector3(launchDir.x * Time.deltaTime, launchDir.y * Time.deltaTime);
        }
    }   

    private void Angle()
    {
        rad = angle * Mathf.Deg2Rad;

        Vx = speed * Mathf.Cos(rad);
        Vy = speed * Mathf.Sin(rad);

        launchDir = new Vector2(Vx, Vy);
        float Degrees = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;
        Debug.Log(Degrees);
    }
}
