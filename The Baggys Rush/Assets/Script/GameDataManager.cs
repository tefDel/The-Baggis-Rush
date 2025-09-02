using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public int totalJugadores = 0;
    public int jugadoresRegistrados = 0;
    public List<JugadorResultado> ranking = new List<JugadorResultado>();
    public string nombreJugador = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 👉 No se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarResultado(string nombre, float tiempo)
    {
        ranking.Add(new JugadorResultado(nombre, tiempo));
        ranking.Sort((a, b) => a.tiempo.CompareTo(b.tiempo));
        jugadoresRegistrados++;
    }

    public void ReiniciarDatos()
    {
        totalJugadores = 0;
        jugadoresRegistrados = 0;
        ranking.Clear();
        nombreJugador = "";
    }
}
