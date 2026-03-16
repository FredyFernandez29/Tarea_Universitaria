using System;

class CuentaBancaria
{
    public string Titular { get; set; }
    public double Saldo { get; set; }

    public void Depositar(double monto)
    {
        Saldo += monto;
        Console.WriteLine("Depósito exitoso. Nuevo saldo: " + Saldo);
    }

    public virtual void Retirar(double monto)
    {
        if (monto <= Saldo)
        {
            Saldo -= monto;
            Console.WriteLine("Retiro exitoso. Nuevo saldo: " + Saldo);
        }
        else
        {
            Console.WriteLine("Saldo insuficiente.");
        }
    }
}

class CuentaAhorros : CuentaBancaria
{
    public double TasaInteres { get; set; }

    public void CalcularInteres()
    {
        double interes = Saldo * TasaInteres / 100;
        Saldo += interes;
        Console.WriteLine("Interés calculado: " + interes + ". Nuevo saldo: " + Saldo);
    }
}

class CuentaCorriente : CuentaBancaria
{
    public double Sobregiro { get; set; }

    public override void Retirar(double monto)
    {
        if (monto <= Saldo + Sobregiro)
        {
            Saldo -= monto;
            Console.WriteLine("Retiro exitoso con sobregiro. Nuevo saldo: " + Saldo);
        }
        else
        {
            Console.WriteLine("Fondos insuficientes incluso con sobregiro.");
        }
    }
}

class Program
{
    static void Main()
    {
        CuentaAhorros ahorros = new CuentaAhorros();
        ahorros.Titular = "Ana";
        ahorros.Saldo = 1000;
        ahorros.TasaInteres = 5;
        ahorros.Depositar(200);
        ahorros.CalcularInteres();

        CuentaCorriente corriente = new CuentaCorriente();
        corriente.Titular = "Luis";
        corriente.Saldo = 500;
        corriente.Sobregiro = 200;
        corriente.Retirar(600);
    }
}