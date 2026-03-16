using System;

class Persona
{
    public string Nombre { get; set; }
    public int Edad { get; set; }

    public void Saludar()
    {
        Console.WriteLine("Hola soy " + Nombre);
    }
}

class Estudiante : Persona
{
    public string Carrera { get; set; }

    public void Estudiar()
    {
        Console.WriteLine(Nombre + " está estudiando " + Carrera);
    }
}

class Program
{
    static void Main()
    {
        Estudiante estudiante = new Estudiante();
        estudiante.Nombre = "Carlos";
        estudiante.Edad = 20;
        estudiante.Carrera = "Ingeniería";
        estudiante.Saludar();
        estudiante.Estudiar();
    }
}