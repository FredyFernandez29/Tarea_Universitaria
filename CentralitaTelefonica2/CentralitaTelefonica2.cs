using System;
using System.Collections.Generic;

namespace CentralitaTelefonica
{
    // ABSTRACCIÓN: clase abstracta Llamada
    public abstract class Llamada
    {
        // Campos privados (ENCAPSULAMIENTO)
        private string numOrigen;
        private string numDestino;
        private double duracion; // segundos

        protected Llamada(string origen, string destino, double duracion)
        {
            this.numOrigen = origen;
            this.numDestino = destino;
            this.duracion = duracion;
        }

        // Propiedades públicas de solo lectura
        public string NumOrigen => numOrigen;
        public string NumDestino => numDestino;
        public double Duracion => duracion;

        // Método abstracto (POLIMORFISMO)
        public abstract double CalcularPrecio();

        public override string ToString()
        {
            return $"Origen: {NumOrigen}, Destino: {NumDestino}, Duración: {Duracion} seg, Coste: {CalcularPrecio():C}";
        }
    }

    // HERENCIA: LlamadaLocal
    public class LlamadaLocal : Llamada
    {
        private const double PRECIO_SEGUNDO = 0.15; // 15 céntimos

        public LlamadaLocal(string origen, string destino, double duracion)
            : base(origen, destino, duracion) { }

        public override double CalcularPrecio()
        {
            return Duracion * PRECIO_SEGUNDO;
        }
    }

    // HERENCIA: LlamadaProvincial
    public class LlamadaProvincial : Llamada
    {
        private static readonly double[] PreciosFranja = { 0.20, 0.25, 0.30 };
        private int franja;

        public LlamadaProvincial(string origen, string destino, double duracion, int franja)
            : base(origen, destino, duracion)
        {
            if (franja < 1 || franja > 3)
                throw new ArgumentException("Franja debe ser 1, 2 o 3.");
            this.franja = franja;
        }

        public int Franja => franja;

        public override double CalcularPrecio()
        {
            return Duracion * PreciosFranja[Franja - 1];
        }

        public override string ToString()
        {
            return base.ToString() + $", Franja: {Franja}";
        }
    }

    // Clase Centralita
    public class Centralita
    {
        // Lista polimórfica
        private List<Llamada> llamadas;

        // Variables de totales (ENCAPSULAMIENTO: privadas con propiedades públicas)
        private int totalLlamadas;
        private double totalFacturado;

        public Centralita()
        {
            llamadas = new List<Llamada>();
            totalLlamadas = 0;
            totalFacturado = 0.0;
        }

        // Propiedades públicas (solo lectura desde fuera)
        public int TotalLlamadas => totalLlamadas;
        public double TotalFacturado => totalFacturado;

        // Registrar una llamada: actualiza totales y muestra la llamada
        public void RegistrarLlamada(Llamada llamada)
        {
            llamadas.Add(llamada);
            double coste = llamada.CalcularPrecio();

            totalLlamadas++;
            totalFacturado += coste;

            Console.WriteLine("Llamada registrada:");
            Console.WriteLine(llamada);
            Console.WriteLine();
        }

        // Genera informe usando las variables de totales
        public void GenerarInforme()
        {
            Console.WriteLine("--- INFORME DE CENTRALITA ---");
            Console.WriteLine($"Total de llamadas: {totalLlamadas}");
            Console.WriteLine($"Facturación total: {totalFacturado:C}");
        }

        // Opcional: mostrar todas las llamadas
        public void MostrarTodasLlamadas()
        {
            Console.WriteLine("--- LISTADO DE LLAMADAS ---");
            foreach (var llamada in llamadas)
            {
                Console.WriteLine(llamada);
            }
            Console.WriteLine();
        }
    }

    // Clase principal
    public class Practica2
    {
        public static void Main(string[] args)
        {
            Centralita central = new Centralita();
            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("=== MENÚ CENTRALITA ===");
                Console.WriteLine("1. Registrar llamada local");
                Console.WriteLine("2. Registrar llamada provincial");
                Console.WriteLine("3. Mostrar todas las llamadas");
                Console.WriteLine("4. Generar informe");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RegistrarLlamadaLocal(central);
                        break;
                    case "2":
                        RegistrarLlamadaProvincial(central);
                        break;
                    case "3":
                        central.MostrarTodasLlamadas();
                        break;
                    case "4":
                        central.GenerarInforme();
                        break;
                    case "5":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void RegistrarLlamadaLocal(Centralita central)
        {
            try
            {
                Console.Write("Número origen: ");
                string origen = Console.ReadLine();
                Console.Write("Número destino: ");
                string destino = Console.ReadLine();
                Console.Write("Duración (segundos): ");
                double duracion = double.Parse(Console.ReadLine());

                LlamadaLocal local = new LlamadaLocal(origen, destino, duracion);
                central.RegistrarLlamada(local);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la llamada: {ex.Message}");
            }
        }

        private static void RegistrarLlamadaProvincial(Centralita central)
        {
            try
            {
                Console.Write("Número origen: ");
                string origen = Console.ReadLine();
                Console.Write("Número destino: ");
                string destino = Console.ReadLine();
                Console.Write("Duración (segundos): ");
                double duracion = double.Parse(Console.ReadLine());
                Console.Write("Franja horaria (1, 2 o 3): ");
                int franja = int.Parse(Console.ReadLine());

                LlamadaProvincial provincial = new LlamadaProvincial(origen, destino, duracion, franja);
                central.RegistrarLlamada(provincial);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la llamada: {ex.Message}");
            }
        }
    }
}