using UnityEngine;

public class TimePowerUp : MonoBehaviour
{
    [Header("Configuración del PowerUp")]
    public float tiempoExtra = 15f; // segundos que agrega

    [Header("Spawning")]
    public GameObject powerUpPrefab;       // prefab del power-up
    public Transform[] spawnPoints;        // lugares posibles en la casa
    public float spawnInterval = 30f;      // cada cuánto aparece uno

    private float timer;
    private GameObject currentPowerUp; // referencia al power-up activo

    void Update()
    {
        // control del spawn cada cierto tiempo
        timer += Time.deltaTime;

        // solo spawnea si no hay ninguno en la escena
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

        // añadir el trigger detector directamente al prefab instanciado
        Collider col = currentPowerUp.GetComponent<Collider>();
        if (col == null)
        {
            col = currentPowerUp.AddComponent<BoxCollider>();
            col.isTrigger = true;
        }

        // añadir componente dinámico para detectar recogida
        currentPowerUp.AddComponent<PowerUpPickup>().Init(this, tiempoExtra);
    }

    // llamado por el objeto al ser recogido
    public void OnPowerUpCollected()
    {
        currentPowerUp = null;
    }
}

// Script interno para manejar la recogida
public class PowerUpPickup : MonoBehaviour
{
    private TimePowerUp manager;
    private float extraTime;

    public void Init(TimePowerUp manager, float tiempo)
    {
        this.manager = manager;
        this.extraTime = tiempo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MusicTimer timer = FindObjectOfType<MusicTimer>();
            if (timer != null)
            {
                timer.AgregarTiempo(extraTime);
            }

            manager.OnPowerUpCollected();
            Destroy(gameObject);
        }
    }
}

