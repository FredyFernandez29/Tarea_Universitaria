// Leer 10 números enteros, almacenarlos en un arreglo y determinar a cuánto es igual el promedio entero de los datos del arreglo
using System;

class Ejercicio7
{
    static void Main()
    {
        int[] numeros = new int[10];
        int suma = 0;
        
        // Leer los 10 números y calcular la suma
        for (int i = 0; i < 10; i++)
        {
            Console.Write($"Ingrese el número {i + 1}: ");
            numeros[i] = int.Parse(Console.ReadLine());
            suma += numeros[i];
        }
        
        // Calcular promedio entero (división entera)
        int promedioEntero = suma / 10;
        
        Console.WriteLine($"\nLa suma de los números es: {suma}");
        Console.WriteLine($"El promedio entero (división entera) es: {promedioEntero}");
        
        // Mostrar los números ingresados
        Console.WriteLine("\nNúmeros ingresados:");
        for (int i = 0; i < 10; i++)
        {
            Console.Write(numeros[i] + " ");
        }
    }
}