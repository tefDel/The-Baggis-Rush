using UnityEngine;

public class HaloPulse : MonoBehaviour
{
    [Header("Configuración del Halo")]
    public Color pulseColor = Color.cyan; // color del brillo
    public float speed = 2f;              // velocidad del pulso
    public float intensity = 2f;          // intensidad máxima

    private Material mat;

    void Start()
    {
        // instanciamos el material para no alterar el original
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float emission = Mathf.PingPong(Time.time * speed, 1.0f) * intensity;
        Color finalColor = pulseColor * Mathf.LinearToGammaSpace(emission);
        mat.SetColor("_EmissionColor", finalColor);
    }
}
