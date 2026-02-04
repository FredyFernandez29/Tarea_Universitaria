

class Ejercicio_con_funciones_1
{
    static void Main()
    {
        Console.WriteLine("Verificador de años bisiestos");
        Console.WriteLine("Ingresa 'salir' para terminar");
        Console.WriteLine("-------------------------------");
        
        string entrada;
        
        do
        {
            Console.Write("\nIngresa un año O 'salir' para terminar: ");
            entrada = Console.ReadLine();
            
            if (entrada.ToLower() == "salir")
            {
                Console.WriteLine("Programa terminado");
                break;
            }
            
            if (int.TryParse(entrada, out int numero))
            {
                bool resultado = EsBisiesto(numero);
                
                if (resultado)
                {
                    Console.WriteLine($"El año {numero} es bisiesto");
                }
                else
                {
                    Console.WriteLine($"El año {numero} no es bisiesto");
                }
            }
            else
            {
                Console.WriteLine("Entrada no válida. Ingresa un número o 'salir'");
            }
            
        } while (true);
        
        Console.WriteLine("Presiona una tecla para cerrar");
        Console.ReadKey();
    }
    
    static bool EsBisiesto(int valor)
    {
        if (valor % 400 == 0) return true;
        if (valor % 100 == 0) return false;
        if (valor % 4 == 0) return true;
        return false;
    }
}