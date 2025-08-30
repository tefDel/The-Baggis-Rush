using UnityEngine;

public class Linea : MonoBehaviour
{
    private LineRenderer line;
    private Transform target;
    public Transform origen; // jugador/cámara

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false; // Inicia deshabilitado hasta tener target
    }

    void Update()
    {
        if (target != null)
        {
            line.enabled = true;
            line.SetPosition(0, origen.position);
            line.SetPosition(1, target.position);
        }
        else
        {
            line.enabled = false; // Ocultar si no hay target
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}