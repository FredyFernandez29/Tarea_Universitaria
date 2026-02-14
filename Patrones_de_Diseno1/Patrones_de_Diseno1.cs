/*
¿QUE ES UN PATRON DE DISEÑO?
 * Los patrones de diseño son soluciones generales y reutilizables para problemas comunes 
 * que ocurren durante el desarrollo de software. No son codigo completo, sino plantillas 
 * o descripciones de como resolver problemas en diferentes situaciones. 
 * 
 * TIPOS DE PATRONES DE DISEÑO:
 * Los patrones de diseño se dividen en tres categorias principales:
 * 
 * 1. PATRONES CREACIONALES:
 *    - Se enfocan en la creacion de objetos.
 *    - Ayudan a hacer un sistema independiente de como se crean, componen y representan sus objetos.
 *    - Ejemplos: Singleton, Factory Method, Builder, Prototype.
 * 
 * 2. PATRONES ESTRUCTURALES:
 *    - Se ocupan de como se componen las clases y objetos para formar estructuras mas grandes.
 *    - Ayudan a garantizar que si una parte cambia, toda la estructura no necesita cambiar.
 *    - Ejemplos: Adapter, Bridge, Composite, Decorator, Facade.
 * 
 * 3. PATRONES DE COMPORTAMIENTO:
 *    - Se centran en la comunicacion entre objetos.
 *    - Definen como los objetos interactuan y distribuyen responsabilidades.
 *    - Ejemplos: Observer, Strategy, Command, Iterator, State.
 * 
 */


using System;
using System.Collections.Generic;

namespace PatronesDeDiseno
{
    // ==================================================
    // EJEMPLO 1: SINGLETON (Creacional)
    // Utilidad: Una unica instancia global
    // ==================================================
    public class ConfiguracionApp
    {
        private static ConfiguracionApp instanciaUnica;
        private static readonly object candado = new object();
        
        public string Tema { get; set; }
        public string Idioma { get; set; }
        
        private ConfiguracionApp() 
        { 
            Tema = "Claro"; 
            Idioma = "Español";
            Console.WriteLine("Configuracion creada UNA vez");
        }
        
        public static ConfiguracionApp ObtenerInstancia()
        {
            if (instanciaUnica == null)
            {
                lock (candado)
                {
                    if (instanciaUnica == null)
                        instanciaUnica = new ConfiguracionApp();
                }
            }
            return instanciaUnica;
        }
        
        public void Mostrar() => 
            Console.WriteLine($"Tema: {Tema}, Idioma: {Idioma}");
    }

    // ==================================================
    // EJEMPLO 2: ADAPTER (Estructural)
    // Utilidad: Hacer compatibles interfaces diferentes
    // ==================================================
    public interface IPagoLocal
    {
        void Pagar(double pesosArgentinos);
    }

    public class PagoEfectivo : IPagoLocal
    {
        public void Pagar(double pesos) => 
            Console.WriteLine($"Pago local: ${pesos} ARS en efectivo");
    }

    public class PagoInternacional
    {
        public void Cobrar(double dolares) => 
            Console.WriteLine($"Pago internacional: ${dolares} USD");
    }

    public class AdaptadorPago : IPagoLocal
    {
        private PagoInternacional pagoInternacional;
        private double tasaCambio = 0.0011; // 1 ARS = 0.0011 USD aprox
        
        public AdaptadorPago(PagoInternacional pago) => 
            pagoInternacional = pago;
        
        public void Pagar(double pesos)
        {
            double dolares = pesos * tasaCambio;
            Console.WriteLine($"Adaptando ${pesos} ARS a ${dolares:F2} USD");
            pagoInternacional.Cobrar(dolares);
        }
    }

    // ==================================================
    // EJEMPLO 3: OBSERVER (Comportamiento)
    // Utilidad: Notificar cambios a multiples objetos
    // ==================================================
    public interface IObservador
    {
        void Actualizar(string mensaje);
    }

    public class EstacionClima
    {
        private List<IObservador> observadores = new List<IObservador>();
        private int temperatura;
        
        public void Conectar(IObservador obs) => observadores.Add(obs);
        public void Desconectar(IObservador obs) => observadores.Remove(obs);
        
        public void CambiarTemperatura(int nuevaTemp)
        {
            temperatura = nuevaTemp;
            Console.WriteLine($"\nTemperatura cambio a {temperatura}°C");
            foreach (var obs in observadores)
                obs.Actualizar($"Temperatura: {temperatura}°C");
        }
    }

    public class Telefono : IObservador
    {
        private string nombre;
        public Telefono(string n) => nombre = n;
        
        public void Actualizar(string msg) => 
            Console.WriteLine($"   [Telefono de {nombre}] {msg}");
    }

    public class Pantalla : IObservador
    {
        private string ubicacion;
        public Pantalla(string u) => ubicacion = u;
        
        public void Actualizar(string msg) => 
            Console.WriteLine($"   [Pantalla de {ubicacion}] {msg}");
    }

    // ==================================================
    // DEMOSTRACION
    // ==================================================
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== PATRONES DE DISEÑO ===\n");
            
            // 1. Singleton
            Console.WriteLine("--- SINGLETON ---");
            var config1 = ConfiguracionApp.ObtenerInstancia();
            var config2 = ConfiguracionApp.ObtenerInstancia();
            
            config1.Tema = "Oscuro";
            config1.Mostrar();
            Console.WriteLine($"Misma instancia? {config1 == config2}\n");
            
            // 2. Adapter
            Console.WriteLine("--- ADAPTER ---");
            IPagoLocal pagoNormal = new PagoEfectivo();
            pagoNormal.Pagar(1000);
            
            var pagoInternacional = new PagoInternacional();
            IPagoLocal pagoAdaptado = new AdaptadorPago(pagoInternacional);
            pagoAdaptado.Pagar(1000);
            Console.WriteLine();
            
            // 3. Observer
            Console.WriteLine("--- OBSERVER ---");
            var estacion = new EstacionClima();
            var telefono1 = new Telefono("Juan");
            var telefono2 = new Telefono("Ana");
            var pantalla1 = new Pantalla("Centro");
            
            estacion.Conectar(telefono1);
            estacion.Conectar(telefono2);
            estacion.Conectar(pantalla1);
            
            estacion.CambiarTemperatura(25);
            estacion.Desconectar(telefono2);
            estacion.CambiarTemperatura(32);
        }
    }

}
