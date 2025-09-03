using UnityEngine;

public class PuertaAuto : MonoBehaviour
{
    [Header("Referencias")]
    public Transform puerta;          // Asigna aqu� el objeto de la puerta

    [Header("Apertura")]
    public float anguloApertura = 90f; // Grados que se abrir�
    public float velocidad = 2f;      // Velocidad de apertura/cierre

    [Header("Opciones Secretas")]
    public bool esPuertaSecreta = false; // Si es secreta, necesita objetos previos
    public int requiredItems = 4;        // Cu�ntos objetos debe tener el jugador

    private Quaternion rotacionInicial;
    private Quaternion rotacionAbierta;
    private bool jugadorCerca = false;
    private bool isUnlocked = false;

    void Start()
    {
        // Guardamos la rotaci�n inicial de la puerta
        rotacionInicial = puerta.rotation;
        // Calculamos la rotaci�n abierta (girando en Y por ejemplo)
        rotacionAbierta = Quaternion.Euler(puerta.eulerAngles + new Vector3(0, anguloApertura, 0));
    }

    void Update()
    {
        if (jugadorCerca && (!esPuertaSecreta || isUnlocked))
        {
            // Interpolamos hacia la posici�n abierta
            puerta.rotation = Quaternion.Lerp(puerta.rotation, rotacionAbierta, Time.deltaTime * velocidad);
        }
        else
        {
            // Si el jugador se aleja o a�n no se desbloquea, cerramos la puerta
            puerta.rotation = Quaternion.Lerp(puerta.rotation, rotacionInicial, Time.deltaTime * velocidad);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (esPuertaSecreta)
            {
                int collected = CountCollectedItems();

                if (collected >= requiredItems)
                {
                    isUnlocked = true;
                    Debug.Log("[PuertaAuto] �Puerta secreta desbloqueada!");
                }
                else
                {
                    Debug.Log("[PuertaAuto] A�n faltan objetos para abrir la puerta secreta.");
                }
            }

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

    int CountCollectedItems()
    {
        int count = 0;
        if (CheckListManager.Instance != null)
        {
            foreach (var item in CheckListManager.Instance.items)
            {
                if (item.obtained) count++;
            }
        }
        return count;
    }
}
