// leer un número entero de cuatro dígitos y determinar si el segundo dígito es igual al penúltimo.

using System;

class Ejercicio9
{
    static void Main()
    {
        Console.WriteLine("ejercicio 9: segundo digito vs penultimo");
        
        Console.Write("ingrese un numero de cuatro digitos: ");
        int numeroIngresado = int.Parse(Console.ReadLine());
        
        bool tieneCuatroDigitos = (numeroIngresado >= 1000 && numeroIngresado <= 9999) || 
                                   (numeroIngresado >= -9999 && numeroIngresado <= -1000);
        
        if (tieneCuatroDigitos)
        {
            int valorAbsoluto = Math.Abs(numeroIngresado);
            int d1 = valorAbsoluto / 1000;
            int d2 = (valorAbsoluto / 100) % 10;
            int d3 = (valorAbsoluto / 10) % 10;
            int d4 = valorAbsoluto % 10;
            
            Console.WriteLine($"digitos: {d1} {d2} {d3} {d4}");
            Console.WriteLine($"segundo: {d2}, penultimo: {d3}");
            Console.WriteLine(d2 == d3 ? "son iguales" : "son diferentes");
        }
        else
        {
            Console.WriteLine("error: el numero no tiene cuatro digitos");
        }
    }
}