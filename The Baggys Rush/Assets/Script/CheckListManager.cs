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
            panel.SetActive(!panel.activeSelf);

            if (panel.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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