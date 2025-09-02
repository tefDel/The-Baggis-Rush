using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelInicio;
    public GameObject panelNumeroJugadores;
    public GameObject panelNombreJugador;
    public GameObject panelControles;
    public GameObject panelPodio;

    [Header("UI Elements")]
    public TMP_InputField inputNumeroJugadores;
    public TMP_InputField inputNombre;
    public TextMeshProUGUI textoPodio;
    public TextMeshProUGUI textoJugadorActual;

    private GameDataManager data;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        data = GameDataManager.Instance;
        if (data.totalJugadores == 0)
        {
            MostrarPanelInicio();
        }
        else if (data.jugadoresRegistrados >= data.totalJugadores)
        {
            MostrarPodio();
        }
        else
        {
            MostrarPanelNombre();
        }
    }

    public void BotonIniciarJuego()
    {
        panelInicio.SetActive(false);
        panelNumeroJugadores.SetActive(true);
        inputNumeroJugadores.text = "";
    }

    public void ConfirmarNumeroJugadores()
    {
        if (int.TryParse(inputNumeroJugadores.text, out int numero) && numero >= 2 && numero <= 10)
        {
            data.totalJugadores = numero;
            data.jugadoresRegistrados = 0;
            data.ranking.Clear();

            MostrarPanelNombre();
        }
    }

    public void MostrarPanelInicio()
    {
        OcultarTodosLosPaneles();
        panelInicio.SetActive(true);
    }

    public void MostrarPanelNombre()
    {
        OcultarTodosLosPaneles();
        panelNombreJugador.SetActive(true);

        int jugadorActual = data.jugadoresRegistrados + 1;
        textoJugadorActual.text = $"Jugador {jugadorActual} de {data.totalJugadores}";
        inputNombre.text = "";
    }

    public void ConfirmarNombreYCargarJuego()
    {
        data.nombreJugador = string.IsNullOrEmpty(inputNombre.text.Trim())
            ? "Jugador " + (data.jugadoresRegistrados + 1)
            : inputNombre.text.Trim();

        SceneManager.LoadScene("SampleScene"); // 👉 Normal, no aditivo
    }

    public void MostrarPodio()
    {
        OcultarTodosLosPaneles();
        panelPodio.SetActive(true);

        textoPodio.text = "PODIO FINAL \n\n";
        for (int i = 0; i < data.ranking.Count; i++)
        {
            string medalla = i == 0 ? "" : i == 1 ? "" : i == 2 ? "" : $"{i + 1}.";
            textoPodio.text += $"{medalla} {data.ranking[i].nombre} - {data.ranking[i].tiempo:F2}s\n";
        }
    }

    public void ReiniciarJuego()
    {
        data.ReiniciarDatos();
        MostrarPanelInicio();
    }

    private void OcultarTodosLosPaneles()
    {
        panelInicio.SetActive(false);
        panelNumeroJugadores.SetActive(false);
        panelNombreJugador.SetActive(false);
        panelControles.SetActive(false);
        panelPodio.SetActive(false);
    }
    public void MostrarControles()
    {
        if (panelControles != null) panelControles.SetActive(true);
    }

    public void CerrarControles()
    {
        if (panelControles != null) panelControles.SetActive(false);
    }
}
