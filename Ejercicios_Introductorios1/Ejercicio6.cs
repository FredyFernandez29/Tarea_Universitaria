// leer un número entero de tres dígitos y determinar si algún dígito es múltiplo de los otros.

using System;

class Ejercicio6
{
    static void Main()
    {
        Console.WriteLine("ejercicio 6: digitos multiplos");
        
        Console.Write("ingrese un numero de tres digitos: ");
        int numeroIngresado = int.Parse(Console.ReadLine());
        
        bool tieneTresDigitos = (numeroIngresado >= 100 && numeroIngresado <= 999) || 
                                 (numeroIngresado >= -999 && numeroIngresado <= -100);
        
        if (tieneTresDigitos)
        {
            int valorAbsoluto = Math.Abs(numeroIngresado);
            int a = valorAbsoluto / 100;
            int b = (valorAbsoluto / 10) % 10;
            int c = valorAbsoluto % 10;
            
            Console.WriteLine($"digitos: {a}, {b}, {c}");
            
            bool encontrado = false;
            
            // funcion para verificar si x es multiplo de y
            // x es multiplo de y si y != 0 y x % y == 0
            bool EsMultiplo(int x, int y)
            {
                return y != 0 && x % y == 0;
            }
            
            // verificar si a es multiplo de b o de c
            if (EsMultiplo(a, b))
            {
                Console.WriteLine($"{a} es multiplo de {b}");
                encontrado = true;
            }
            if (EsMultiplo(a, c))
            {
                Console.WriteLine($"{a} es multiplo de {c}");
                encontrado = true;
            }
            
            // verificar si b es multiplo de a o de c
            if (EsMultiplo(b, a))
            {
                Console.WriteLine($"{b} es multiplo de {a}");
                encontrado = true;
            }
            if (EsMultiplo(b, c))
            {
                Console.WriteLine($"{b} es multiplo de {c}");
                encontrado = true;
            }
            
            // verificar si c es multiplo de a o de b
            if (EsMultiplo(c, a))
            {
                Console.WriteLine($"{c} es multiplo de {a}");
                encontrado = true;
            }
            if (EsMultiplo(c, b))
            {
                Console.WriteLine($"{c} es multiplo de {b}");
                encontrado = true;
            }
            
            if (!encontrado)
            {
                Console.WriteLine("ningun digito es multiplo de otro");
            }
        }
        else
        {
            Console.WriteLine("error: el numero no tiene tres digitos");
        }
    }
}