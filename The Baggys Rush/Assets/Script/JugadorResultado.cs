using System;
using UnityEngine;

public class JugadorResultado
{
    public static JugadorResultado Instance;

    public string nombre;
    public float tiempo;

    public JugadorResultado()
    {
    }

    public JugadorResultado(string nombre, float tiempo)
    {
        this.nombre = nombre;
        this.tiempo = tiempo;
    }

   
}
