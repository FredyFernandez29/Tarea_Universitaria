using System;

// Clase Motor
public class Motor
{
    private int litros_de_aceite;
    private int potencia;

    // Constructor: recibe potencia, litros de aceite por defecto 0
    public Motor(int potencia)
    {
        this.potencia = potencia;
        this.litros_de_aceite = 0;
    }

    // Getters y setters 
    public int LitrosDeAceite
    {
        get { return litros_de_aceite; }
        set { litros_de_aceite = value; }
    }

    public int Potencia
    {
        get { return potencia; }
        set { potencia = value; }
    }
}

// Clase Coche
public class Coche
{
    private Motor motor;
    private string marca;
    private string modelo;
    private double precioAcumuladoAverias;

    // Constructor solo con marca y modelo
    public Coche(string marca, string modelo)
    {
        this.marca = marca;
        this.modelo = modelo;
        this.precioAcumuladoAverias = 0.0;
    }

    // Propiedades públicas 
    public Motor Motor
    {
        get { return motor; }
        set { motor = value; }
    }

    public string Marca
    {
        get { return marca; }
    }

    public string Modelo
    {
        get { return modelo; }
    }

    public double PrecioAcumuladoAverias
    {
        get { return precioAcumuladoAverias; }
    }

    // Método para acumular avería
    public void AcumularAveria(double importe)
    {
        this.precioAcumuladoAverias += importe;
    }
}

// Clase Garaje
public class Garaje
{
    private Coche coche;          
    private string averia;        
    private int numeroCochesAtendidos;  

    public Garaje()
    {
        this.coche = null;
        this.averia = null;
        this.numeroCochesAtendidos = 0;
    }

    
    public bool AceptarCoche(Coche coche, string averia)
    {
        if (this.coche != null)
            return false; 

        this.coche = coche;
        this.averia = averia;
        this.numeroCochesAtendidos++;
        return true;
    }

    // Libera el garaje
    public void DevolverCoche()
    {
        this.coche = null;
        this.averia = null;
    }

    
    public int NumeroCochesAtendidos
    {
        get { return numeroCochesAtendidos; }
    }
}

// Clase principal PracticaPOO
public class PracticaPOO
{
    public static void Main(string[] args)
    {
        // Crear garaje
        Garaje garaje = new Garaje();

        // Crear dos coches
        Coche coche1 = new Coche("Toyota", "Corolla");
        Coche coche2 = new Coche("Honda", "Civic");

        // Crear motores 
        Motor motor1 = new Motor(120);
        Motor motor2 = new Motor(150);

        // Asignar motores a los coches
        coche1.Motor = motor1;
        coche2.Motor = motor2;

        Coche[] coches = { coche1, coche2 };

        Random rand = new Random();

        // Cada coche entra al menos 2 veces
        for (int i = 0; i < 2; i++)
        {
            foreach (Coche c in coches)
            {
                // Avería aleatoria (mitad "aceite", mitad otra)
                string averia = rand.NextDouble() < 0.5 ? "aceite" : "freno";

                // Intentar aceptar el coche
                if (garaje.AceptarCoche(c, averia))
                {
                    // Importe aleatorio entre 100 y 400
                    double importe = 100 + rand.NextDouble() * 300;
                    c.AcumularAveria(importe);

                    // Si es avería de aceite, aumentar 10 litros
                    if (averia == "aceite")
                    {
                        Motor m = c.Motor;
                        m.LitrosDeAceite += 10;
                    }

                    // Devolver coche
                    garaje.DevolverCoche();
                }
                else
                {
                    Console.WriteLine("Garaje ocupado, no se pudo atender " + c.Marca);
                }
            }
        }

        // Mostrar información final de los coches
        Console.WriteLine("\nInformación de los coches:");
        foreach (Coche c in coches)
        {
            // Formato con Marca y Modelo separados por coma
            Console.WriteLine("Marca: " + c.Marca + ", Modelo: " + c.Modelo);
            Console.WriteLine("  Precio acumulado averías: " + c.PrecioAcumuladoAverias.ToString("F2")); // dos decimales
            Motor m = c.Motor;
            Console.WriteLine("  Potencia: " + m.Potencia + ", Litros de aceite: " + m.LitrosDeAceite);
        }
    }
}