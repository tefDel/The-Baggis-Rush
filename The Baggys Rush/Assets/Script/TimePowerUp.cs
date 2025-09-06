using UnityEngine;

public class TimePowerUp : MonoBehaviour
{
    [Header("Configuración del PowerUp")]
    public float tiempoExtra;

    [Header("Spawning")]
    public GameObject powerUpPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 30f;

    [Header("Audio")]
    public AudioClip sonidoPickup;

    private float timer;
    private GameObject currentPowerUp;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentPowerUp == null)
        {
            SpawnPowerUp();
            timer = 0f;
        }
    }

    void SpawnPowerUp()
    {
        if (spawnPoints.Length == 0 || powerUpPrefab == null) return;

        int index = Random.Range(0, spawnPoints.Length);
        currentPowerUp = Instantiate(powerUpPrefab, spawnPoints[index].position, Quaternion.identity);

        Collider col = currentPowerUp.GetComponent<Collider>();
        if (col == null)
        {
            col = currentPowerUp.AddComponent<BoxCollider>();
            col.isTrigger = true;
        }

        AudioSource audioSource = currentPowerUp.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = currentPowerUp.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        LevitateAndRotate levitate = currentPowerUp.AddComponent<LevitateAndRotate>();

        PowerUpPickup pickup = currentPowerUp.AddComponent<PowerUpPickup>();
        pickup.Init(this, tiempoExtra, sonidoPickup, audioSource);
    }

    public void OnPowerUpCollected()
    {
        currentPowerUp = null;
    }

    private class LevitateAndRotate : MonoBehaviour
    {
        public float rotationSpeed = 50f;
        public float floatAmplitude = 0.25f;
        public float floatFrequency = 2f;

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.position;
        }

        void Update()
        {
            // Rotación
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

            // Movimiento flotante
            float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private class PowerUpPickup : MonoBehaviour
    {
        private TimePowerUp manager;
        private float extraTime;
        private AudioClip sonidoPickup;
        private AudioSource audioSource;
        private bool collected = false;

        public void Init(TimePowerUp manager, float tiempo, AudioClip sonido, AudioSource source)
        {
            this.manager = manager;
            this.extraTime = tiempo;
            this.sonidoPickup = sonido;
            this.audioSource = source;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (collected) return; 

            if (other.CompareTag("Player"))
            {
                collected = true;

                MusicTimer timer = FindObjectOfType<MusicTimer>();
                if (timer != null)
                {
                    timer.AgregarTiempo(extraTime);
                }

                if (sonidoPickup != null && audioSource != null)
                {
                    audioSource.PlayOneShot(sonidoPickup);
                }

                manager.OnPowerUpCollected();

                StartCoroutine(DesaparecerAnimacion());
            }
        }

        private System.Collections.IEnumerator DesaparecerAnimacion()
        {
            float duration = 0.5f; 
            float elapsed = 0f;

            Vector3 originalScale = transform.localScale;
            Vector3 popScale = originalScale * 1.2f; 

            float popTime = 0.15f;
            while (elapsed < popTime)
            {
                float t = elapsed / popTime;
                transform.localScale = Vector3.Lerp(originalScale, popScale, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;
            Vector3 endScale = Vector3.zero;
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            Color startColor = sr != null ? sr.color : Color.white;

            while (elapsed < duration)
            {
                float t = elapsed / duration;

                transform.localScale = Vector3.Lerp(popScale, endScale, t);
                transform.Rotate(Vector3.up * 500f * Time.deltaTime);
                if (sr != null)
                {
                    sr.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), t);
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
