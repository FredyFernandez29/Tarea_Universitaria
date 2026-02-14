// Leer un número entero de dos dígitos y determinar a cuánto es igual la suma de sus dígitos.


using System;

class Ejercicio1
{
    static void Main()
    {
        Console.WriteLine("=== ejercicio 1: suma de digitos ===");
        
        // solicito el número al usuario
        Console.Write("ingrese un numero entero de dos digitos: ");
        int numeroIngresado = int.Parse(Console.ReadLine());
        
        // verifico si el número tiene exactamente dos dígitos
        bool tieneDosDigitos = (numeroIngresado >= 10 && numeroIngresado <= 99) || 
                                (numeroIngresado >= -99 && numeroIngresado <= -10);
        
        if (tieneDosDigitos)
        {
            // uso Math.Abs para trabajar con el valor absoluto e ignorar el signo
            int valorAbsoluto = Math.Abs(numeroIngresado);
            
            // obtengo los dígitos individualmente
            int digitoDecena = valorAbsoluto / 10;
            int digitoUnidad = valorAbsoluto % 10;
            
            // calculo la suma
            int sumaTotal = digitoDecena + digitoUnidad;
            
            // muestro los resultados
            Console.WriteLine("\n--- resultado ---");
            Console.WriteLine($"número ingresado: {numeroIngresado}");
            Console.WriteLine($"dígito de las decenas: {digitoDecena}");
            Console.WriteLine($"dígito de las unidades: {digitoUnidad}");
            Console.WriteLine($"la suma de los digitos es: {sumaTotal}");
        }
        else
        {
            Console.WriteLine("error: el número ingresado no tiene dos dígitos.");
            Console.WriteLine("debe ingresar un número entre 10 y 99, o entre -99 y -10.");
        }
    }
}