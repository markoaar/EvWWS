using UnityEngine;

public class P3_stateMachine : MonoBehaviour
{
    // Estado
    private CBState currentState;
    enum CBState { IDLE_ST, RUN_ST, JUMP_ST, ROLL_ST };

    // Componentes
    private Transform cowB_T = null;
    private SpriteRenderer cowB_SR = null;
    private Animator cowB_An = null;

    // Parametros
    private readonly float speed = 3.5f;
    private float floorPos; // Limite del suelo

    private Vector3 velocity;
    [Header("Parameters")]
    [SerializeField] private float gravity = 0;
    [SerializeField] private float jumpForce = 0;

    private bool onAir = false;
    private bool canJump = false;

    private void Start()
    {
        currentState = CBState.IDLE_ST;

        cowB_T = GetComponent<Transform>();
        cowB_SR = GetComponent<SpriteRenderer>();
        cowB_An = GetComponent<Animator>();

        floorPos = cowB_T.position.y;

        velocity = Vector3.zero;
    }

    private void Update()
    {
        Debug.Log(currentState);

        switch(currentState)
        {
            #region IDLE STATE
            case CBState.IDLE_ST:

                // Al ingresar al estado se cancela cualquier otra animación/estado
                cowB_An.SetFloat("move", 0);
                cowB_An.SetBool("jump", false);
                cowB_An.SetBool("roll", false);

                // Condiciones de salida
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) currentState = CBState.RUN_ST;
                if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;
                if (Input.GetKey(KeyCode.LeftShift)) currentState = CBState.ROLL_ST;

                break;
            #endregion

            #region RUN STATE
            case CBState.RUN_ST:

                // Left
                if (Input.GetKey(KeyCode.A)) 
                {
                    // Aquí se puede agregar un sonido de pasos PlayOneShot
                    cowB_SR.flipX = true;
                    velocity.x = -speed;
                    cowB_An.SetFloat("move", -velocity.x);
                    
                    cowB_T.position += velocity * Time.deltaTime; // Se actualiza la dirección
                }

                // Right
                if (Input.GetKey(KeyCode.D)) 
                {
                    // Aquí se puede agregar un sonido de pasos PlayOneShot
                    cowB_SR.flipX = false;
                    velocity.x = speed;
                    cowB_An.SetFloat("move", velocity.x);

                    cowB_T.position += velocity * Time.deltaTime; // Se actualiza la dirección
                }

                // Condiciones de salida
                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    cowB_An.SetFloat("move", 0);
                    currentState = CBState.IDLE_ST;
                }
                if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;
                if (Input.GetKey(KeyCode.LeftShift)) currentState = CBState.ROLL_ST;

                break;
            #endregion

            #region JUMP STATE
            case CBState.JUMP_ST:

                // Jump
                if (Input.GetKey(KeyCode.Space) && onAir == false)
                {
                    // Aquí se puede agregar un sonido de salto PlayOneShot

                    // Al ingresar al estado se cancela cualquier otra animación/estado
                    cowB_An.SetBool("roll", false);
                    cowB_An.SetFloat("move", 0);
                    cowB_An.SetBool("jump", true);

                    canJump = true;
                    onAir = true;
                    velocity.y = jumpForce;
                }

                // Detección de suelo
                if (cowB_T.position.y < floorPos)
                {
                    // Aquí se puede agregar un sonido de caida PlayOneShot

                    cowB_An.SetBool("jump", false);

                    canJump = false;
                    onAir = false;
                    cowB_T.position = new Vector3(cowB_T.position.x, floorPos); // Se resetea la pos.y

                    velocity = Vector3.zero; //Se resetea la velocidad para evitar movimiento vertical

                    // Condiciones de salida
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) currentState = CBState.RUN_ST;
                    currentState = CBState.IDLE_ST;
                }

                // Detección de salto
                if (canJump)
                {
                    velocity.y += gravity * Time.deltaTime; // Se aplica la gravedad
                    cowB_T.position += velocity * Time.deltaTime; // Se actualiza con la dirección
                }

                break;
            #endregion

            #region ROLL STATE
            case CBState.ROLL_ST:

                // Solo funciona en el suelo (consideración personal)
                if (cowB_T.position.y <= floorPos) 
                {
                    //Roll
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        // Aquí se puede agregar un sonido de rolling PlayOneShot

                        // Al ingresar al estado se cancela cualquier otra animación/estado
                        cowB_An.SetFloat("move", 0);
                        cowB_An.SetBool("roll", true);

                        // Permitir movimientos laterales en estado Roll
                        if (Input.GetKey(KeyCode.A))
                        {
                            cowB_SR.flipX = true;
                            velocity.x = -speed;

                            cowB_T.position += velocity * Time.deltaTime; 
                        }

                        if (Input.GetKey(KeyCode.D))
                        {
                            cowB_SR.flipX = false;
                            velocity.x = speed;

                            cowB_T.position += velocity * Time.deltaTime;
                        }
                    }
                }

                // Condiciones de salida
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    cowB_An.SetBool("roll", false);
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) currentState = CBState.RUN_ST;
                    if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;
                    currentState = CBState.IDLE_ST;
                }

                if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;

                break;
                #endregion
        }
    }
}
