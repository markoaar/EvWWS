using UnityEngine;

public class P2_freefall : MonoBehaviour
{
    [Header("Parametros")]
    public float gravity = 0;
    public float angle = 0;

    private float rad = 0;
    private float Vx = 0;
    private float Vy = 0;
    private Vector2 launchDir;

    private bool fall = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && fall == false)
        {
            Angle();
            fall = true;
        }

        if (fall)
        {
            transform.position += new Vector3(launchDir.x * Time.deltaTime, launchDir.y * Time.deltaTime);
        }
    }

    private void OnBecameInvisible()
    {
        fall = false;
        transform.position = new Vector3(-0, 0);
    }

    private void Angle()
    {
        rad = (-90 - angle) * Mathf.Deg2Rad;
        Vx = gravity * Mathf.Cos(rad);
        Vy = gravity * Mathf.Sin(rad);

        launchDir = new Vector2(Vx, Vy);
        float Degrees = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;
        Debug.Log(Degrees); //Se corrobora el angulo de lanzamiento
    }
}
