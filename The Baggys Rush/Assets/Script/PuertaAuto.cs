using UnityEngine;

public class PuertaAuto : MonoBehaviour
{
    public Transform puerta;          // Asigna aqu� el objeto de la puerta
    public float anguloApertura = 90f; // Grados que se abrir�
    public float velocidad = 2f;      // Velocidad de apertura/cierre

    private Quaternion rotacionInicial;
    private Quaternion rotacionAbierta;
    private bool jugadorCerca = false;

    void Start()
    {
        // Guardamos la rotaci�n inicial de la puerta
        rotacionInicial = puerta.rotation;
        // Calculamos la rotaci�n abierta (girando en Y por ejemplo)
        rotacionAbierta = Quaternion.Euler(puerta.eulerAngles + new Vector3(0, anguloApertura, 0));
    }

    void Update()
    {
        if (jugadorCerca)
        {
            // Interpolamos hacia la posici�n abierta
            puerta.rotation = Quaternion.Lerp(puerta.rotation, rotacionAbierta, Time.deltaTime * velocidad);
        }
        else
        {
            // Si el jugador se aleja, cerramos la puerta
            puerta.rotation = Quaternion.Lerp(puerta.rotation, rotacionInicial, Time.deltaTime * velocidad);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}
