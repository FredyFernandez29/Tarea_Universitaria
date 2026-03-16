using System;

class Vehiculo
{
    public string Marca { get; set; }

    public void Arrancar()
    {
        Console.WriteLine("El vehículo arranca");
    }
}

class Automovil : Vehiculo
{
    public int NumeroPuertas { get; set; }

    public void TocarBocina()
    {
        Console.WriteLine("Pii pii!");
    }
}

class Deportivo : Automovil
{
    public bool TieneTurbo { get; set; }

    public void ActivarTurbo()
    {
        if (TieneTurbo)
            Console.WriteLine("Turbo activado");
        else
            Console.WriteLine("No tiene turbo");
    }
}

class Program
{
    static void Main()
    {
        Deportivo miAuto = new Deportivo();
        miAuto.Marca = "Ferrari";
        miAuto.NumeroPuertas = 2;
        miAuto.TieneTurbo = true;
        miAuto.Arrancar();
        miAuto.TocarBocina();
        miAuto.ActivarTurbo();
    }
}