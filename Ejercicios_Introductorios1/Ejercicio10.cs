// leer dos números enteros y si la diferencia entre los dos es menor o igual a 10 
// entonces mostrar todos los enteros entre el menor y el mayor.

using System;

class Ejercicio10
{
    static void Main()
    {
        Console.WriteLine("ejercicio 10: numeros entre dos valores");
        
        Console.Write("primer numero: ");
        int a = int.Parse(Console.ReadLine());
        
        Console.Write("segundo numero: ");
        int b = int.Parse(Console.ReadLine());
        
        int diferencia = Math.Abs(a - b);
        Console.WriteLine($"diferencia: {diferencia}");
        
        if (diferencia <= 10)
        {
            int menor = Math.Min(a, b);
            int mayor = Math.Max(a, b);
            
            for (int i = menor; i <= mayor; i++)
            {
                Console.Write(i);
                if (i < mayor) Console.Write(" ");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("la diferencia es mayor a 10, no se muestran numeros");
        }
    }
}