// Leer 10 enteros, almacenarlos en un arreglo y determinar en qué posición del arreglo está el mayor número primo leído.

using System;

class Ejercicio3
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
        
        int mayorPrimo = int.MinValue;
        int posicionMayorPrimo = -1;
        
        // Encontrar el mayor número primo
        for (int i = 0; i < 10; i++)
        {
            if (EsPrimo(numeros[i]) && numeros[i] > mayorPrimo)
            {
                mayorPrimo = numeros[i];
                posicionMayorPrimo = i;
            }
        }
        
        if (posicionMayorPrimo != -1)
            Console.WriteLine($"\nEl mayor número primo es {mayorPrimo} y está en la posición {posicionMayorPrimo + 1}");
        else
            Console.WriteLine("\nNo se encontraron números primos");
    }
    
    static bool EsPrimo(int numero)
    {
        if (numero < 2)
            return false;
        
        for (int divisor = 2; divisor <= Math.Sqrt(numero); divisor++)
        {
            if (numero % divisor == 0)
                return false;
        }
        return true;
    }
}