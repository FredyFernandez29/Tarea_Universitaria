// leer un número entero de dos dígitos y determinar si es primo y además si es negativo.

using System;

class Ejercicio2
{
    static void Main()
    {
        Console.WriteLine("ejercicio 2: primo y negativo");
        
        Console.Write("ingrese un numero entero de dos digitos: ");
        int numeroIngresado = int.Parse(Console.ReadLine());
        
        bool tieneDosDigitos = (numeroIngresado >= 10 && numeroIngresado <= 99) || 
                                (numeroIngresado >= -99 && numeroIngresado <= -10);
        
        if (tieneDosDigitos)
        {
            bool esNegativo = numeroIngresado < 0;
            int valorAbsoluto = Math.Abs(numeroIngresado);
            bool esPrimo = true;
            
            if (valorAbsoluto < 2)
            {
                esPrimo = false;
            }
            else
            {
                for (int divisor = 2; divisor <= valorAbsoluto / 2; divisor++)
                {
                    if (valorAbsoluto % divisor == 0)
                    {
                        esPrimo = false;
                        break;
                    }
                }
            }
            
            Console.WriteLine($"numero: {numeroIngresado}");
            Console.WriteLine($"es negativo? {esNegativo}");
            Console.WriteLine($"es primo? {esPrimo}");
        }
        else
        {
            Console.WriteLine("error: el numero no tiene dos digitos");
        }
    }
}