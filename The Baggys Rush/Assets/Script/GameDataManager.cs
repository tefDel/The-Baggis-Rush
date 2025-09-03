using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    // .JSON AUTOMÁTICO 
    public void GuardarResultado(string nombre, int edad, string email, string ciudad, float tiempo)
    {
        ranking.Add(new JugadorResultado(nombre, edad, email, ciudad, tiempo));
        ranking.Sort((a, b) => a.Tiempo.CompareTo(b.Tiempo)); // menor tiempo primero
        jugadoresRegistrados++;

    }

    public void GuardarRankingEnJson()
    {
        Ranking wrapper = new Ranking { ranking = ranking };
        string json = JsonUtility.ToJson(wrapper, true);

        string path = Path.Combine(Application.persistentDataPath, "ranking.json");
        File.WriteAllText(path, json);

        Debug.Log("Ranking guardado en: " + path);
    }


    public void CargarRankingDesdeJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "ranking.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Ranking datos = JsonUtility.FromJson<Ranking>(json);

            if (datos != null && datos.ranking != null)
            {
                ranking = datos.ranking;
                Debug.Log("Backup cargado manualmente y aplicado al ranking.");
            }
        }
        else
        {
            Debug.LogWarning(" No se encontró backup en: " + path);
        }
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
        GuardarRankingEnJson();
    }
}
