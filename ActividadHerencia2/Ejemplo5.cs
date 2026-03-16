using System;

abstract class PaloGolf
{
    public abstract void Golpear();
}

class Driver : PaloGolf
{
    public override void Golpear()
    {
        Console.WriteLine("Driver: golpe fuerte contacto");
    }
}

class Hierro : PaloGolf
{
    public override void Golpear()
    {
        Console.WriteLine("Hierro: golpe medio contacto");
    }
}

class Putter : PaloGolf
{
    public override void Golpear()
    {
        Console.WriteLine("Putter: golpe débil contacto");
    }
}

class Program
{
    static void Main()
    {
        Driver driver = new Driver();
        driver.Golpear();

        Hierro hierro = new Hierro();
        hierro.Golpear();

        Putter putter = new Putter();
        putter.Golpear();
    }
}