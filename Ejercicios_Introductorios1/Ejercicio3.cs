// leer un número entero de dos dígitos y determinar si sus dos dígitos son primos.

using System;

class Ejercicio3
{
    static void Main()
    {
        Console.WriteLine("ejercicio 3: digitos primos");
        
        Console.Write("ingrese un numero entero de dos digitos: ");
        int numeroIngresado = int.Parse(Console.ReadLine());
        
        bool tieneDosDigitos = (numeroIngresado >= 10 && numeroIngresado <= 99) || 
                                (numeroIngresado >= -99 && numeroIngresado <= -10);
        
        if (tieneDosDigitos)
        {
            int valorAbsoluto = Math.Abs(numeroIngresado);
            int primerDigito = valorAbsoluto / 10;
            int segundoDigito = valorAbsoluto % 10;
            
            bool primeroEsPrimo = EsPrimo(primerDigito);
            bool segundoEsPrimo = EsPrimo(segundoDigito);
            
            Console.WriteLine($"numero: {numeroIngresado}");
            Console.WriteLine($"primer digito ({primerDigito}) es primo? {primeroEsPrimo}");
            Console.WriteLine($"segundo digito ({segundoDigito}) es primo? {segundoEsPrimo}");
            
            if (primeroEsPrimo && segundoEsPrimo)
                Console.WriteLine("ambos son primos");
            else if (primeroEsPrimo || segundoEsPrimo)
                Console.WriteLine("solo uno es primo");
            else
                Console.WriteLine("ninguno es primo");
        }
        else
        {
            Console.WriteLine("error: el numero no tiene dos digitos");
        }
    }
    
    static bool EsPrimo(int numero)
    {
        if (numero < 2) return false;
        for (int divisor = 2; divisor <= Math.Sqrt(numero); divisor++)
        {
            if (numero % divisor == 0) return false;
        }
        return true;
    }
}