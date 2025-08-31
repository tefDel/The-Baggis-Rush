using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CheckListManager : MonoBehaviour
{
    public static CheckListManager Instance;

    [System.Serializable]
    public class ChecklistItem
    {
        public string itemName;
        public bool obtained;
        public GameObject ObjetoColeccionable;
        [HideInInspector] public TextMeshProUGUI uiText;
    }

    [Header("UI References")]
    public GameObject panel;
    public GameObject itemPrefab;
    public Transform listContainer;
    public TextMeshProUGUI titleText;
    public Linea lineScript; // Asignar en Inspector

    [Header("Datos")]
    public string listTitle = "Lista";
    public List<ChecklistItem> items = new List<ChecklistItem>();

    [Header("Indicador Tecla")]
    public GameObject tabImagen; 

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Configurar UI
        if (titleText != null)
            titleText.text = listTitle;

        // Crear items en la UI
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(itemPrefab, listContainer);
            TextMeshProUGUI textComponent = newItem.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = item.itemName;
            item.uiText = textComponent;

            // Vincular con objeto coleccionable
            if (item.ObjetoColeccionable != null)
            {
                Coleccionable col = item.ObjetoColeccionable.GetComponent<Coleccionable>();
                if (col != null)
                    col.itemName = item.itemName;
            }
        }

        panel.SetActive(false);

        // Mostrar imagen de Tab al inicio
        if (tabImagen != null)
            tabImagen.SetActive(true);

        // CLAVE: Inicializar línea con primer objetivo
        if (lineScript != null)
        {
            Transform primerObjetivo = GetNextTarget();
            lineScript.SetTarget(primerObjetivo);
        }
    }

    void Update()
    {
        // Toggle panel con Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool nuevoEstado = !panel.activeSelf;
            panel.SetActive(nuevoEstado);

            // Mostrar u ocultar cursor
            if (nuevoEstado)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Ocultar imagen de Tab cuando panel está abierto
                if (tabImagen != null)
                    tabImagen.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Volver a mostrar imagen de Tab cuando panel está cerrado
                if (tabImagen != null)
                    tabImagen.SetActive(true);
            }
        }
    }

    public void MarkAsObtained(string itemName)
    {
        ChecklistItem item = items.Find(x => x.itemName == itemName);
        if (item != null && !item.obtained)
        {
            item.obtained = true;
            item.uiText.text = $"<s>{item.itemName}</s>"; // Tachar
            item.uiText.color = Color.gray;
        }

        // Actualizar línea al siguiente objetivo
        Transform siguiente = GetNextTarget();
        if (lineScript != null)
            lineScript.SetTarget(siguiente);
    }

    public Transform GetNextTarget()
    {
        // Buscar primer item no obtenido
        foreach (var item in items)
        {
            if (!item.obtained && item.ObjetoColeccionable != null)
            {
                return item.ObjetoColeccionable.transform;
            }
        }
        return null; // Todos completados
    }
}
