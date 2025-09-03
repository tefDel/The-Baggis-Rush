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

                // Pausar timer antes de leer tiempo
                if (musicTimer != null) musicTimer.PauseTimer();

                // Obtener tiempos
                float tiempoTranscurrido = musicTimer != null ? musicTimer.GetTiempoTranscurrido() : 0f;
                float tiempoRestante = musicTimer != null ? musicTimer.GetTiempoRestante() : 0f;

                Debug.Log($"[PuertaFinal] Tiempo restante = {tiempoRestante:F2}, transcurrido = {tiempoTranscurrido:F2}");

                string tiempoTexto = FormatearTiempo(tiempoTranscurrido);

                // Mostrar mensaje de victoria
                MostrarMensaje(msgVictoria + tiempoTexto, 10f);

                // --- Guardar en GameDataManager usando LOS DATOS REALES del jugador ---
                if (GameDataManager.Instance != null)
                {
                    var gdm = GameDataManager.Instance;

                    // Debug para confirmar qué valores se guardarán
                    Debug.Log($"[PuertaFinal] Guardando resultado: nombre='{gdm.nombreJugador}', edad={gdm.edadJugador}, email='{gdm.emailJugador}', ciudad='{gdm.ciudadJugador}', tiempo={tiempoTranscurrido}");

                    // Usa los campos guardados en el GameDataManager (rellenados por tu UI)
                    gdm.GuardarResultado(
                        string.IsNullOrEmpty(gdm.nombreJugador) ? "Jugador" : gdm.nombreJugador,
                        gdm.edadJugador,
                        string.IsNullOrEmpty(gdm.emailJugador) ? "Sin email" : gdm.emailJugador,
                        string.IsNullOrEmpty(gdm.ciudadJugador) ? "Sin ciudad" : gdm.ciudadJugador,
                        tiempoTranscurrido
                    );
                }
                else
                {
                    Debug.LogWarning("[PuertaFinal] GameDataManager.Instance es null. No se guardó el resultado.");
                }

                // Volver al menú
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

    IEnumerator EsconderMensaje(float segundos)
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
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Inicial");
    }
}
