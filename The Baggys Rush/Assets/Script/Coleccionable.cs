using UnityEngine;

public class Coleccionable : MonoBehaviour
{
    public string itemName;          // Debe coincidir con el nombre en la checklist
    public GameObject indicadorUI;   // Texto o icono que aparece al acercarse
    public KeyCode teclaRecoger = KeyCode.F;

    private bool jugadorCerca = false;

    void Start()
    {
        if (indicadorUI != null)
            indicadorUI.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(teclaRecoger))
        {
            Recoger();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (indicadorUI != null)
                indicadorUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (indicadorUI != null)
                indicadorUI.SetActive(false);
        }
    }

    void Recoger()
    {
        // Avisamos al manager
        CheckListManager.Instance.MarkAsObtained(itemName);

        // Ocultamos el indicador
        if (indicadorUI != null)
            indicadorUI.SetActive(false);

        // Hacemos desaparecer el objeto
        gameObject.SetActive(false);
    }
}
