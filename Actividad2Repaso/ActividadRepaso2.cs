using System;

namespace FernandezBus
{
    public interface IVenta
    {
        decimal CalcularVenta();
    }
    
    public abstract class Bus : IVenta
    {
        public string Placa { get; set; }
        public string Nombre { get; set; }
        public int CapacidadTotal { get; set; }
        public int Pasajeros { get; set; }
        public decimal Ventas { get; set; }
        
        public Bus(string nombre, int capacidadTotal, decimal ventas)
        {
            Nombre = nombre;
            CapacidadTotal = capacidadTotal;
            Ventas = ventas;
            Pasajeros = 0;
            Placa = GenerarPlaca();
        }
        
        private string GenerarPlaca()
        {
            Random rand = new Random();
            return $"{rand.Next(100, 999)}{rand.Next(100, 999)}";
        }
        
        public decimal CalcularVenta()
        {
            return Ventas;
        }
        
        public void RegistrarPasajero(int cantidad)
        {
            Pasajeros = cantidad;
        }
        
        public int AsientosDisponibles()
        {
            return CapacidadTotal - Pasajeros;
        }
    }
    
    public class BusPlantinum : Bus
    {
        public BusPlantinum() : base("Plantinum", 22, 5000) { }
    }
    
    public class BusGold : Bus
    {
        public BusGold() : base("Gold", 15, 4000) { }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Bus plantinum = new BusPlantinum();
            Bus gold = new BusGold();
            
            plantinum.RegistrarPasajero(5);
            gold.RegistrarPasajero(3);
            Console.WriteLine("\nFernandezBus");
            Console.WriteLine($"\nAuto Bus {plantinum.Nombre} {plantinum.Pasajeros} Pasajeros Ventas {plantinum.CalcularVenta():N0} quedan {plantinum.AsientosDisponibles()} asientos disponibles | Placa: {plantinum.Placa}");
            Console.WriteLine($"\nAuto Bus {gold.Nombre} {gold.Pasajeros} Pasajeros Ventas {gold.CalcularVenta():N0} quedan {gold.AsientosDisponibles()} asientos disponibles | Placa: {gold.Placa}\n");
        }
    }
}