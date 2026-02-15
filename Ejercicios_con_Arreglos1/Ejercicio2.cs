// Leer 10 enteros, almacenarlos en un arreglo y determinar en qué posición de del arreglo está el mayor número par leído.

using System;

class Ejercicio2
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
        
        int mayorPar = int.MinValue;
        int posicionMayorPar = -1;
        
        // Encontrar el mayor número par
        for (int i = 0; i < 10; i++)
        {
            if (numeros[i] % 2 == 0 && numeros[i] > mayorPar)
            {
                mayorPar = numeros[i];
                posicionMayorPar = i;
            }
        }
        
        if (posicionMayorPar != -1)
            Console.WriteLine($"\nEl mayor número par es {mayorPar} y está en la posición {posicionMayorPar + 1}");
        else
            Console.WriteLine("\nNo se encontraron números pares");
    }
}