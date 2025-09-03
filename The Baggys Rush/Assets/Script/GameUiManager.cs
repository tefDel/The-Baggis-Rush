using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

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
    public TMP_InputField inputEdad;
    public TMP_InputField inputEmail;
    public TMP_InputField inputCiudad;
    public TextMeshProUGUI textoPodio;
    public TextMeshProUGUI textoJugadorActual;
    public TextMeshProUGUI textoError; 

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
        inputEdad.text = "";
        inputEmail.text = "";
        inputCiudad.text = "";
        if (textoError != null) textoError.text = "";
    }

    public void ConfirmarNombreYCargarJuego()
    {
        // Validar campos
        if (string.IsNullOrWhiteSpace(inputNombre.text) ||
            !int.TryParse(inputEdad.text, out int edadTemp) || edadTemp <= 0 ||
            string.IsNullOrWhiteSpace(inputCiudad.text) ||
            !EsEmailValido(inputEmail.text))
        {
            if (textoError != null)
                textoError.text = "* Por favor completa los campos correctamente.";
            return; 
        }

        data.nombreJugador = inputNombre.text.Trim();
        data.edadJugador = edadTemp;
        data.emailJugador = inputEmail.text.Trim();
        data.ciudadJugador = inputCiudad.text.Trim();

        SceneManager.LoadScene("SampleScene");
    }

    private bool EsEmailValido(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        // Regex simple para validar email
        string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, patron);
    }

    public void MostrarPodio()
    {
        OcultarTodosLosPaneles();
        panelPodio.SetActive(true);

        textoPodio.text = "🏆 PODIO FINAL 🏆\n\n";
        for (int i = 0; i < data.ranking.Count; i++)
        {
            string medalla = i == 0 ? "🥇" : i == 1 ? "🥈" : i == 2 ? "🥉" : $"{i + 1}.";
            var jugador = data.ranking[i];

            textoPodio.text += $"{medalla} {jugador.Nombre} | Edad: {jugador.Edad} | " +
                               $"{jugador.Ciudad} | {jugador.Email} | Tiempo {jugador.Tiempo:F2}s\n";
        }
    }

    public void ReiniciarJuego()
    {
        data.ReiniciarDatos();
        MostrarPanelInicio();
    }
    public void GuardarBackupManual()
    {
        Debug.Log("MÉTODO GuardarBackupManual() fue llamado");
        GameDataManager.Instance.GuardarRankingEnJson();
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
