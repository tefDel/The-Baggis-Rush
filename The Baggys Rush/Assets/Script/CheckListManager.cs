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
        public GameObject ObjetoColeccionable; // Objeto en la escena (coleccionable)
        [HideInInspector] public TextMeshProUGUI uiText; // referencia al texto en la UI
    }

    [Header("UI References")]
    public GameObject panel;          // El panel que contiene la lista
    public GameObject itemPrefab;     // Prefab de un texto (NO botón)
    public Transform listContainer;   // Donde se instanciarán los ítems
    public TextMeshProUGUI titleText; // Texto del título ("Lista")

    [Header("Datos")]
    public string listTitle = "Lista"; // Nombre que aparecerá arriba
    public List<ChecklistItem> items = new List<ChecklistItem>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Poner título
        if (titleText != null)
            titleText.text = listTitle;

        // Inicializa la lista en la UI
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(itemPrefab, listContainer);

            // Buscar el texto dentro del prefab
            TextMeshProUGUI textComponent = newItem.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = item.itemName;
            item.uiText = textComponent;

            // Sin botón, solo texto

            // Vincular con objeto en la escena
            if (item.ObjetoColeccionable != null)
            {
                Coleccionable col = item.ObjetoColeccionable.GetComponent<Coleccionable>();
                if (col != null)
                    col.itemName = item.itemName; // Forzamos que coincidan
            }
        }

        panel.SetActive(false); // empieza oculto
    }

    void Update()
    {
        // Ejemplo: tecla Tab para mostrar/ocultar el panel
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            panel.SetActive(!panel.activeSelf);

            // Activar/desactivar cursor
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
            item.uiText.text = $"<s>{item.itemName}</s>"; // tachado
            item.uiText.color = Color.gray;              // opcional: gris
        }
    }
}
