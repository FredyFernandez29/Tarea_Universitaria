// leer dos números enteros de dos dígitos y determinar si la suma de los dos números origina un número par.

using System;

class Ejercicio4
{
    static void Main()
    {
        Console.WriteLine("ejercicio 4: suma par");
        
        Console.Write("ingrese primer numero de dos digitos: ");
        int primerNumero = int.Parse(Console.ReadLine());
        
        Console.Write("ingrese segundo numero de dos digitos: ");
        int segundoNumero = int.Parse(Console.ReadLine());
        
        bool primeroValido = (primerNumero >= 10 && primerNumero <= 99) || 
                             (primerNumero >= -99 && primerNumero <= -10);
        bool segundoValido = (segundoNumero >= 10 && segundoNumero <= 99) || 
                             (segundoNumero >= -99 && segundoNumero <= -10);
        
        if (primeroValido && segundoValido)
        {
            int suma = primerNumero + segundoNumero;
            bool esPar = (suma % 2 == 0);
            
            Console.WriteLine($"{primerNumero} + {segundoNumero} = {suma}");
            Console.WriteLine($"la suma es par? {esPar}");
        }
        else
        {
            Console.WriteLine("error: uno o ambos numeros no tienen dos digitos");
        }
    }
}