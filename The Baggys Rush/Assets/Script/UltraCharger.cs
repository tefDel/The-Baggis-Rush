using UnityEngine;
using System.Collections;

public class PowerUpCharger : MonoBehaviour
{
    [Header("Efectos visuales")]
    public GameObject efectosVisuales; // luces/partículas que deben apagarse
    public float duracionPowerUp = 20f;

    [Header("Referencias")]
    public MusicTimer musicTimer;   // Asignar en inspector
    public AudioSource musica;      // La música principal
    public AudioSource sonidoPower; // 🔊 Sonido mágico del cargador
    public Camera mainCamera;       // Cámara principal para efectos

    [Header("Efecto cámara")]
    public float zoomFactor = 0.8f;     // cuánto acercar la cámara
    public float zoomSpeed = 2f;        // velocidad transición zoom
    public float shakeIntensity = 0.2f; // fuerza del shake
    public float shakeDuration = 1f;    // cuánto dura la vibración inicial

    private bool activo = false;
    private float originalFOV;
    private Vector3 originalCamPos;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
            originalCamPos = mainCamera.transform.localPosition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!activo && other.CompareTag("Player"))
        {
            StartCoroutine(ActivarPowerUp());
        }
    }

    IEnumerator ActivarPowerUp()
    {
        activo = true;

        // 1. Apagar efectos visuales del objeto
        if (efectosVisuales != null)
            efectosVisuales.SetActive(false);

        // 2. Pausar música y congelar cronómetro
        if (musica != null) musica.Pause();
        if (musicTimer != null) musicTimer.PauseTimer();

        // 3. 🔊 Reproducir sonido mágico del cargador
        if (sonidoPower != null)
        {
            sonidoPower.loop = true; // 🔁 se mantiene activo todo el power-up
            sonidoPower.Play();
        }

        // 4. Efecto cámara → zoom + vibración
        if (mainCamera != null)
        {
            StartCoroutine(CameraZoom(originalFOV * zoomFactor));
            StartCoroutine(CameraShake(shakeDuration, shakeIntensity));
        }

        // Esperar la duración
        yield return new WaitForSeconds(duracionPowerUp);

        // 5. Reanudar música y timer
        if (musica != null) musica.UnPause();
        if (musicTimer != null) musicTimer.StartTimer();

        // 6. 🚨 Detener sonido mágico
        if (sonidoPower != null) sonidoPower.Stop();

        // 7. Restaurar zoom de cámara
        if (mainCamera != null)
            StartCoroutine(CameraZoom(originalFOV));

        // 8. Desaparecer el objeto
        gameObject.SetActive(false);
    }

    IEnumerator CameraZoom(float targetFOV)
    {
        float t = 0f;
        float startFOV = mainCamera.fieldOfView;

        while (t < 1f)
        {
            t += Time.deltaTime * zoomSpeed;
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }
    }

    IEnumerator CameraShake(float duration, float intensity)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Vector3 offset = Random.insideUnitSphere * intensity;
            mainCamera.transform.localPosition = originalCamPos + offset;
            yield return null;
        }

        mainCamera.transform.localPosition = originalCamPos;
    }
}
