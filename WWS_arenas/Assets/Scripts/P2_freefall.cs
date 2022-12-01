using UnityEngine;

public class P2_freefall : MonoBehaviour
{
    [Header("Parameters")]
    public float gravity = 0;
    public float inclination = 0;

    private float rad = 0;
    private float Vx = 0;
    private float Vy = 0;
    private Vector2 fallDir;

    private bool fall = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && fall == false)
        {
            // Aquí se puede agregar un sonido de descenso PlayOneShot
            Angle();
            fall = true;
        }

        if (fall)
        {
            transform.position += new Vector3(fallDir.x * Time.deltaTime, fallDir.y * Time.deltaTime); // Se aplica la dirección de caída
        }
    }

    private void OnBecameInvisible()
    {
        // Aquí se puede agregar un sonido de colisión PlayOneShot

        fall = false;
        transform.position = new Vector3(-0, 0);
    }

    private void Angle() // Se calcula el angulo de inclinación de caída
    {
        rad = (-90 - inclination) * Mathf.Deg2Rad;
        Vx = gravity * Mathf.Cos(rad);
        Vy = gravity * Mathf.Sin(rad);

        fallDir = new Vector2(Vx, Vy);
        float Degrees = Mathf.Atan2(fallDir.y, fallDir.x) * Mathf.Rad2Deg;
        Debug.Log(Degrees + " grados de inclinación"); //Se corrobora el angulo de inclinación
    }
}
