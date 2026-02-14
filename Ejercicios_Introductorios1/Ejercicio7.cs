// leer tres números enteros y determinar cuál es el mayor. usar solamente dos variables.

using System;

class Ejercicio7
{
    static void Main()
    {
        Console.WriteLine("ejercicio 7: encontrar el mayor (solo 2 variables)");
        
        int numero;
        int mayor = int.MinValue;
        
        Console.Write("primer numero: ");
        numero = int.Parse(Console.ReadLine());
        if (numero > mayor) mayor = numero;
        
        Console.Write("segundo numero: ");
        numero = int.Parse(Console.ReadLine());
        if (numero > mayor) mayor = numero;
        
        Console.Write("tercer numero: ");
        numero = int.Parse(Console.ReadLine());
        if (numero > mayor) mayor = numero;
        
        Console.WriteLine($"el mayor es: {mayor}");
    }
}