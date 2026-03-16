using System;
using System.Collections.Generic;

class Ave
{
    public string ColorPlumas { get; set; }
    public double AnchuraAlas { get; set; }
    public double VelocidadMaxima { get; set; }

    public Ave(string colorPlumas, double anchuraAlas, double velocidadMaxima)
    {
        ColorPlumas = colorPlumas;
        AnchuraAlas = anchuraAlas;
        VelocidadMaxima = velocidadMaxima;
    }

    public virtual void Volar()
    {
        Console.WriteLine("Esta ave vuela de manera generica.");
    }

    public virtual void MostrarInformacion()
    {
        Console.WriteLine($"Color: {ColorPlumas}, Alas: {AnchuraAlas} m, Velocidad maxima: {VelocidadMaxima} km/h");
    }
}

class Aguila : Ave
{
    public Aguila(string colorPlumas, double anchuraAlas, double velocidadMaxima) : base(colorPlumas, anchuraAlas, velocidadMaxima) { }

    public override void Volar()
    {
        Console.WriteLine("El agula vuela alto y rapido.");
    }
}

class Pinguino : Ave
{
    public Pinguino(string colorPlumas, double anchuraAlas, double velocidadMaxima) : base(colorPlumas, anchuraAlas, velocidadMaxima) { }

    public override void Volar()
    {
        Console.WriteLine("El pinguino no vuela, nada.");
    }

    public override void MostrarInformacion()
    {
        Console.WriteLine($"Pinguino - Color: {ColorPlumas}, Alas: {AnchuraAlas} m (no vuela), Velocidad en agua: {VelocidadMaxima} km/h");
    }
}

class Colibri : Ave
{
    public Colibri(string colorPlumas, double anchuraAlas, double velocidadMaxima) : base(colorPlumas, anchuraAlas, velocidadMaxima) { }

    public override void Volar()
    {
        Console.WriteLine("El colibri vuela suspendido en el aire.");
    }
}

class Program
{
    static void Main()
    {
        List<Ave> aves = new List<Ave>
        {
            new Aguila("Marron y blanco", 2.0, 160),
            new Pinguino("Negro y blanco", 0.6, 30),
            new Colibri("Verde y rojo", 0.1, 50)
        };

        foreach (Ave ave in aves)
        {
            ave.MostrarInformacion();
            ave.Volar();
            Console.WriteLine();
        }
    }
}