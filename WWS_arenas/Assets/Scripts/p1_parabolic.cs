using UnityEngine;

public class P1_parabolic : MonoBehaviour
{
    [Header("Parameters")]
    public float speed = 0;
    public float angle = 0;
    public float gravity = 0;

    private float rad = 0;
    private float Vx = 0;
    private float Vy = 0;
    private Vector2 launchDir;

    private bool canShoot = false;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && canShoot == false)
        {
            // Aquí se puede agregar un sonido de disparo PlayOneShot
            Angle();
            canShoot = true;
        }

        if (canShoot)
        {
            launchDir.y += gravity * Time.deltaTime; // Se aplica la gravedad

            transform.position += new Vector3(launchDir.x * Time.deltaTime, launchDir.y * Time.deltaTime);
        }
    }

    private void OnBecameInvisible() // Reset Position
    {
        // Aquí se puede agregar un sonido de explosión PlayOneShot

        canShoot = false;
        transform.position = new Vector3(-5, 0);
    }

    private void Angle() // Se calcula la dirección de lanzamiento a partir del angulo deseado
    {
        rad = angle * Mathf.Deg2Rad;
        Vx = speed * Mathf.Cos(rad);
        Vy = speed * Mathf.Sin(rad);

        launchDir = new Vector2(Vx, Vy);
        float Degrees = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;
        Debug.Log(Degrees + " grados de lanzamiento"); //Se corrobora el angulo de lanzamiento
    }
}
