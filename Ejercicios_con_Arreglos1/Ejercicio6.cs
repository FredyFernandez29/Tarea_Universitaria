// Leer 10 números enteros, almacenarlos en un arreglo y determinar en qué posiciones se encuentran los números con más de 3 dígitos

using System;

class Ejercicio6
{
    static void Main()
    {
        int[] numeros = new int[10];
        
        // Leer los 10 números
        for (int i = 0; i < 10; i++)
        {
            Console.Write($"Ingrese el número {i + 1}: ");
            numeros[i] = int.Parse(Console.ReadLine());
        }
        
        Console.WriteLine("\nPosiciones de números con más de 3 dígitos:");
        bool encontrado = false;
        
        for (int i = 0; i < 10; i++)
        {
            int cantidadDigitos = Math.Abs(numeros[i]).ToString().Length;
            
            if (cantidadDigitos > 3)
            {
                Console.WriteLine($"Número {numeros[i]} en posición {i + 1} tiene {cantidadDigitos} dígitos");
                encontrado = true;
            }
        }
        
        if (!encontrado)
            Console.WriteLine("No se encontraron números con más de 3 dígitos");
    }
}