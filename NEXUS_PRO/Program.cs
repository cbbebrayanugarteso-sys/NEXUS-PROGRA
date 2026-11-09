using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NEXUS
{
    internal class Program
    {
        // Evento principal para interrupciones críticas
        public static event Action<string> NotificarAlertaCritica;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Subscripción a las alertas del sistema
            NotificarAlertaCritica += ProcesarAlertaCritica;

            IniciarSimulacionNEXUS();
        }

        static void ProcesarAlertaCritica(string motivo)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n┌────────────────────────────────────────────────────────┐");
            Console.WriteLine("│ ⚡ ALERTA DE SISTEMA: ANOMALÍA CRÍTICA EN RED          │");
            Console.WriteLine($"│ {motivo.PadRight(54)} │");
            Console.WriteLine("└────────────────────────────────────────────────────────┘");
            Console.ResetColor();
        }

        // ==========================================
        //  SISTEMA DE REGISTRO Y BUCLE PRINCIPAL
        // ==========================================
        static void IniciarSimulacionNEXUS()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("──────────────────────────────────────────────────────────");
            Console.WriteLine("        NEXUS // REGISTRO Y CONTROL DE CADETES            ");
            Console.WriteLine("──────────────────────────────────────────────────────────");
            Console.ResetColor();

            // 1. Ingreso y validación de parámetros del cadete
            Console.Write("Nombre del Cadete: ");
            string cadete = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cadete)) cadete = "Cadete Desconocido";

            int edad = CapturarEnteroValido("Edad del Cadete (18-60): ", 18, 60);

            Console.WriteLine("\nRealidades disponibles para inmersión:");
            Console.WriteLine("[1] IRIS-07 // Valle de Cristal");
            Console.WriteLine("[2] IRIS-12 // Sector Abisal");
            Console.WriteLine("[3] IRIS-21 // Zona de Interferencia");
            int seleccionRealidad = CapturarEnteroValido("Seleccione Realidad (1-3): ", 1, 3);
            string realidadAsignada = seleccionRealidad == 1 ? "IRIS-07" : (seleccionRealidad == 2 ? "IRIS-12" : "IRIS-21");

            int nivelEnergia = CapturarEnteroValido("Nivel Inicial de Energía (1-100): ", 1, 100);
            int nivelEstabilidad = CapturarEnteroValido("Nivel Inicial de Estabilidad (1-100): ", 1, 100);

            // 2. Validación de criterios para inmersión
            if (edad < 18 || nivelEnergia < 20 || nivelEstabilidad < 20)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n✖ [INMERSIÓN RECHAZADA] Las constantes biológicas o los parámetros de red son insuficientes.");
                Console.ResetColor();
                Console.WriteLine("Presione ENTER para salir...");
                Console.ReadLine();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n✔ [INMERSIÓN AUTORIZADA] Sincronización exitosa. Conectando a NEXUS...");
            Console.ResetColor();
            Thread.Sleep(1200);

            // 3. Ciclo principal de control
            bool estaConectado = true;
            while (estaConectado)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("┌────────────────────────────────────────────────────────┐");
                Console.WriteLine("│                  NEXUS TRAINING SYSTEM                 │");
                Console.WriteLine("└────────────────────────────────────────────────────────┘");
                Console.ResetColor();
                Console.WriteLine($"CADETE     : {cadete} (Edad: {edad})");
                Console.WriteLine($"REALIDAD   : {realidadAsignada}");
                Console.WriteLine($"ENERGÍA    : {nivelEnergia}/100");
                Console.WriteLine($"ESTABILIDAD: {nivelEstabilidad}/100");
                Console.WriteLine("──────────────────────────────────────────────────────────");
                Console.WriteLine(" [1] Explorar Realidad (Visualizar Cartografía)");
                Console.WriteLine(" [2] Analizar Anomalía IRIS");
                Console.WriteLine(" [3] Calibrar Estabilidad");
                Console.WriteLine(" [4] Recuperar Energía (Recarga de Núcleo)");
                Console.WriteLine(" [5] Servidor IRIS (Transmitir Mapa TCP)");
                Console.WriteLine(" [6] Intentar Desconexión");
                Console.WriteLine("──────────────────────────────────────────────────────────");

                int opcionSeleccionada = CapturarEnteroValido("Opción deseada (1-6) > ", 1, 6);

                switch (opcionSeleccionada)
                {
                    case 1:
                        if (nivelEnergia < 15)
                        {
                            Console.WriteLine("\n❌ Energía insuficiente para desplegar telemetría gráfica.");
                        }
                        else
                        {
                            nivelEnergia -= 15;
                            nivelEstabilidad -= (nivelEstabilidad < 30) ? 20 : 10;
                            DesplegarMapaCartografico("IRIS-07 // Valle de Cristal", ObtenerMatrizValle());
                        }
                        break;

                    case 2:
                        if (nivelEnergia < 10)
                        {
                            Console.WriteLine("\n❌ Nivel de energía insuficiente para realizar el escaneo.");
                        }
                        else
                        {
                            nivelEnergia -= 10;
                            nivelEstabilidad -= 15;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\n[ANÁLISIS]: Frecuencia IRIS detectada. Se identificaron resonancias de datos en el sector.");
                            Console.ResetColor();
                        }
                        break;

                    case 3:
                        if (nivelEnergia < 20)
                        {
                            Console.WriteLine("\n❌ Requiere al menos 20 de Energía para estabilizar la línea.");
                        }
                        else
                        {
                            nivelEnergia -= 20;
                            nivelEstabilidad = Math.Min(100, nivelEstabilidad + 30);
                            Console.WriteLine("\n✔️ Estabilidad de señal recalibrada exitosamente.");
                        }
                        break;

                    case 4:
                        nivelEnergia = Math.Min(100, nivelEnergia + 35);
                        nivelEstabilidad -= 5;
                        Console.WriteLine("\n⚡ Núcleo cargado. +35 de Energía. -5% de Estabilidad por sobrecarga térmica.");
                        break;

                    case 5:
                        IniciarServidorNEXUS();
                        break;

                    case 6:
                        estaConectado = false;
                        Console.WriteLine("\nCerrando enlace neural de forma segura...");
                        break;
                }

                // Evaluar evento de emergencia
                if (estaConectado && (nivelEstabilidad <= 15 || (nivelEnergia <= 10 && nivelEstabilidad <= 25)))
                {
                    NotificarAlertaCritica?.Invoke("ALERTA DE INTERFERENCIA IRIS: Colapso inminente de la realidad.");
                    estaConectado = false;
                }

                if (estaConectado)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nSesión de entrenamiento finalizada.");
            Console.ResetColor();
        }

        // Método auxiliar de validación de entradas
        static int CapturarEnteroValido(string indicador, int min, int max)
        {
            int valor;
            while (true)
            {
                Console.Write(indicador);
                string textoIngresado = Console.ReadLine();

                if (int.TryParse(textoIngresado, out valor) && valor >= min && valor <= max)
                {
                    return valor;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR]: Entrada inválida. Ingrese un número entero entre {min} y {max}.");
                Console.ResetColor();
            }
        }

        // ==========================================
        //  TRANSMISIÓN RED TCP (SERVIDOR NEXUS)
        // ==========================================
        static void IniciarServidorNEXUS()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("──────────────────────────────────────────────────────────");
            Console.WriteLine("          NEXUS - SERVIDOR DE CARTOGRAFÍA IRIS            ");
            Console.WriteLine("──────────────────────────────────────────────────────────");
            Console.ResetColor();

            try
            {
                TcpListener servidorTcp = new TcpListener(IPAddress.Any, 5000);
                servidorTcp.Start();

                Console.WriteLine("\n[SERVIDOR ACTIVO] Esperando la conexión de otro NEXUS en el puerto 5000...");
                TcpClient clienteConectado = servidorTcp.AcceptTcpClient();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n¡NEXUS remoto conectado con éxito!");
                Console.ResetColor();

                StreamWriter flujoEscritura = new StreamWriter(clienteConectado.GetStream());
                int[,] matrizEnviada = ObtenerMatrizValle();
                string nombreSector = "IRIS-07 // Valle de Cristal";

                flujoEscritura.WriteLine(nombreSector);

                int lineas = matrizEnviada.GetLength(0);
                int columnas = matrizEnviada.GetLength(1);

                for (int f = 0; f < lineas; f++)
                {
                    string lineaProcesada = "";
                    for (int c = 0; c < columnas; c++)
                    {
                        lineaProcesada += matrizEnviada[f, c] + (c == columnas - 1 ? "" : ",");
                    }
                    flujoEscritura.WriteLine(lineaProcesada);
                    Console.WriteLine($"Transmitiendo Fila {f}: [{lineaProcesada}]");
                    Thread.Sleep(100);
                }

                flujoEscritura.WriteLine("FIN");
                flujoEscritura.Flush();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n[ÉXITO] Cartografía transmitida correctamente.");
                Console.ResetColor();

                clienteConectado.Close();
                servidorTcp.Stop();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR DE RED]: {ex.Message}");
                Console.ResetColor();
            }
        }

        // ==========================================
        //  MATRIZ DE DATOS (VALLE DE CRISTAL)
        // ==========================================
        static int[,] ObtenerMatrizValle()
        {
            return new int[,]
            {
                { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 2, 0, 1, 3, 1, 0, 5, 5, 0 },
                { 0, 0, 0, 1, 1, 1, 0, 5, 5, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 3, 1, 0, 0, 0, 4, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 3, 0, 0 },
                { 0, 5, 5, 0, 0, 0, 0, 0, 0, 0 }
            };
        }

        // ==========================================
        //  VISUALIZACIÓN Y DIBUJO DE MAPA IRIS
        // ==========================================
        static void DesplegarMapaCartografico(string etiquetaRealidad, int[,] datosMapa)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"CARGANDO DATOS TELEMÉTRICOS DE [{etiquetaRealidad}]...");
            Thread.Sleep(400);
            Console.Clear();

            int filas = datosMapa.GetLength(0);
            int columnas = datosMapa.GetLength(1);
            int totalCeldas = filas * columnas;
            int celdasMapeadas = 0;

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ REALIDAD: {etiquetaRealidad.PadRight(47)} ║");
            Console.WriteLine($"╠══════════════════════════════════════════════════════════╣");

            Console.Write("║       ");
            for (int c = 0; c < columnas; c++)
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine("                                 ║");

            for (int f = 0; f < filas; f++)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write($"║ [{f}]   ");

                for (int c = 0; c < columnas; c++)
                {
                    int elemento = datosMapa[f, c];

                    switch (elemento)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("░ ");
                            celdasMapeadas++;
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("█ ");
                            celdasMapeadas++;
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("▲ ");
                            celdasMapeadas++;
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("★ ");
                            celdasMapeadas++;
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("✖ ");
                            celdasMapeadas++;
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("◉ ");
                            celdasMapeadas++;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("? ");
                            break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("                                 ║");
            }

            double calculoPorcentaje = ((double)celdasMapeadas / totalCeldas) * 100;

            Console.WriteLine($"╠══════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Leyenda: ▲ Base  ★ Recurso  ✖ Anomalía  ◉ Interés  ░ Libre║");
            Console.WriteLine($"║ COORDENADAS: [0,0] a [{filas - 1},{columnas - 1}] | CARTOGRAFÍA: {calculoPorcentaje:F0}%           ║");
            Console.WriteLine($"╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }
    }
}