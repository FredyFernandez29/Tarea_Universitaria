//  Leer 10 enteros, almacenarlos en un arreglo y determinar en qué posición del arreglo está el mayor número leído.

using System;

class Ejercicio1
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
        
        // Encontrar el mayor número y su posición
        int mayor = numeros[0];
        int posicionMayor = 0;
        
        for (int i = 1; i < 10; i++)
        {
            if (numeros[i] > mayor)
            {
                mayor = numeros[i];
                posicionMayor = i;
            }
        }
        
        Console.WriteLine($"\nEl mayor número es {mayor} y está en la posición {posicionMayor + 1}");
    }
}