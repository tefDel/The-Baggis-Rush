using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CheckListManager : MonoBehaviour
{
    [System.Serializable]
    public class ChecklistItem
    {
        public string itemName;
        public bool obtained;
        [HideInInspector] public TextMeshProUGUI uiText; // referencia al texto en la UI
        [HideInInspector] public Button button; // referencia al botón
    }

    [Header("UI References")]
    public GameObject panel;          // El panel que contiene la lista
    public GameObject itemPrefab;     // Prefab del botón del ítem (con TMP como hijo)
    public Transform listContainer;   // Donde se instanciarán los ítems
    public TextMeshProUGUI titleText; // Texto del título ("Lista")

    [Header("Datos")]
    public string listTitle = "Lista"; // Nombre que aparecerá arriba
    public List<ChecklistItem> items = new List<ChecklistItem>();

    void Start()
    {
        // Poner título
        if (titleText != null)
            titleText.text = listTitle;

        // Inicializa la lista en la UI
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(itemPrefab, listContainer);

            // Buscar el texto dentro del botón
            TextMeshProUGUI textComponent = newItem.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = item.itemName;
            item.uiText = textComponent;

            // Guardar referencia al botón
            Button btn = newItem.GetComponent<Button>();
            item.button = btn;

            // Capturar el nombre del item para el listener
            string capturedName = item.itemName;
            btn.onClick.AddListener(() => MarkAsObtained(capturedName));
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

            // Desactivar el botón para que no se vuelva a hacer clic
            if (item.button != null)
                item.button.interactable = false;
        }
    }
}
