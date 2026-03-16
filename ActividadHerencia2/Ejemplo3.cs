using System;

class Animal
{
    public string Nombre { get; set; }

    public void Dormir()
    {
        Console.WriteLine(Nombre + " está durmiendo");
    }

    public virtual void HacerSonido()
    {
        Console.WriteLine("Este animal hace un sonido");
    }
}

class Perro : Animal
{
    public override void HacerSonido()
    {
        Console.WriteLine(Nombre + " dice guau");
    }

    public void MoverCola()
    {
        Console.WriteLine(Nombre + " mueve la cola");
    }
}

class Gato : Animal
{
    public override void HacerSonido()
    {
        Console.WriteLine(Nombre + " dice miau");
    }

    public void Ronronear()
    {
        Console.WriteLine(Nombre + " está ronroneando");
    }
}

class Program
{
    static void Main()
    {
        Perro perro = new Perro();
        perro.Nombre = "Rex";
        perro.Dormir();
        perro.HacerSonido();
        perro.MoverCola();

        Gato gato = new Gato();
        gato.Nombre = "Misi";
        gato.Dormir();
        gato.HacerSonido();
        gato.Ronronear();
    }
}