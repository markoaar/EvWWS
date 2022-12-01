using System.Collections.Generic;
using UnityEngine;

public class P4_pooling : MonoBehaviour
{
    // Script mixto para efecto de la evaluación, puede ir cada propiedad como script por separado (Bullet & Revolver)
    public string objectType; // Detección de tipo de objeto a funcionar

    #region Revolver Type
    [Header("Pooling Properties | Revolver")]
    // Pooling Parametros
    [SerializeField] private int poolSize;
    private List<GameObject> ammoPool;

    [Header("Revolver Properties | Revolver")]
    // Revolver Parametros
    [SerializeField] private GameObject bulletGO = null;
    [SerializeField] private float shootDelay;
    private float currentShoot = 0;
    private Transform aimGO = null;
    #endregion

    #region Bullet Type
    [Header("Bullet Properties | Bullet")]
    // Bullet Parametros
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int lifeTime;
    #endregion

    private void Start()
    {
        aimGO = GameObject.Find("shootAim").GetComponent<Transform>();

        // Se genera el pool de objetos
        ammoPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullets = Instantiate(bulletGO);
            bullets.SetActive(false);
            ammoPool.Add(bullets);
        }
    }

    private void Update()
    {
        switch (objectType)
        {
            case "Bullet":

                // Movimiento lateral perpetuo
                transform.position += new Vector3(bulletSpeed, 0) * Time.deltaTime;

                break;

            case "Revolver":
                
                // Delay de disparo
                currentShoot += Time.deltaTime;
                
                for (int j = 0; j < ammoPool.Count; j++)
                {
                    if(!ammoPool[j].activeInHierarchy)
                    {
                        if (currentShoot > shootDelay)
                        {
                            // Aquí se puede agregar un sonido de disparo PlayOneShot
                            currentShoot = 0;
                            // Se resetea la posición del objeto y se activa
                            ammoPool[j].transform.SetPositionAndRotation(aimGO.position, aimGO.rotation);
                            ammoPool[j].SetActive(true);
                        }    
                    }   
                }

                break;
        }
    }

    // Loop de activación/desactivación condicionado al tipo de objeto (Bullet)
    #region Bullet Active Loop
    private void OnEnable()
    {
        if (objectType == "Bullet") Invoke(nameof(HideBullet), lifeTime);
    }

    private void HideBullet()
    {
        // Aquí se puede agregar un sonido de colisión PlayOneShot

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    #endregion
}