using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroAnimacion : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camara;
    public Transform posicionInicialCamara;  // mirando al techo
    public Transform posicionFinalCamara;    // sentado / normal
    public CanvasGroup fadePanel;            // panel negro fullscreen
    public MusicTimer musicTimer;
    public MovimientoPJ movimientoPJ;

    [Header("Audio Intro")]
    public AudioClip sonidoBostezo; 

    [Header("Config Animación")]
    public float duracionParpadeo = 0.4f;
    public float duracionRotacion = 2f;
    public float delayAntesJuego = 0.5f;

    // posiciones originales
    private Vector3 camPosOriginal;
    private Quaternion camRotOriginal;
    private AudioSource audioSource;

    private void Start()
    {
        // Crear automáticamente un AudioSource para reproducir el bostezo
        audioSource = gameObject.AddComponent<AudioSource>();

        // Guardar la posición original de la cámara (la que tienes en el Player)
        if (camara != null)
        {
            camPosOriginal = camara.transform.position;
            camRotOriginal = camara.transform.rotation;

            // Colocamos cámara en "acostado"
            if (posicionInicialCamara != null)
            {
                camara.transform.position = posicionInicialCamara.position;
                camara.transform.rotation = posicionInicialCamara.rotation;
            }
        }

        if (fadePanel != null)
            fadePanel.alpha = 1f; // empieza todo oscuro

        if (musicTimer != null)
            musicTimer.PauseTimer(); // detenemos timer

        if (movimientoPJ != null)
            movimientoPJ.enabled = false; // jugador no se puede mover

        // Iniciar secuencia
        StartCoroutine(SecuenciaIntro());
    }

    private IEnumerator SecuenciaIntro()
    {
        // --- Parpadeo realista ---
        yield return StartCoroutine(Fade(1f, 0.6f, duracionParpadeo));
        yield return StartCoroutine(Fade(0.6f, 1f, duracionParpadeo * 0.8f));
        yield return StartCoroutine(Fade(1f, 0.3f, duracionParpadeo));
        yield return StartCoroutine(Fade(0.3f, 0f, duracionParpadeo * 1.2f));

        // --- Bostezo justo antes de levantarse ---
        if (sonidoBostezo != null)
            audioSource.PlayOneShot(sonidoBostezo);

        // --- Rotación de cámara hacia sentado ---
        if (camara != null && posicionFinalCamara != null)
        {
            float t = 0f;
            Vector3 startPos = camara.transform.position;
            Quaternion startRot = camara.transform.rotation;

            while (t < 1f)
            {
                t += Time.deltaTime / duracionRotacion;
                camara.transform.position = Vector3.Lerp(startPos, posicionFinalCamara.position, t);
                camara.transform.rotation = Quaternion.Slerp(startRot, posicionFinalCamara.rotation, t);
                yield return null;
            }
        }

        yield return new WaitForSeconds(delayAntesJuego);

        // --- Restaurar cámara al Player ---
        if (camara != null)
        {
            camara.transform.position = camPosOriginal;
            camara.transform.rotation = camRotOriginal;
        }

        // Arrancar juego
        if (musicTimer != null) musicTimer.StartTimer();
        if (movimientoPJ != null) movimientoPJ.enabled = true;
    }

    private IEnumerator Fade(float from, float to, float duracion)
    {
        if (fadePanel == null) yield break;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duracion;
            fadePanel.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }
}
