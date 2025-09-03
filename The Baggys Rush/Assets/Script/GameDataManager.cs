using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public int totalJugadores = 0;
    public int jugadoresRegistrados = 0;
    public List<JugadorResultado> ranking = new List<JugadorResultado>();

    // Datos del jugador en curso
    public string nombreJugador = "";
    public int edadJugador = 0;
    public string emailJugador = "";
    public string ciudadJugador = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarResultado(string nombre, int edad, string email, string ciudad, float tiempo)
    {
        ranking.Add(new JugadorResultado(nombre, edad, email, ciudad, tiempo));
        ranking.Sort((a, b) => a.Tiempo.CompareTo(b.Tiempo)); // menor tiempo primero
        jugadoresRegistrados++;
    }

    public void ReiniciarDatos()
    {
        totalJugadores = 0;
        jugadoresRegistrados = 0;
        ranking.Clear();
        nombreJugador = "";
        edadJugador = 0;
        emailJugador = "";
        ciudadJugador = "";
    }
}
