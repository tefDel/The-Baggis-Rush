using UnityEngine;
using TMPro;
using System.Collections;

public class PuertaFinal : MonoBehaviour
{
    [Header("Referencias")]
    public CheckListManager checklist;
    public MusicTimer musicTimer;
    public TextMeshProUGUI mensajeUI;
    public GameObject panelMensajes;
    public Transform puerta;

    [Header("Apertura de puerta")]
    public float anguloApertura = 90f;
    public float velocidad = 2f;

    [Header("Mensajes")]
    public string msgBloqueada = "¡Aún no has recogido todo lo necesario para el viaje!";
    public string msgVictoria = "¡Felicidades! Alcanzaste a llegar a tu vuelo JUSTO a tiempo.\nTiempo de completado: ";

    private Quaternion rotacionInicial;
    private Quaternion rotacionAbierta;
    private bool completado = false;
    private bool puedeAbrir = false;
    private Coroutine mensajeCoroutine;


    void Start()
    {
        if (puerta != null)
        {
            rotacionInicial = puerta.rotation;
            rotacionAbierta = Quaternion.Euler(puerta.eulerAngles + new Vector3(0, anguloApertura, 0));
        }

        if (panelMensajes != null) panelMensajes.SetActive(false);
    }

    void Update()
    {
        if (puerta == null) return;

        if (puedeAbrir)
            puerta.rotation = Quaternion.Lerp(puerta.rotation, rotacionAbierta, Time.deltaTime * velocidad);
        else
            puerta.rotation = Quaternion.Lerp(puerta.rotation, rotacionInicial, Time.deltaTime * velocidad);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[PuertaFinal] OnTriggerEnter con: " + other.name);
        if (!other.CompareTag("Player")) return;

        if (!TodosItemsCompletados())
        {
            MostrarMensaje(msgBloqueada, 3f);
        }
        else
        {
            if (!completado)
            {
                completado = true;
                puedeAbrir = true;

                // Tiempo de completado
                // 1) Pausar timer antes de leer tiempo
                if (musicTimer != null) musicTimer.PauseTimer();

                // 2) Obtener tiempo final
                float tiempoTranscurrido = musicTimer != null ? musicTimer.GetTiempoTranscurrido() : 0f;
                float tiempoRestante = musicTimer != null ? musicTimer.GetTiempoRestante() : 0f;

                Debug.Log($"[PuertaFinal] Tiempo restante = {tiempoRestante:F2}, transcurrido = {tiempoTranscurrido:F2}");

                string tiempoTexto = FormatearTiempo(tiempoTranscurrido);

                // 3) Mostrar mensaje de victoria
                MostrarMensaje(msgVictoria + tiempoTexto, 10f);

                // 4) Guardar en GameUIManager (esto alimenta el ranking)
                GameDataManager.Instance.GuardarResultado(GameDataManager.Instance.nombreJugador, tiempoTranscurrido);


                // 5) Volver al menú
                StartCoroutine(RegresarAlMenu());
            }
        }
    }

    bool TodosItemsCompletados()
    {
        if (checklist == null || checklist.items == null) return false;
        foreach (var it in checklist.items) if (!it.obtained) return false;
        return true;
    }

    void MostrarMensaje(string texto, float duracion)
    {
        if (mensajeUI == null || panelMensajes == null)
        {
            Debug.LogWarning("[PuertaFinal] mensajeUI o panelMensajes no asignados.");
            return;
        }

        if (mensajeCoroutine != null) StopCoroutine(mensajeCoroutine);

        panelMensajes.SetActive(true);
        mensajeUI.text = texto;
        mensajeCoroutine = StartCoroutine(EsconderMensaje(duracion));
    }

    System.Collections.IEnumerator EsconderMensaje(float segundos)
    {
        if (segundos > 0f)
        {
            yield return new WaitForSeconds(segundos);
            if (panelMensajes != null) panelMensajes.SetActive(false);
        }
    }

    string FormatearTiempo(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    private IEnumerator RegresarAlMenu()
    {
        yield return new WaitForSeconds(1f); // Espera 3 seg para que el jugador vea el mensaje
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Inicial"); // Reemplaza "Menu" con el nombre real de tu escena
    }
}
