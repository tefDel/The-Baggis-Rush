using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Panels (Menú Principal)")]
    public GameObject panelNombreJugador;
    public GameObject panelJuego;
    public GameObject panelFinPartida;
    public GameObject panelControles;

    [Header("UI Elements")]
    public TMP_InputField inputNombre;
    public TextMeshProUGUI textoTimer;
    public TextMeshProUGUI textoResultado;

    [Header("Botones")]
    public Button botonIniciar;
    public Button botonConfirmarNombre;
    public Button botonControles;
    public Button botonCerrarControles;
    public Button botonReintentar;

    private float tiempo;
    private bool jugando;

    // Datos globales (estáticos, persisten entre escenas)
    public static string nombreJugador;
    public static float mejorTiempo;

    void Start()
    {
        // Solo si estamos en el menú principal
        if (SceneManager.GetActiveScene().name != "SampleScene")
        {
            panelNombreJugador.SetActive(false);
            panelJuego.SetActive(false);
            panelFinPartida.SetActive(false);
            panelControles.SetActive(false);

            botonIniciar.onClick.AddListener(MostrarPanelNombre);
            botonConfirmarNombre.onClick.AddListener(ConfirmarNombreYCargarJuego);
            botonControles.onClick.AddListener(() => panelControles.SetActive(true));
            botonCerrarControles.onClick.AddListener(() => panelControles.SetActive(false));
        }
        else
        {
            // Estamos en SampleScene → iniciar juego
            panelJuego.SetActive(true);
            panelFinPartida.SetActive(false);
            jugando = true;
            tiempo = 0f;
        }

        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(Reiniciar);
    }

    void Update()
    {
        if (jugando && SceneManager.GetActiveScene().name == "SampleScene")
        {
            tiempo += Time.deltaTime;
            textoTimer.text = "Tiempo: " + tiempo.ToString("F2") + " s";
        }
    }

    public void MostrarPanelNombre()
    {
        panelNombreJugador.SetActive(true);
    }

    public void ConfirmarNombreYCargarJuego()
    {
        nombreJugador = inputNombre.text;
        if (string.IsNullOrEmpty(nombreJugador))
            nombreJugador = "Jugador";

        SceneManager.LoadScene("SampleScene");
    }

    // Llamar esta función desde SampleScene cuando el jugador termine
    public void TerminarPartida()
    {
        jugando = false;
        panelJuego.SetActive(false);
        panelFinPartida.SetActive(true);

        mejorTiempo = tiempo;
        textoResultado.text = nombreJugador + " - Tiempo récord: " + mejorTiempo.ToString("F2") + " s";
    }

    public void Reiniciar()
    {
        // Vuelve al menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }
}
