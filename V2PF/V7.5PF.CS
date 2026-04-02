using System;
using System.Collections.Generic;

namespace SistemaNotasEscolares
{
    // --- interfaces ----------------------------------------------------------
    interface IEntidad
    {
        int Id { get; set; }
        string Nombre { get; set; }
    }

    // --- clases base ----------------------------------------------------------
    public abstract class Persona : IEntidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public abstract string ObtenerResumen();
    }

    // --- entidades ------------------------------------------------------------
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
        public int Creditos { get; set; }  // Nuevo: creditos de la materia
        public string ObtenerResumen()
        {
            return $"[{Id:D3}] {Nombre} ({Creditos} cr)";
        }
    }

    public class Nota
    {
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public double Calificacion { get; set; }
    }

    // --- conversor de calificaciones -----------------------------------------
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

        public static bool EsAprobado(double calificacion) => calificacion >= 70;
    }

    // --- gestor central (incluye estadisticas) -------------------------------
    public class GestorNotas
    {
        private List<Estudiante> estudiantes = new List<Estudiante>();
        private List<Materia> materias = new List<Materia>();
        private List<Nota> notas = new List<Nota>();

        private int siguienteIdEstudiante = 1;
        private int siguienteIdMateria = 1;

        // Constantes
        public const double NotaMinima = 0;
        public const double NotaMaxima = 100;
        public const int GradoMinimo = 1;
        public const int GradoMaximo = 12;
        public const int CreditosMinimos = 1;
        public const int CreditosMaximos = 5;

        // --- CRUD basico ------------------------------------------------------
        public void AgregarEstudiante(string nombre, int grado)
        {
            estudiantes.Add(new Estudiante { Id = siguienteIdEstudiante++, Nombre = nombre, Grado = grado });
        }

        public bool EliminarEstudiante(int id)
        {
            Estudiante est = estudiantes.Find(e => e.Id == id);
            if (est == null) return false;
            estudiantes.Remove(est);
            notas.RemoveAll(n => n.EstudianteId == id);
            return true;
        }

        public void AgregarMateria(string nombre, int creditos)
        {
            materias.Add(new Materia { Id = siguienteIdMateria++, Nombre = nombre, Creditos = creditos });
        }

        public bool AgregarNota(int estudianteId, int materiaId, double calificacion)
        {
            if (!EstudianteExiste(estudianteId) || !MateriaExiste(materiaId))
                return false;
            // Evitar duplicado: un estudiante no puede tener dos notas en la misma materia
            if (notas.Exists(n => n.EstudianteId == estudianteId && n.MateriaId == materiaId))
                return false;
            notas.Add(new Nota { EstudianteId = estudianteId, MateriaId = materiaId, Calificacion = calificacion });
            return true;
        }

        // Nueva: eliminar una nota especifica
        public bool EliminarNota(int estudianteId, int materiaId)
        {
            Nota nota = notas.Find(n => n.EstudianteId == estudianteId && n.MateriaId == materiaId);
            if (nota == null) return false;
            notas.Remove(nota);
            return true;
        }

        public bool EstudianteExiste(int id) => estudiantes.Exists(e => e.Id == id);
        public bool MateriaExiste(int id) => materias.Exists(m => m.Id == id);

        public List<Estudiante> ObtenerEstudiantes() => estudiantes;
        public List<Materia> ObtenerMaterias() => materias;

        public Estudiante ObtenerEstudiante(int id) => estudiantes.Find(e => e.Id == id);
        public Materia ObtenerMateria(int id) => materias.Find(m => m.Id == id);
        public string ObtenerNombreEstudiante(int id) => ObtenerEstudiante(id)?.Nombre ?? "Desconocido";

        public List<Nota> ObtenerNotasPorEstudiante(int estudianteId) =>
            notas.FindAll(n => n.EstudianteId == estudianteId);

        public List<Nota> ObtenerNotasPorMateria(int materiaId) =>
            notas.FindAll(n => n.MateriaId == materiaId);

        public List<Estudiante> ObtenerEstudiantesPorGrado(int grado) =>
            estudiantes.FindAll(e => e.Grado == grado);

        // --- estadisticas de estudiante (con creditos) -----------------------
        public double CalcularPromedioEstudiante(int estudianteId)
        {
            var notasEst = ObtenerNotasPorEstudiante(estudianteId);
            if (notasEst.Count == 0) return 0;
            double suma = 0;
            foreach (var n in notasEst) suma += n.Calificacion;
            return suma / notasEst.Count;
        }

        // Promedio ponderado por creditos (GPA ponderado)
        public double CalcularPromedioPonderadoEstudiante(int estudianteId)
        {
            var notasEst = ObtenerNotasPorEstudiante(estudianteId);
            if (notasEst.Count == 0) return 0;
            double sumaPonderada = 0;
            int totalCreditos = 0;
            foreach (var n in notasEst)
            {
                Materia m = ObtenerMateria(n.MateriaId);
                if (m != null)
                {
                    sumaPonderada += n.Calificacion * m.Creditos;
                    totalCreditos += m.Creditos;
                }
            }
            if (totalCreditos == 0) return 0;
            return sumaPonderada / totalCreditos;
        }

        public double ObtenerNotaMaximaEstudiante(int estudianteId)
        {
            var notasEst = ObtenerNotasPorEstudiante(estudianteId);
            if (notasEst.Count == 0) return 0;
            double max = notasEst[0].Calificacion;
            foreach (var n in notasEst) if (n.Calificacion > max) max = n.Calificacion;
            return max;
        }

        public double ObtenerNotaMinimaEstudiante(int estudianteId)
        {
            var notasEst = ObtenerNotasPorEstudiante(estudianteId);
            if (notasEst.Count == 0) return 0;
            double min = notasEst[0].Calificacion;
            foreach (var n in notasEst) if (n.Calificacion < min) min = n.Calificacion;
            return min;
        }

        // GPA ponderado por creditos usando puntos GPA
        public double CalcularGpaEstudiante(int estudianteId)
        {
            var notasEst = ObtenerNotasPorEstudiante(estudianteId);
            if (notasEst.Count == 0) return 0;
            double sumaPuntos = 0;
            int totalCreditos = 0;
            foreach (var n in notasEst)
            {
                Materia m = ObtenerMateria(n.MateriaId);
                if (m != null)
                {
                    sumaPuntos += ConversorCalificacion.ObtenerPuntosGpa(n.Calificacion) * m.Creditos;
                    totalCreditos += m.Creditos;
                }
            }
            if (totalCreditos == 0) return 0;
            return sumaPuntos / totalCreditos;
        }

        public string ObtenerEstadoGeneralEstudiante(int estudianteId)
        {
            double promedio = CalcularPromedioEstudiante(estudianteId);
            if (promedio >= 90) return "Excelente";
            if (promedio >= 80) return "Muy bueno";
            if (promedio >= 70) return "Aprobado";
            return "Reprobado";
        }

        // --- estadisticas de materia -----------------------------------------
        public double CalcularPromedioMateria(int materiaId)
        {
            var notasMat = ObtenerNotasPorMateria(materiaId);
            if (notasMat.Count == 0) return 0;
            double suma = 0;
            foreach (var n in notasMat) suma += n.Calificacion;
            return suma / notasMat.Count;
        }

        public double ObtenerNotaMaximaMateria(int materiaId)
        {
            var notasMat = ObtenerNotasPorMateria(materiaId);
            if (notasMat.Count == 0) return 0;
            double max = notasMat[0].Calificacion;
            foreach (var n in notasMat) if (n.Calificacion > max) max = n.Calificacion;
            return max;
        }

        public double ObtenerNotaMinimaMateria(int materiaId)
        {
            var notasMat = ObtenerNotasPorMateria(materiaId);
            if (notasMat.Count == 0) return 0;
            double min = notasMat[0].Calificacion;
            foreach (var n in notasMat) if (n.Calificacion < min) min = n.Calificacion;
            return min;
        }

        public int ContarAprobadosMateria(int materiaId)
        {
            var notasMat = ObtenerNotasPorMateria(materiaId);
            int count = 0;
            foreach (var n in notasMat)
                if (ConversorCalificacion.EsAprobado(n.Calificacion)) count++;
            return count;
        }

        public int ContarReprobadosMateria(int materiaId) =>
            ObtenerNotasPorMateria(materiaId).Count - ContarAprobadosMateria(materiaId);
    }

    // --- interfaz visual en terminal -----------------------------------------
    public static class Ui
    {
        private const int Ancho = 60;
        private static ConsoleColor colorTitulo = ConsoleColor.Cyan;
        private static ConsoleColor colorExito = ConsoleColor.Green;
        private static ConsoleColor colorError = ConsoleColor.Red;
        private static ConsoleColor colorInfo = ConsoleColor.Yellow;
        private static ConsoleColor colorSubtexto = ConsoleColor.DarkGray;
        private static ConsoleColor colorNormal = ConsoleColor.White;

        public static void Limpiar() => Console.Clear();

        public static void LineaDoble()
        {
            Console.ForegroundColor = colorTitulo;
            Console.WriteLine(new string('=', Ancho));
            Console.ResetColor();
        }

        public static void LineaSimple()
        {
            Console.ForegroundColor = colorSubtexto;
            Console.WriteLine(new string('-', Ancho));
            Console.ResetColor();
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
            Limpiar();
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

        public static void Exito(string texto) => Escribir($"  + {texto}", colorExito);
        public static void Error(string texto) => Escribir($"  ! {texto}", colorError);
        public static void Info(string texto) => Escribir($"  > {texto}", colorInfo);

        private static void Escribir(string texto, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        public static void Fila(string etiqueta, string valor)
        {
            Console.ForegroundColor = colorSubtexto;
            Console.Write($"  {etiqueta,-22}: ");
            Console.ForegroundColor = colorNormal;
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
            Console.ForegroundColor = colorInfo;
            Console.Write($"  {texto}: ");
            Console.ForegroundColor = colorNormal;
        }

        public static string Leer() => Console.ReadLine() ?? "";

        public static void Separador() => Console.WriteLine();

        public static void OpcionMenu(string numero, string texto)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"  [{numero,-2}]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  {texto}");
            Console.ResetColor();
        }

        public static void MostrarEstudiante(Estudiante e, string extra = "")
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
                Console.ForegroundColor = colorInfo;
                Console.Write(extra);
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }

    // --- programa principal ---------------------------------------------------
    class Program
    {
        static GestorNotas gestor = new GestorNotas();

        static void Main(string[] args)
        {
            Ui.Pantalla("SISTEMA DE GESTION DE NOTAS ESCOLARES");
            Ui.Info("Literal: A(90-100) B(80-89) C(70-79) D(60-69) F(<60)");
            Ui.Info("Aprobado desde 70 | GPA maximo: 4.00 (ponderado por creditos)");
            Ui.Separador();
            Pausa();

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                Ui.EntradaPrompt("Opcion");
                string opcion = Ui.Leer().Trim();

                switch (opcion)
                {
                    case "1": AgregarEstudiante(); break;
                    case "2": AgregarMateria(); break;
                    case "3": AgregarNota(); break;
                    case "4": ListarEstudiantes(); break;
                    case "5": VerNotasEstudiante(); break;
                    case "6": ResumenPorMateria(); break;
                    case "7": EstadisticasEstudiante(); break;
                    case "8": EstadisticasGenerales(); break;
                    case "9": EstudiantesPorGrado(); break;
                    case "10": EliminarEstudiante(); break;
                    case "11": EliminarNota(); break;      // Nueva opcion
                    case "12": salir = true; break;
                    default: Ui.Error("Opcion invalida."); break;
                }

                if (!salir) Pausa();
            }

            Ui.Pantalla("Hasta luego");
        }

        static void MostrarMenu()
        {
            Ui.Pantalla("Menu Principal");
            Ui.OpcionMenu("1", "Agregar estudiante");
            Ui.OpcionMenu("2", "Agregar materia (con creditos)");
            Ui.OpcionMenu("3", "Agregar nota");
            Ui.OpcionMenu("4", "Ver todos los estudiantes");
            Ui.OpcionMenu("5", "Ver notas de un estudiante");
            Ui.OpcionMenu("6", "Resumen por materia");
            Ui.OpcionMenu("7", "Estadisticas de un estudiante");
            Ui.OpcionMenu("8", "Estadisticas generales");
            Ui.OpcionMenu("9", "Estudiantes por grado");
            Ui.OpcionMenu("10", "Eliminar estudiante");
            Ui.OpcionMenu("11", "Eliminar nota de estudiante");
            Ui.OpcionMenu("12", "Salir");
            Ui.LineaSimple();
        }

        static void Pausa()
        {
            Ui.Separador();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Presione Enter para continuar...");
            Console.ResetColor();
            Console.ReadLine();
        }

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
            foreach (var e in gestor.ObtenerEstudiantes())
                Ui.MostrarEstudiante(e);
            Ui.LineaSimple();
        }

        static void MostrarListaMaterias()
        {
            Ui.LineaSimple();
            foreach (var m in gestor.ObtenerMaterias())
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"  [{m.Id:D3}]");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  {m.Nombre} ({m.Creditos} cr)");
                Console.ResetColor();
            }
            Ui.LineaSimple();
        }

        // --- opciones del menu ------------------------------------------------
        static void AgregarEstudiante()
        {
            Ui.Titulo("Agregar estudiante");
            Ui.EntradaPrompt("Nombre");
            string nombre = Ui.Leer();
            if (string.IsNullOrWhiteSpace(nombre)) { Ui.Error("El nombre no puede estar vacio."); return; }

            if (!LeerEntero("Grado (1 a 12)", out int grado)) return;
            if (grado < GestorNotas.GradoMinimo || grado > GestorNotas.GradoMaximo)
            {
                Ui.Error($"El grado debe estar entre {GestorNotas.GradoMinimo} y {GestorNotas.GradoMaximo}.");
                return;
            }

            gestor.AgregarEstudiante(nombre, grado);
            Ui.Exito($"Estudiante {nombre} registrado en grado {grado}.");
        }

        static void AgregarMateria()
        {
            Ui.Titulo("Agregar materia");
            Ui.EntradaPrompt("Nombre");
            string nombre = Ui.Leer();
            if (string.IsNullOrWhiteSpace(nombre)) { Ui.Error("El nombre no puede estar vacio."); return; }

            if (!LeerEntero($"Creditos ({GestorNotas.CreditosMinimos} a {GestorNotas.CreditosMaximos})", out int creditos)) return;
            if (creditos < GestorNotas.CreditosMinimos || creditos > GestorNotas.CreditosMaximos)
            {
                Ui.Error($"Los creditos deben estar entre {GestorNotas.CreditosMinimos} y {GestorNotas.CreditosMaximos}.");
                return;
            }

            gestor.AgregarMateria(nombre, creditos);
            Ui.Exito($"Materia '{nombre}' con {creditos} creditos agregada.");
        }

        static void AgregarNota()
        {
            Ui.Titulo("Agregar nota");

            if (gestor.ObtenerEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }
            if (gestor.ObtenerMaterias().Count == 0) { Ui.Error("No hay materias registradas."); return; }

            Ui.Info("Estudiantes:");
            MostrarListaEstudiantes();
            if (!LeerEntero("ID del estudiante", out int estudianteId)) return;
            if (!gestor.EstudianteExiste(estudianteId)) { Ui.Error("El estudiante no existe."); return; }

            Ui.Info("Materias:");
            MostrarListaMaterias();
            if (!LeerEntero("ID de la materia", out int materiaId)) return;
            if (!gestor.MateriaExiste(materiaId)) { Ui.Error("La materia no existe."); return; }

            // Verificar si ya tiene nota en esa materia
            var notasExistentes = gestor.ObtenerNotasPorEstudiante(estudianteId);
            if (notasExistentes.Exists(n => n.MateriaId == materiaId))
            {
                Ui.Error("El estudiante ya tiene una nota en esta materia. Use la opcion de eliminar nota si desea cambiarla.");
                return;
            }

            if (!LeerDecimal("Nota (0 a 100)", out double nota)) return;
            if (nota < GestorNotas.NotaMinima || nota > GestorNotas.NotaMaxima)
            {
                Ui.Error($"La nota debe estar entre {GestorNotas.NotaMinima} y {GestorNotas.NotaMaxima}.");
                return;
            }

            if (gestor.AgregarNota(estudianteId, materiaId, nota))
            {
                string literal = ConversorCalificacion.ObtenerLiteral(nota);
                string estado = ConversorCalificacion.EsAprobado(nota) ? "aprobado" : "reprobado";
                Ui.Exito($"Nota {nota} -> Literal: {literal} | {estado}.");
            }
            else Ui.Error("Error al agregar la nota.");
        }

        static void ListarEstudiantes()
        {
            Ui.Titulo("Todos los estudiantes");
            var estudiantes = gestor.ObtenerEstudiantes();
            if (estudiantes.Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            Ui.Fila("Total", $"{estudiantes.Count}");
            Ui.LineaSimple();

            foreach (var e in estudiantes)
            {
                var notas = gestor.ObtenerNotasPorEstudiante(e.Id);
                string extra = "sin notas";
                if (notas.Count > 0)
                {
                    double prom = gestor.CalcularPromedioEstudiante(e.Id);
                    extra = $"Prom: {prom:F1}  Lit: {ConversorCalificacion.ObtenerLiteral(prom)}  GPA: {gestor.CalcularGpaEstudiante(e.Id):F2}";
                }
                Ui.MostrarEstudiante(e, extra);
            }
            Ui.LineaSimple();
        }

        static void VerNotasEstudiante()
        {
            Ui.Titulo("Notas de estudiante");
            if (gestor.ObtenerEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            MostrarListaEstudiantes();
            if (!LeerEntero("ID del estudiante", out int id)) return;
            if (!gestor.EstudianteExiste(id)) { Ui.Error("El estudiante no existe."); return; }

            var notas = gestor.ObtenerNotasPorEstudiante(id);
            var est = gestor.ObtenerEstudiante(id);

            Ui.Separador();
            Ui.Fila("Estudiante", est.Nombre);
            Ui.Fila("Grado", $"{est.Grado}");
            Ui.LineaSimple();

            if (notas.Count == 0) { Ui.Info("Sin notas registradas."); return; }

            foreach (var n in notas)
            {
                var mat = gestor.ObtenerMateria(n.MateriaId);
                string nombreMat = mat != null ? $"{mat.Nombre} ({mat.Creditos} cr)" : "Desconocida";
                string literal = ConversorCalificacion.ObtenerLiteral(n.Calificacion);
                bool aprobado = ConversorCalificacion.EsAprobado(n.Calificacion);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  {nombreMat,-30}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"  {n.Calificacion,6:F2}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  ({literal})");
                Console.ForegroundColor = aprobado ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  {(aprobado ? "aprobado" : "reprobado")}");
                Console.ResetColor();
            }
        }

        static void ResumenPorMateria()
        {
            Ui.Titulo("Resumen por materia");
            if (gestor.ObtenerMaterias().Count == 0) { Ui.Error("No hay materias registradas."); return; }

            MostrarListaMaterias();
            if (!LeerEntero("ID de la materia", out int id)) return;
            var materia = gestor.ObtenerMateria(id);
            if (materia == null) { Ui.Error("La materia no existe."); return; }

            var notas = gestor.ObtenerNotasPorMateria(id);
            if (notas.Count == 0) { Ui.Info("No hay notas registradas en esta materia."); return; }

            double promedio = gestor.CalcularPromedioMateria(id);
            double max = gestor.ObtenerNotaMaximaMateria(id);
            double min = gestor.ObtenerNotaMinimaMateria(id);
            int aprobados = gestor.ContarAprobadosMateria(id);
            int reprobados = gestor.ContarReprobadosMateria(id);

            Ui.Fila("Materia", $"{materia.Nombre} ({materia.Creditos} creditos)");
            Ui.Fila("Promedio del grupo", $"{promedio:F2}  ({ConversorCalificacion.ObtenerLiteral(promedio)})");
            Ui.Fila("Nota mas alta", $"{max:F2}");
            Ui.Fila("Nota mas baja", $"{min:F2}");
            Ui.Fila("Total estudiantes", $"{notas.Count}");
            Ui.Fila("Aprobados", $"{aprobados}");
            Ui.Fila("Reprobados", $"{reprobados}");
            Ui.LineaSimple();

            Ui.Info("Detalle por estudiante:");
            Ui.LineaSimple();
            foreach (var n in notas)
            {
                var est = gestor.ObtenerEstudiante(n.EstudianteId);
                string nombre = est != null ? $"{est.Nombre} (G{est.Grado})" : "Desconocido";
                string literal = ConversorCalificacion.ObtenerLiteral(n.Calificacion);
                bool aprobado = ConversorCalificacion.EsAprobado(n.Calificacion);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  {nombre,-30}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"  {n.Calificacion,6:F2}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  ({literal})");
                Console.ForegroundColor = aprobado ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  {(aprobado ? "aprobado" : "reprobado")}");
                Console.ResetColor();
            }
        }

        static void EstadisticasEstudiante()
        {
            Ui.Titulo("Estadisticas de estudiante");
            if (gestor.ObtenerEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            MostrarListaEstudiantes();
            if (!LeerEntero("ID del estudiante", out int id)) return;
            if (!gestor.EstudianteExiste(id)) { Ui.Error("El estudiante no existe."); return; }

            var notas = gestor.ObtenerNotasPorEstudiante(id);
            if (notas.Count == 0) { Ui.Info("El estudiante no tiene notas registradas."); return; }

            string nombre = gestor.ObtenerNombreEstudiante(id);
            double promedioSimple = gestor.CalcularPromedioEstudiante(id);
            double promedioPonderado = gestor.CalcularPromedioPonderadoEstudiante(id);
            double gpa = gestor.CalcularGpaEstudiante(id);
            string literal = ConversorCalificacion.ObtenerLiteral(promedioSimple);
            string estado = gestor.ObtenerEstadoGeneralEstudiante(id);
            double max = gestor.ObtenerNotaMaximaEstudiante(id);
            double min = gestor.ObtenerNotaMinimaEstudiante(id);

            Ui.Fila("Estudiante", nombre);
            Ui.Fila("Promedio simple", $"{promedioSimple:F2}");
            Ui.Fila("Promedio ponderado (por creditos)", $"{promedioPonderado:F2}");
            Ui.Fila("Literal", literal);
            Ui.Fila("GPA ponderado", $"{gpa:F2} / 4.00");
            Ui.Fila("Nota maxima", $"{max:F2}");
            Ui.Fila("Nota minima", $"{min:F2}");
            Ui.Fila("Materias cursadas", $"{notas.Count}");
            Ui.FilaEstado("Estado", estado, ConversorCalificacion.EsAprobado(promedioSimple));
            Ui.LineaSimple();
        }

        static void EstadisticasGenerales()
        {
            Ui.Titulo("Estadisticas generales del colegio");
            var estudiantes = gestor.ObtenerEstudiantes();
            if (estudiantes.Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            int totalConNotas = 0;
            int aprobados = 0, reprobados = 0;
            double mejorPromedio = -1, peorPromedio = 101;
            string mejorEstudiante = "", peorEstudiante = "";
            double sumaGpas = 0;

            foreach (var e in estudiantes)
            {
                var notas = gestor.ObtenerNotasPorEstudiante(e.Id);
                if (notas.Count == 0) continue;
                totalConNotas++;
                double prom = gestor.CalcularPromedioEstudiante(e.Id);
                double gpa = gestor.CalcularGpaEstudiante(e.Id);
                sumaGpas += gpa;

                if (ConversorCalificacion.EsAprobado(prom)) aprobados++;
                else reprobados++;

                if (prom > mejorPromedio)
                {
                    mejorPromedio = prom;
                    mejorEstudiante = $"{e.Nombre} (Grado {e.Grado})";
                }
                if (prom < peorPromedio)
                {
                    peorPromedio = prom;
                    peorEstudiante = $"{e.Nombre} (Grado {e.Grado})";
                }
            }

            Ui.Fila("Total estudiantes", $"{estudiantes.Count}");
            Ui.Fila("Con notas", $"{totalConNotas}");
            Ui.Fila("Aprobados", $"{aprobados}");
            Ui.Fila("Reprobados", $"{reprobados}");

            if (totalConNotas > 0)
            {
                Ui.Fila("GPA promedio colegio (ponderado)", $"{(sumaGpas / totalConNotas):F2} / 4.00");
                Ui.LineaSimple();
                Ui.Fila("Mejor promedio", $"{mejorEstudiante}  ({mejorPromedio:F2})");
                Ui.Fila("Menor promedio", $"{peorEstudiante}  ({peorPromedio:F2})");
            }
            Ui.LineaSimple();

            Ui.Info("Promedio por materia:");
            foreach (var m in gestor.ObtenerMaterias())
            {
                var notas = gestor.ObtenerNotasPorMateria(m.Id);
                if (notas.Count == 0)
                    Ui.Fila($"{m.Nombre} ({m.Creditos} cr)", "sin notas");
                else
                {
                    double prom = gestor.CalcularPromedioMateria(m.Id);
                    Ui.Fila($"{m.Nombre} ({m.Creditos} cr)", $"{prom:F2}  ({ConversorCalificacion.ObtenerLiteral(prom)})");
                }
            }
        }

        static void EstudiantesPorGrado()
        {
            Ui.Titulo("Estudiantes por grado");
            if (!LeerEntero("Grado (1 a 12)", out int grado)) return;
            if (grado < GestorNotas.GradoMinimo || grado > GestorNotas.GradoMaximo)
            {
                Ui.Error($"El grado debe estar entre {GestorNotas.GradoMinimo} y {GestorNotas.GradoMaximo}.");
                return;
            }

            var lista = gestor.ObtenerEstudiantesPorGrado(grado);
            if (lista.Count == 0) { Ui.Info($"No hay estudiantes en grado {grado}."); return; }

            Ui.Fila("Grado", $"{grado}");
            Ui.Fila("Estudiantes", $"{lista.Count}");
            Ui.LineaSimple();

            foreach (var e in lista)
            {
                var notas = gestor.ObtenerNotasPorEstudiante(e.Id);
                string extra = "sin notas";
                if (notas.Count > 0)
                {
                    double prom = gestor.CalcularPromedioEstudiante(e.Id);
                    extra = $"Prom: {prom:F1}  Lit: {ConversorCalificacion.ObtenerLiteral(prom)}  GPA: {gestor.CalcularGpaEstudiante(e.Id):F2}";
                }
                Ui.MostrarEstudiante(e, extra);
            }
        }

        static void EliminarEstudiante()
        {
            Ui.Titulo("Eliminar estudiante");
            if (gestor.ObtenerEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }

            MostrarListaEstudiantes();
            if (!LeerEntero("ID del estudiante a eliminar", out int id)) return;
            if (!gestor.EstudianteExiste(id)) { Ui.Error("El estudiante no existe."); return; }

            string nombre = gestor.ObtenerNombreEstudiante(id);
            Ui.EntradaPrompt($"Confirmar eliminacion de '{nombre}' (s/n)");
            string confirm = Ui.Leer().Trim().ToLower();
            if (confirm != "s") { Ui.Info("Operacion cancelada."); return; }

            if (gestor.EliminarEstudiante(id))
                Ui.Exito($"'{nombre}' eliminado junto con sus notas.");
            else
                Ui.Error("No se pudo eliminar.");
        }

        static void EliminarNota()
        {
            Ui.Titulo("Eliminar nota de estudiante");
            if (gestor.ObtenerEstudiantes().Count == 0) { Ui.Error("No hay estudiantes registrados."); return; }
            if (gestor.ObtenerMaterias().Count == 0) { Ui.Error("No hay materias registradas."); return; }

            MostrarListaEstudiantes();
            if (!LeerEntero("ID del estudiante", out int estudianteId)) return;
            if (!gestor.EstudianteExiste(estudianteId)) { Ui.Error("El estudiante no existe."); return; }

            var notas = gestor.ObtenerNotasPorEstudiante(estudianteId);
            if (notas.Count == 0) { Ui.Info("El estudiante no tiene notas registradas."); return; }

            Ui.Info("Notas actuales del estudiante:");
            Ui.LineaSimple();
            foreach (var n in notas)
            {
                var mat = gestor.ObtenerMateria(n.MateriaId);
                string nombreMat = mat != null ? mat.Nombre : "Desconocida";
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  Materia: {nombreMat} (ID: {n.MateriaId})");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" - Nota: {n.Calificacion:F2}");
                Console.ResetColor();
            }
            Ui.LineaSimple();

            if (!LeerEntero("ID de la materia de la nota a eliminar", out int materiaId)) return;
            if (!gestor.MateriaExiste(materiaId)) { Ui.Error("La materia no existe."); return; }

            // Verificar que el estudiante tenga nota en esa materia
            if (!notas.Exists(n => n.MateriaId == materiaId))
            {
                Ui.Error("El estudiante no tiene nota en esa materia.");
                return;
            }

            Ui.EntradaPrompt("¿Esta seguro de eliminar esta nota? (s/n)");
            string confirm = Ui.Leer().Trim().ToLower();
            if (confirm != "s") { Ui.Info("Operacion cancelada."); return; }

            if (gestor.EliminarNota(estudianteId, materiaId))
                Ui.Exito("Nota eliminada correctamente.");
            else
                Ui.Error("No se pudo eliminar la nota.");
        }
    }
}