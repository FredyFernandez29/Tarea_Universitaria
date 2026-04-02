using System;
using System.Collections.Generic;

namespace SistemaNotasEscolares
{
    // ─── interfaces ───────────────────────────────────────────────────────────

    interface IEntidad
    {
        int Id { get; set; }
        string Nombre { get; set; }
        string ObtenerResumen();
    }

    interface IEstadistico
    {
        double CalcularPromedio();
        double ObtenerNotaMaxima();
        double ObtenerNotaMinima();
    }

    // ─── clases base ──────────────────────────────────────────────────────────

    public abstract class Persona : IEntidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public abstract string ObtenerResumen();
    }

    // ─── entidades ────────────────────────────────────────────────────────────

    public class Estudiante : Persona
    {
        public int Grado { get; set; }

        public override string ObtenerResumen()
        {
            return $"[{Id:D3}] {Nombre}  |  Grado {Grado}";
        }
    }

    public class Materia : IEntidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public string ObtenerResumen()
        {
            return $"[{Id:D3}] {Nombre}";
        }
    }

    public class Nota
    {
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public double Calificacion { get; set; }
    }

    // ─── conversion de calificacion a literal y gpa ───────────────────────────

    public static class ConversorCalificacion
    {
        public static string ObtenerLiteral(double calificacion)
        {
            if (calificacion >= 90) return "A";
            if (calificacion >= 80) return "B";
            if (calificacion >= 70) return "C";
            if (calificacion >= 60) return "D";
            return "F";
        }

        public static double ObtenerPuntosGpa(double calificacion)
        {
            if (calificacion >= 90) return 4.0;
            if (calificacion >= 80) return 3.0;
            if (calificacion >= 70) return 2.0;
            if (calificacion >= 60) return 1.0;
            return 0.0;
        }

        public static bool EsAprobado(double calificacion)
        {
            return calificacion >= 70;
        }
    }

    // ─── estadisticas de estudiante ───────────────────────────────────────────

    public class EstadisticasEstudiante : IEstadistico
    {
        private List<Nota> notas;
        private string nombreEstudiante;

        public EstadisticasEstudiante(string nombre, List<Nota> notas)
        {
            this.nombreEstudiante = nombre;
            this.notas = notas;
        }

        public double CalcularPromedio()
        {
            if (notas.Count == 0) return 0;
            double suma = 0;
            foreach (Nota n in notas) suma += n.Calificacion;
            return suma / notas.Count;
        }

        public double ObtenerNotaMaxima()
        {
            if (notas.Count == 0) return 0;
            double max = notas[0].Calificacion;
            foreach (Nota n in notas)
                if (n.Calificacion > max) max = n.Calificacion;
            return max;
        }

        public double ObtenerNotaMinima()
        {
            if (notas.Count == 0) return 0;
            double min = notas[0].Calificacion;
            foreach (Nota n in notas)
                if (n.Calificacion < min) min = n.Calificacion;
            return min;
        }

        public double CalcularGpa()
        {
            if (notas.Count == 0) return 0;
            double suma = 0;
            foreach (Nota n in notas)
                suma += ConversorCalificacion.ObtenerPuntosGpa(n.Calificacion);
            return suma / notas.Count;
        }

        public string ObtenerEstadoGeneral()
        {
            double promedio = CalcularPromedio();
            if (promedio >= 90) return "Excelente";
            if (promedio >= 80) return "Muy bueno";
            if (promedio >= 70) return "Aprobado";
            return "Reprobado";
        }

        public void MostrarEstadisticas()
        {
            double promedio = CalcularPromedio();
            double gpa      = CalcularGpa();
            string literal  = ConversorCalificacion.ObtenerLiteral(promedio);
            string estado   = ObtenerEstadoGeneral();

            Ui.Titulo($"Estadisticas de {nombreEstudiante}");
            Ui.Fila("Promedio general", $"{promedio:F2}");
            Ui.Fila("Literal",          literal);
            Ui.Fila("GPA",              $"{gpa:F2} / 4.00");
            Ui.Fila("Nota maxima",      $"{ObtenerNotaMaxima():F2}");
            Ui.Fila("Nota minima",      $"{ObtenerNotaMinima():F2}");
            Ui.Fila("Materias cursadas", $"{notas.Count}");
            Ui.FilaEstado("Estado", estado, ConversorCalificacion.EsAprobado(promedio));
            Ui.LineaSimple();
        }
    }

    // ─── estadisticas de materia ──────────────────────────────────────────────

    public class EstadisticasMateria : IEstadistico
    {
        private List<Nota> notas;
        private string nombreMateria;

        public EstadisticasMateria(string nombre, List<Nota> notas)
        {
            this.nombreMateria = nombre;
            this.notas = notas;
        }

        public double CalcularPromedio()
        {
            if (notas.Count == 0) return 0;
            double suma = 0;
            foreach (Nota n in notas) suma += n.Calificacion;
            return suma / notas.Count;
        }

        public double ObtenerNotaMaxima()
        {
            if (notas.Count == 0) return 0;
            double max = notas[0].Calificacion;
            foreach (Nota n in notas)
                if (n.Calificacion > max) max = n.Calificacion;
            return max;
        }

        public double ObtenerNotaMinima()
        {
            if (notas.Count == 0) return 0;
            double min = notas[0].Calificacion;
            foreach (Nota n in notas)
                if (n.Calificacion < min) min = n.Calificacion;
            return min;
        }

        public int ContarAprobados()
        {
            int count = 0;
            foreach (Nota n in notas)
                if (ConversorCalificacion.EsAprobado(n.Calificacion)) count++;
            return count;
        }

        public int ContarReprobados()
        {
            return notas.Count - ContarAprobados();
        }

        public void MostrarEstadisticas()
        {
            double promedio = CalcularPromedio();

            Ui.Titulo($"Resumen de {nombreMateria}");
            Ui.Fila("Promedio del grupo", $"{promedio:F2}  ({ConversorCalificacion.ObtenerLiteral(promedio)})");
            Ui.Fila("Nota mas alta",      $"{ObtenerNotaMaxima():F2}");
            Ui.Fila("Nota mas baja",      $"{ObtenerNotaMinima():F2}");
            Ui.Fila("Total estudiantes",  $"{notas.Count}");
            Ui.Fila("Aprobados",          $"{ContarAprobados()}");
            Ui.Fila("Reprobados",         $"{ContarReprobados()}");
            Ui.LineaSimple();
        }
    }

    // ─── interfaz visual en terminal ──────────────────────────────────────────

    public static class Ui
    {
        private const int Ancho = 58;

        private static ConsoleColor colorTitulo   = ConsoleColor.Cyan;
        private static ConsoleColor colorExito    = ConsoleColor.Green;
        private static ConsoleColor colorError    = ConsoleColor.Red;
        private static ConsoleColor colorWarning  = ConsoleColor.Yellow;
        private static ConsoleColor colorSubtexto = ConsoleColor.DarkGray;
        private static ConsoleColor colorInput    = ConsoleColor.White;

        private static void Escribir(string texto, ConsoleColor color, bool nueva = true)
        {
            Console.ForegroundColor = color;
            if (nueva) Console.WriteLine(texto);
            else Console.Write(texto);
            Console.ResetColor();
        }

        public static void LineaDoble()
        {
            Escribir(new string('=', Ancho), colorTitulo);
        }

        public static void LineaSimple()
        {
            Escribir(new string('-', Ancho), colorSubtexto);
        }

        public static void Titulo(string texto)
        {
            Console.WriteLine();
            LineaDoble();
            Console.ForegroundColor = colorTitulo;
            Console.WriteLine($"  {texto.ToUpper()}");
            Console.ResetColor();
            LineaDoble();
        }

        public static void Pantalla(string texto)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('#', Ancho));
            Console.ForegroundColor = ConsoleColor.Cyan;
            string centrado = texto.PadLeft((Ancho + texto.Length) / 2).PadRight(Ancho);
            Console.WriteLine(centrado);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('#', Ancho));
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void Exito(string texto)
        {
            Escribir($"  + {texto}", colorExito);
        }

        public static void Error(string texto)
        {
            Escribir($"  ! {texto}", colorError);
        }

        public static void Info(string texto)
        {
            Escribir($"  > {texto}", colorWarning);
        }

        public static void Fila(string etiqueta, string valor)
        {
            Console.ForegroundColor = colorSubtexto;
            Console.Write($"  {etiqueta,-22}: ");
            Console.ForegroundColor = colorInput;
            Console.WriteLine(valor);
            Console.ResetColor();
        }

        public static void FilaEstado(string etiqueta, string valor, bool aprobado)
        {
            Console.ForegroundColor = colorSubtexto;
            Console.Write($"  {etiqueta,-22}: ");
            Console.ForegroundColor = aprobado ? colorExito : colorError;
            Console.WriteLine(valor);
            Console.ResetColor();
        }

        public static void EntradaPrompt(string texto)
        {
            Console.ForegroundColor = colorWarning;
            Console.Write($"  {texto}: ");
            Console.ForegroundColor = colorInput;
        }

        public static string Leer()
        {
            string entrada = Console.ReadLine();
            Console.ResetColor();
            return entrada ?? "";
        }

        public static void Separador() => Console.WriteLine();

        public static void OpcionMenu(string numero, string texto)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"  [{numero,-2}]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  {texto}");
            Console.ResetColor();
        }

        public static void FilaEstudiante(Estudiante e, string extra = "")
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"  [{e.Id:D3}]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  {e.Nombre,-25}");
            Console.ForegroundColor = colorSubtexto;
            Console.Write($"  Grado {e.Grado,2}");
            if (!string.IsNullOrEmpty(extra))
            {
                Console.Write("   ");
                Console.ForegroundColor = colorWarning;
                Console.Write(extra);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void BannerInicio()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("  +======================================================+");
            Console.WriteLine("  |                                                      |");
            Console.WriteLine("  |     SISTEMA DE GESTION DE NOTAS ESCOLARES            |");
            Console.WriteLine("  |                                                      |");
            Console.WriteLine("  |      Educacion con datos, datos con proposito        |");
            Console.WriteLine("  |                                                      |");
            Console.WriteLine("  +======================================================+");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine("  Literales: A(90-100)  B(80-89)  C(70-79)  D(60-69)  F(<60)");
            Console.WriteLine("  Aprobado desde 70  |  GPA maximo: 4.00");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    // ─── gestor central ───────────────────────────────────────────────────────

    public class GestorNotas
    {
        public const double NotaMinima  = 0;
        public const double NotaMaxima  = 100;
        public const int    GradoMinimo = 1;
        public const int    GradoMaximo = 12;

        private List<Estudiante> estudiantes = new List<Estudiante>();
        private List<Materia>    materias    = new List<Materia>();
        private List<Nota>       notas       = new List<Nota>();

        private int siguienteIdEstudiante = 1;
        private int siguienteIdMateria    = 1;

        public void AgregarEstudiante(string nombre, int grado)
        {
            estudiantes.Add(new Estudiante { Id = siguienteIdEstudiante++, Nombre = nombre, Grado = grado });
        }

        public bool EliminarEstudiante(int id)
        {
            Estudiante encontrado = null;
            foreach (Estudiante e in estudiantes)
                if (e.Id == id) { encontrado = e; break; }

            if (encontrado == null) return false;

            estudiantes.Remove(encontrado);

            List<Nota> aEliminar = new List<Nota>();
            foreach (Nota n in notas)
                if (n.EstudianteId == id) aEliminar.Add(n);
            foreach (Nota n in aEliminar) notas.Remove(n);

            return true;
        }

        public void AgregarMateria(string nombre)
        {
            materias.Add(new Materia { Id = siguienteIdMateria++, Nombre = nombre });
        }

        public void AgregarNota(int estudianteId, int materiaId, double calificacion)
        {
            notas.Add(new Nota { EstudianteId = estudianteId, MateriaId = materiaId, Calificacion = calificacion });
        }

        public bool EstudianteExiste(int id)
        {
            foreach (Estudiante e in estudiantes) if (e.Id == id) return true;
            return false;
        }

        public bool MateriaExiste(int id)
        {
            foreach (Materia m in materias) if (m.Id == id) return true;
            return false;
        }

        public List<Estudiante> ObtenerTodosEstudiantes()  => estudiantes;
        public List<Materia>    ObtenerTodasMaterias()     => materias;

        public Estudiante ObtenerEstudiante(int id)
        {
            foreach (Estudiante e in estudiantes) if (e.Id == id) return e;
            return null;
        }

        public Materia ObtenerMateria(int id)
        {
            foreach (Materia m in materias) if (m.Id == id) return m;
            return null;
        }

        public string ObtenerNombreEstudiante(int id)
        {
            foreach (Estudiante e in estudiantes) if (e.Id == id) return e.Nombre;
            return "Desconocido";
        }

        public List<Nota> ObtenerNotasPorEstudiante(int id)
        {
            List<Nota> resultado = new List<Nota>();
            foreach (Nota n in notas) if (n.EstudianteId == id) resultado.Add(n);
            return resultado;
        }

        public List<Nota> ObtenerNotasPorMateria(int id)
        {
            List<Nota> resultado = new List<Nota>();
            foreach (Nota n in notas) if (n.MateriaId == id) resultado.Add(n);
            return resultado;
        }

        public List<Estudiante> ObtenerEstudiantesPorGrado(int grado)
        {
            List<Estudiante> resultado = new List<Estudiante>();
            foreach (Estudiante e in estudiantes) if (e.Grado == grado) resultado.Add(e);
            return resultado;
        }
    }

    // ─── programa principal ───────────────────────────────────────────────────

    class Program
    {
        static GestorNotas gestor = new GestorNotas();

        static void Main(string[] args)
        {
            Ui.BannerInicio();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Presione Enter para continuar...");
            Console.ResetColor();
            Console.ReadLine();

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                Ui.EntradaPrompt("Opcion");
                string entrada = Ui.Leer().Trim();

                switch (entrada)
                {
                    case "1":  AgregarEstudiante();          break;
                    case "2":  AgregarMateria();             break;
                    case "3":  AgregarNota();                break;
                    case "4":  VerTodosEstudiantes();        break;
                    case "5":  VerNotasEstudiante();         break;
                    case "6":  ResumenPorMateria();          break;
                    case "7":  EstadisticasEstudianteMenu(); break;
                    case "8":  EstadisticasGenerales();      break;
                    case "9":  EstudiantesPorGrado();        break;
                    case "10": EliminarEstudiante();         break;
                    case "11": salir = true;                 break;
                    default:
                        Ui.Error("Opcion invalida.");
                        break;
                }

                if (!salir)
                {
                    Ui.Separador();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("  Presione Enter para continuar...");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }

            Ui.Pantalla("Hasta luego");
        }

        static void MostrarMenu()
        {
            Ui.Pantalla("Menu principal");
            Ui.OpcionMenu("1",  "Agregar estudiante");
            Ui.OpcionMenu("2",  "Agregar materia");
            Ui.OpcionMenu("3",  "Agregar nota");
            Ui.OpcionMenu("4",  "Ver todos los estudiantes");
            Ui.OpcionMenu("5",  "Ver notas de un estudiante");
            Ui.OpcionMenu("6",  "Resumen por materia");
            Ui.OpcionMenu("7",  "Estadisticas de un estudiante");
            Ui.OpcionMenu("8",  "Estadisticas generales del colegio");
            Ui.OpcionMenu("9",  "Ver estudiantes por grado");
            Ui.OpcionMenu("10", "Eliminar estudiante");
            Ui.OpcionMenu("11", "Salir");
            Ui.LineaSimple();
        }

        // ── helpers de entrada ────────────────────────────────────────────────

        static bool LeerEntero(string prompt, out int resultado)
        {
            Ui.EntradaPrompt(prompt);
            if (!int.TryParse(Ui.Leer(), out resultado))
            {
                Ui.Error("Debe ingresar un numero entero.");
                return false;
            }
            return true;
        }

        static bool LeerDecimal(string prompt, out double resultado)
        {
            Ui.EntradaPrompt(prompt);
            if (!double.TryParse(Ui.Leer(), out resultado))
            {
                Ui.Error("Debe ingresar un numero valido.");
                return false;
            }
            return true;
        }

        static void MostrarListaEstudiantes()
        {
            Ui.LineaSimple();
            foreach (Estudiante e in gestor.ObtenerTodosEstudiantes())
                Ui.FilaEstudiante(e);
            Ui.LineaSimple();
        }

        static void MostrarListaMaterias()
        {
            Ui.LineaSimple();
            foreach (Materia m in gestor.ObtenerTodasMaterias())
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"  [{m.Id:D3}]");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  {m.Nombre}");
                Console.ResetColor();
            }
            Ui.LineaSimple();
        }

        // ── opciones ──────────────────────────────────────────────────────────

        static void AgregarEstudiante()
        {
            Ui.Titulo("Agregar estudiante");
            Ui.EntradaPrompt("Nombre");
            string nombre = Ui.Leer();
            if (string.IsNullOrWhiteSpace(nombre)) { Ui.Error("El nombre no puede estar vacio."); return; }

            if (!LeerEntero("Grado (1 a 12)", out int grado)) return;
            if (grado < GestorNotas.GradoMinimo || grado > GestorNotas.GradoMaximo)
            {
                Ui.Error("El grado debe estar entre 1 y 12.");
                return;
            }

            gestor.AgregarEstudiante(nombre, grado);
            Ui.Exito($"{nombre} registrado en grado {grado}.");
        }

        static void AgregarMateria()
        {
            Ui.Titulo("Agregar materia");
            Ui.EntradaPrompt("Nombre");
            string nombre = Ui.Leer();
            if (string.IsNullOrWhiteSpace(nombre)) { Ui.Error("El nombre no puede estar vacio."); return; }
            gestor.AgregarMateria(nombre);
            Ui.Exito($"Materia '{nombre}' agregada.");
        }

        static void AgregarNota()
        {
            Ui.Titulo("Agregar nota");

            if (gestor.ObtenerTodosEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }
            if (gestor.ObtenerTodasMaterias().Count == 0)    { Ui.Error("No hay materias registradas.");    return; }

            Ui.Info("Estudiantes:");
            MostrarListaEstudiantes();
            if (!LeerEntero("Id del estudiante", out int estudianteId)) return;
            if (!gestor.EstudianteExiste(estudianteId)) { Ui.Error("El estudiante no existe."); return; }

            Ui.Info("Materias:");
            MostrarListaMaterias();
            if (!LeerEntero("Id de la materia", out int materiaId)) return;
            if (!gestor.MateriaExiste(materiaId)) { Ui.Error("La materia no existe."); return; }

            if (!LeerDecimal("Nota (0 a 100)", out double calificacion)) return;
            if (calificacion < GestorNotas.NotaMinima || calificacion > GestorNotas.NotaMaxima)
            {
                Ui.Error("La nota debe estar entre 0 y 100.");
                return;
            }

            gestor.AgregarNota(estudianteId, materiaId, calificacion);
            string literal = ConversorCalificacion.ObtenerLiteral(calificacion);
            string estado  = ConversorCalificacion.EsAprobado(calificacion) ? "aprobado" : "reprobado";
            Ui.Exito($"Nota {calificacion} -> Literal: {literal}  |  {estado}.");
        }

        static void VerTodosEstudiantes()
        {
            Ui.Titulo("Todos los estudiantes");

            List<Estudiante> todos = gestor.ObtenerTodosEstudiantes();
            if (todos.Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            Ui.Fila("Total", $"{todos.Count}");
            Ui.LineaSimple();

            foreach (Estudiante e in todos)
            {
                List<Nota> notas = gestor.ObtenerNotasPorEstudiante(e.Id);
                string extra = "sin notas";
                if (notas.Count > 0)
                {
                    IEstadistico stats = new EstadisticasEstudiante(e.Nombre, notas);
                    EstadisticasEstudiante statsE = (EstadisticasEstudiante)stats;
                    double prom = stats.CalcularPromedio();
                    extra = $"Prom: {prom:F1}  Literal: {ConversorCalificacion.ObtenerLiteral(prom)}  GPA: {statsE.CalcularGpa():F2}";
                }
                Ui.FilaEstudiante(e, extra);
            }
        }

        static void VerNotasEstudiante()
        {
            Ui.Titulo("Notas de estudiante");

            if (gestor.ObtenerTodosEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            MostrarListaEstudiantes();
            if (!LeerEntero("Id del estudiante", out int id)) return;
            if (!gestor.EstudianteExiste(id)) { Ui.Error("El estudiante no existe."); return; }

            List<Nota> notas = gestor.ObtenerNotasPorEstudiante(id);
            Estudiante est   = gestor.ObtenerEstudiante(id);

            Ui.Separador();
            Ui.Fila("Estudiante", est.Nombre);
            Ui.Fila("Grado",      $"{est.Grado}");
            Ui.LineaSimple();

            if (notas.Count == 0) { Ui.Info("Sin notas registradas."); return; }

            foreach (Nota n in notas)
            {
                Materia mat     = gestor.ObtenerMateria(n.MateriaId);
                string  nomMat  = mat != null ? mat.Nombre : "Materia desconocida";
                string  lit     = ConversorCalificacion.ObtenerLiteral(n.Calificacion);
                bool    aprobado = ConversorCalificacion.EsAprobado(n.Calificacion);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  {nomMat,-25}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"  {n.Calificacion,6:F2}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  ({lit})");
                Console.ForegroundColor = aprobado ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  {(aprobado ? "aprobado" : "reprobado")}");
                Console.ResetColor();
            }
        }

        static void ResumenPorMateria()
        {
            Ui.Titulo("Resumen por materia");

            if (gestor.ObtenerTodasMaterias().Count == 0) { Ui.Error("No hay materias registradas."); return; }

            MostrarListaMaterias();
            if (!LeerEntero("Id de la materia", out int id)) return;

            Materia materia = gestor.ObtenerMateria(id);
            if (materia == null) { Ui.Error("La materia no existe."); return; }

            List<Nota> notas = gestor.ObtenerNotasPorMateria(id);
            if (notas.Count == 0) { Ui.Info("No hay notas registradas en esta materia."); return; }

            IEstadistico stats = new EstadisticasMateria(materia.Nombre, notas);
            EstadisticasMateria statsM = (EstadisticasMateria)stats;
            statsM.MostrarEstadisticas();

            Ui.Info("Detalle por estudiante:");
            Ui.LineaSimple();

            foreach (Nota n in notas)
            {
                Estudiante est  = gestor.ObtenerEstudiante(n.EstudianteId);
                string nombre   = est != null ? $"{est.Nombre} (G{est.Grado})" : "Desconocido";
                string lit      = ConversorCalificacion.ObtenerLiteral(n.Calificacion);
                bool   aprobado = ConversorCalificacion.EsAprobado(n.Calificacion);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  {nombre,-30}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"  {n.Calificacion,6:F2}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  ({lit})");
                Console.ForegroundColor = aprobado ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  {(aprobado ? "aprobado" : "reprobado")}");
                Console.ResetColor();
            }
        }

        static void EstadisticasEstudianteMenu()
        {
            Ui.Titulo("Estadisticas de estudiante");

            if (gestor.ObtenerTodosEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            MostrarListaEstudiantes();
            if (!LeerEntero("Id del estudiante", out int id)) return;
            if (!gestor.EstudianteExiste(id)) { Ui.Error("El estudiante no existe."); return; }

            List<Nota> notas = gestor.ObtenerNotasPorEstudiante(id);
            string nombre    = gestor.ObtenerNombreEstudiante(id);

            if (notas.Count == 0) { Ui.Info("El estudiante no tiene notas registradas."); return; }

            IEstadistico stats = new EstadisticasEstudiante(nombre, notas);
            EstadisticasEstudiante statsE = (EstadisticasEstudiante)stats;
            statsE.MostrarEstadisticas();
        }

        static void EstadisticasGenerales()
        {
            Ui.Titulo("Estadisticas generales del colegio");

            List<Estudiante> todos = gestor.ObtenerTodosEstudiantes();
            if (todos.Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            int    totalConNotas   = 0;
            int    totalAprobados  = 0;
            int    totalReprobados = 0;
            double mejorPromedio   = -1;
            double peorPromedio    = 101;
            string mejorEstudiante = "";
            string peorEstudiante  = "";
            double sumaGpas        = 0;

            foreach (Estudiante e in todos)
            {
                List<Nota> notas = gestor.ObtenerNotasPorEstudiante(e.Id);
                if (notas.Count == 0) continue;

                totalConNotas++;
                IEstadistico stats = new EstadisticasEstudiante(e.Nombre, notas);
                EstadisticasEstudiante statsE = (EstadisticasEstudiante)stats;

                double promedio = stats.CalcularPromedio();
                sumaGpas += statsE.CalcularGpa();

                if (ConversorCalificacion.EsAprobado(promedio)) totalAprobados++;
                else totalReprobados++;

                if (promedio > mejorPromedio)
                {
                    mejorPromedio   = promedio;
                    mejorEstudiante = $"{e.Nombre} (Grado {e.Grado})";
                }

                if (promedio < peorPromedio)
                {
                    peorPromedio   = promedio;
                    peorEstudiante = $"{e.Nombre} (Grado {e.Grado})";
                }
            }

            Ui.Fila("Total estudiantes",    $"{todos.Count}");
            Ui.Fila("Con notas",            $"{totalConNotas}");
            Ui.Fila("Aprobados",            $"{totalAprobados}");
            Ui.Fila("Reprobados",           $"{totalReprobados}");

            if (totalConNotas > 0)
            {
                Ui.Fila("GPA promedio colegio", $"{(sumaGpas / totalConNotas):F2} / 4.00");
                Ui.LineaSimple();
                Ui.Fila("Mejor promedio",  $"{mejorEstudiante}  ({mejorPromedio:F2})");
                Ui.Fila("Menor promedio",  $"{peorEstudiante}  ({peorPromedio:F2})");
            }

            Ui.LineaSimple();
            Ui.Info("Promedio por materia:");
            Ui.Separador();

            foreach (Materia m in gestor.ObtenerTodasMaterias())
            {
                List<Nota> nm = gestor.ObtenerNotasPorMateria(m.Id);
                if (nm.Count == 0) { Ui.Fila(m.Nombre, "sin notas"); continue; }
                IEstadistico sm   = new EstadisticasMateria(m.Nombre, nm);
                double       prom = sm.CalcularPromedio();
                Ui.Fila(m.Nombre, $"{prom:F2}  ({ConversorCalificacion.ObtenerLiteral(prom)})");
            }
        }

        static void EstudiantesPorGrado()
        {
            Ui.Titulo("Estudiantes por grado");

            if (!LeerEntero("Grado (1 a 12)", out int grado)) return;
            if (grado < GestorNotas.GradoMinimo || grado > GestorNotas.GradoMaximo)
            {
                Ui.Error("El grado debe estar entre 1 y 12.");
                return;
            }

            List<Estudiante> lista = gestor.ObtenerEstudiantesPorGrado(grado);
            if (lista.Count == 0) { Ui.Info($"No hay estudiantes en grado {grado}."); return; }

            Ui.Fila("Grado",       $"{grado}");
            Ui.Fila("Estudiantes", $"{lista.Count}");
            Ui.LineaSimple();

            foreach (Estudiante e in lista)
            {
                List<Nota> notas = gestor.ObtenerNotasPorEstudiante(e.Id);
                string extra = "sin notas";
                if (notas.Count > 0)
                {
                    IEstadistico stats = new EstadisticasEstudiante(e.Nombre, notas);
                    EstadisticasEstudiante statsE = (EstadisticasEstudiante)stats;
                    double prom = stats.CalcularPromedio();
                    extra = $"Prom: {prom:F1}  Literal: {ConversorCalificacion.ObtenerLiteral(prom)}  GPA: {statsE.CalcularGpa():F2}";
                }
                Ui.FilaEstudiante(e, extra);
            }
        }

        static void EliminarEstudiante()
        {
            Ui.Titulo("Eliminar estudiante");

            if (gestor.ObtenerTodosEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            MostrarListaEstudiantes();
            if (!LeerEntero("Id del estudiante a eliminar", out int id)) return;

            string nombre = gestor.ObtenerNombreEstudiante(id);
            if (!gestor.EstudianteExiste(id)) { Ui.Error("El estudiante no existe."); return; }

            Ui.EntradaPrompt($"Confirmar eliminacion de '{nombre}' (s/n)");
            string confirm = Ui.Leer().Trim().ToLower();

            if (confirm != "s") { Ui.Info("Operacion cancelada."); return; }

            bool ok = gestor.EliminarEstudiante(id);
            if (ok) Ui.Exito($"'{nombre}' eliminado junto con todas sus notas.");
            else    Ui.Error("El estudiante no existe.");
        }
    }
}