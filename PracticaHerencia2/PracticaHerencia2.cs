using System;
using System.Collections.Generic;

namespace ChimiMiBarriga
{
    // Interfaz para mostrar el detalle de la venta
    public interface iMostrable
    {
        void MostrarDetalle();
    }

    // Clase base para ingredientes adicionales
    public class Ingrediente
    {
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public Ingrediente(string nombre, double precio)
        {
            Nombre = nombre;
            Precio = precio;
        }
    }

    // Clase base abstracta Hamburguesa
    public abstract class Hamburguesa : iMostrable
    {
        protected string pan;
        protected string carne;
        protected double precioBase;
        protected List<Ingrediente> adicionales;
        protected int maxAdicionales;

        public Hamburguesa(string pan, string carne, double precioBase, int maxAdicionales)
        {
            this.pan = pan;
            this.carne = carne;
            this.precioBase = precioBase;
            this.maxAdicionales = maxAdicionales;
            adicionales = new List<Ingrediente>();
        }

        // Metodo para agregar ingrediente adicional
        public virtual bool AgregarIngrediente(string nombre, double precio)
        {
            if (adicionales.Count < maxAdicionales)
            {
                adicionales.Add(new Ingrediente(nombre, precio));
                return true;
            }
            return false;
        }

        // Calcular precio total
        public double CalcularTotal()
        {
            double total = precioBase;
            foreach (var ing in adicionales)
            {
                total += ing.Precio;
            }
            return total;
        }

        // Mostrar detalle de la venta
        public virtual void MostrarDetalle()
        {
            Console.WriteLine($"\n=== {this.GetType().Name} (hamburguesa {ObtenerTipo()}) ===");
            Console.WriteLine($"Pan: {pan}");
            Console.WriteLine($"Carne: {carne}");
            Console.WriteLine($"Precio base: {precioBase:C}");

            if (adicionales.Count > 0)
            {
                Console.WriteLine("\nIngredientes adicionales:");
                foreach (var ing in adicionales)
                {
                    Console.WriteLine($"  - {ing.Nombre}: {ing.Precio:C}");
                }
            }

            Console.WriteLine($"\nTotal a pagar: {CalcularTotal():C}");
        }

        protected virtual string ObtenerTipo()
        {
            return "base";
        }
    }

    // Hamburguesa BarrigaClasica (hamburguesa clasica)
    public class BarrigaClasica : Hamburguesa
    {
        public BarrigaClasica(string pan, string carne, double precioBase)
            : base(pan, carne, precioBase, 4) // maximo 4 adicionales
        {
        }

        protected override string ObtenerTipo()
        {
            return "clasica";
        }
    }

    // Hamburguesa BarrigaFlaca (hamburguesa saludable)
    public class BarrigaFlaca : Hamburguesa
    {
        public BarrigaFlaca(string carne, double precioBase)
            : base("pan integral", carne, precioBase, 6) // maximo 6 adicionales
        {
        }

        // Metodo especifico para agregar ingredientes saludables
        public bool AgregarIngredienteSaludable(string nombre, double precio)
        {
            return AgregarIngrediente(nombre, precio);
        }

        protected override string ObtenerTipo()
        {
            return "saludable";
        }
    }

    // Hamburguesa BarrigaGrande (hamburguesa premium)
    public class BarrigaGrande : Hamburguesa
    {
        public BarrigaGrande(string pan, string carne, double precioBase)
            : base(pan, carne, precioBase, 4) // permite hasta 4 adicionales
        {
            // Agregar automaticamente PapasBilly y bebida
            AgregarIngrediente("PapasBilly", 2.50);
            AgregarIngrediente("bebida", 1.80);
        }

        protected override string ObtenerTipo()
        {
            return "premium";
        }
    }

    // Clase para probar el sistema
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Chimi MiBarriga ===\n");

            // BarrigaClasica (hamburguesa clasica) con ingredientes adicionales
            BarrigaClasica clasica = new BarrigaClasica("pan blanco", "res", 5.00);
            clasica.AgregarIngrediente("Salsa Especial de la casa", 0.80);
            clasica.AgregarIngrediente("cebolla caramelizada", 0.60);
            clasica.AgregarIngrediente("extra bacon", 1.20);
            clasica.AgregarIngrediente("mostaza", 0.40);
            clasica.MostrarDetalle();

            // BarrigaFlaca (hamburguesa saludable) con ingredientes
            BarrigaFlaca flaca = new BarrigaFlaca("pollo", 6.00);
            flaca.AgregarIngredienteSaludable("Salsa Especial de la casa", 0.80);
            flaca.AgregarIngredienteSaludable("cebolla caramelizada", 0.60);
            flaca.AgregarIngredienteSaludable("extra bacon", 1.20);
            flaca.AgregarIngredienteSaludable("lechuga", 0.50);
            flaca.AgregarIngredienteSaludable("tomate", 0.50);
            flaca.AgregarIngredienteSaludable("aguacate", 1.00);
            flaca.MostrarDetalle();

            // BarrigaGrande (hamburguesa premium) con papas incluidas y adicionales permitidos
            BarrigaGrande grande = new BarrigaGrande("pan brioche", "angus", 8.50);
            grande.AgregarIngrediente("Salsa Especial de la casa", 0.80);
            grande.AgregarIngrediente("extra bacon", 1.20);
            grande.MostrarDetalle();
        }
    }
}