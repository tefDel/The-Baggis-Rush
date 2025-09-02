using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MusicTimer : MonoBehaviour
{
    [Header("Música")]
    public AudioSource musica;
    public float tiempoLimite; // empieza en 1 minuto
    public float tiempoAcelerar = 20f;
    public float pitchMax = 1.5f;

    [Header("UI Timer")]
    public TextMeshProUGUI textoTimer;
    public Color colorNormal = Color.white;
    public Color colorCritico = Color.red;
    public float parpadeoVelocidad = 5f;
    public float tiempoParpadeo = 5f;

    [Header("Mensajes Fin")]
    public GameObject panelMensajes;
    public TextMeshProUGUI mensajeUI;
    public string msgPerder = "Uy... perdiste tu vuelo. :( \n¿Reintentar?";

    private float tiempoRestante;
    private bool isRunning = true;
    public DialogoManager dialogueManager;

    void Start()
    {
        ResetTimer();
        if (musica != null)
        {
            musica.loop = true;
            musica.pitch = 1f;
            musica.Play();
        }
        ActualizarTexto();
    }

    void Update()
    {
        if (!isRunning) return;

        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante < 0f) tiempoRestante = 0f;

        if (tiempoRestante <= tiempoAcelerar && musica != null)
        {
            float progreso = 1f - (tiempoRestante / tiempoAcelerar);
            musica.pitch = Mathf.Lerp(1f, pitchMax, progreso);
        }

        ActualizarTexto();

        if (tiempoRestante <= 0f)
        {
            isRunning = false;
            Debug.Log("[MusicTimer] Tiempo terminado.");

            // mensaje de derrota
            if (panelMensajes != null && mensajeUI != null)
            {
                panelMensajes.SetActive(true);
                mensajeUI.text = msgPerder;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }

            // si quieres parar música
            if (musica != null) musica.Stop();

            if (dialogueManager != null)
                dialogueManager.SendMessage("EndConversation");

            MovimientoPJ movimiento = FindObjectOfType<MovimientoPJ>();
            if (movimiento != null)
            {
                movimiento.enabled = false;
            }
        }
    }

    void ActualizarTexto()
    {
        if (textoTimer == null) return;

        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

        // siempre mostramos TIEMPO RESTANTE en pantalla
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        if (tiempoRestante <= tiempoAcelerar)
        {
            textoTimer.color = colorCritico;
            if (tiempoRestante <= tiempoParpadeo)
            {
                float alpha = Mathf.Abs(Mathf.Sin(Time.time * parpadeoVelocidad));
                textoTimer.color = new Color(colorCritico.r, colorCritico.g, colorCritico.b, alpha);
            }
        }
        else
        {
            textoTimer.color = colorNormal;
        }
    }

    // Getters públicos
    public float GetTiempoRestante() => tiempoRestante;
    public float GetTiempoTranscurrido() => Mathf.Clamp(tiempoLimite - tiempoRestante, 0f, tiempoLimite);

    // Control del timer
    public void StartTimer() { isRunning = true; }
    public void PauseTimer() { isRunning = false; }
    public void ResetTimer()
    {
        tiempoRestante = tiempoLimite;
        isRunning = true;
        if (musica != null) musica.pitch = 1f;
        ActualizarTexto();
    }

    public void ReiniciarEscena()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void AgregarTiempo(float segundos)
    {
        tiempoRestante += segundos;

        // opcional: evitar que pase del tiempo límite inicial
        if (tiempoRestante > tiempoLimite)
            tiempoRestante = tiempoLimite;

        ActualizarTexto();
    }
}
