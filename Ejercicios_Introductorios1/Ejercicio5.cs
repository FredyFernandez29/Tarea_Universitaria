// leer un número entero de tres dígitos y determinar en qué posición está el mayor dígito.

using System;

class Ejercicio5
{
    static void Main()
    {
        Console.WriteLine("ejercicio 5: posicion del digito mayor");
        
        Console.Write("ingrese un numero de tres digitos: ");
        int numeroIngresado = int.Parse(Console.ReadLine());
        
        bool tieneTresDigitos = (numeroIngresado >= 100 && numeroIngresado <= 999) || 
                                 (numeroIngresado >= -999 && numeroIngresado <= -100);
        
        if (tieneTresDigitos)
        {
            int valorAbsoluto = Math.Abs(numeroIngresado);
            int centena = valorAbsoluto / 100;
            int decena = (valorAbsoluto / 10) % 10;
            int unidad = valorAbsoluto % 10;
            
            Console.WriteLine($"numero: {numeroIngresado}");
            Console.WriteLine($"digitos: centena={centena}, decena={decena}, unidad={unidad}");
            
            int digitoMayor = centena;
            int posicion = 1;
            
            if (decena > digitoMayor)
            {
                digitoMayor = decena;
                posicion = 2;
            }
            
            if (unidad > digitoMayor)
            {
                digitoMayor = unidad;
                posicion = 3;
            }
            
            string[] nombres = { "centena", "decena", "unidad" };
            Console.WriteLine($"el digito mayor es {digitoMayor} y esta en la {nombres[posicion-1]}");
        }
        else
        {
            Console.WriteLine("error: el numero no tiene tres digitos");
        }
    }
}