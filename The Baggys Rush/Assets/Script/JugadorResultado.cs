using System;

[System.Serializable]
public class JugadorResultado
{
    public string Nombre;
    public int Edad;
    public string Email;
    public string Ciudad;
    public float Tiempo;

    public JugadorResultado(string nombre, int edad, string email, string ciudad, float tiempo)
    {
        Nombre = nombre;
        Edad = edad;
        Email = email;
        Ciudad = ciudad;
        Tiempo = tiempo;
    }
}
