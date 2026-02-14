// leer un número entero de cinco dígitos y determinar si es un número capicúa. ej: 15651, 59895.

using System;

class Ejercicio8
{
    static void Main()
    {
        Console.WriteLine("ejercicio 8: numero capicua");
        
        Console.Write("ingrese un numero de cinco digitos: ");
        int numeroIngresado = int.Parse(Console.ReadLine());
        
        bool tieneCincoDigitos = (numeroIngresado >= 10000 && numeroIngresado <= 99999) || 
                                  (numeroIngresado >= -99999 && numeroIngresado <= -10000);
        
        if (tieneCincoDigitos)
        {
            int valorAbsoluto = Math.Abs(numeroIngresado);
            int d1 = valorAbsoluto / 10000;
            int d2 = (valorAbsoluto / 1000) % 10;
            int d3 = (valorAbsoluto / 100) % 10;
            int d4 = (valorAbsoluto / 10) % 10;
            int d5 = valorAbsoluto % 10;
            
            bool esCapicua = (d1 == d5) && (d2 == d4);
            
            Console.WriteLine($"numero: {numeroIngresado}");
            Console.WriteLine(esCapicua ? "es capicua" : "no es capicua");
        }
        else
        {
            Console.WriteLine("error: el numero no tiene cinco digitos");
        }
    }
}