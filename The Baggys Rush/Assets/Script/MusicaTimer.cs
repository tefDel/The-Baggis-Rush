using UnityEngine;

public class MusicTimer : MonoBehaviour
{
    public AudioSource musica;     
    public float tiempoLimite = 45f;  
    public float tiempoAcelerar = 20f; 
    public float pitchMax = 1.5f;     
    private float tiempoRestante;
    public DialogoManager dialogueManager;
    void Start()
    {
        tiempoRestante = tiempoLimite;
        musica.loop = true;
        musica.pitch = 1f; // velocidad normal
        musica.Play();
    }

    void Update()
    {
        // Reducimos el tiempo
        tiempoRestante -= Time.deltaTime;

        // Si estamos dentro del rango de aceleración
        if (tiempoRestante <= tiempoAcelerar)
        {
            float progreso = 1f - (tiempoRestante / tiempoAcelerar);
            musica.pitch = Mathf.Lerp(1f, pitchMax, progreso);
        }

        // Cuando el tiempo se acaba
        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            // Aquí puedes poner que termine el juego, se pierda, etc.
            Debug.Log("¡Tiempo terminado!");
            // cerrar diálogo si está abierto
            if (dialogueManager != null)
            {
                dialogueManager.SendMessage("EndConversation");
            }
        }
    }
}
