using System;
using Newtonsoft.Json;
using Serilog;

class Program
{
    static void Main()
    {
        // Configurar o log
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Console.WriteLine("Conversor de Moeda - Real para Dólar");

            Console.Write("Digite o valor em reais (R$): ");
            string entrada = Console.ReadLine();

            if (decimal.TryParse(entrada, out decimal valorEmReais))
            {
                decimal cotacaoDolar = 5.20m; // Valor fictício
                decimal valorEmDolar = valorEmReais / cotacaoDolar;

                Console.WriteLine($"R$ {valorEmReais} equivalem a $ {valorEmDolar:F2} dólares.");

                // Gerar um JSON de exemplo
                var resultado = new
                {
                    ValorEmReais = valorEmReais,
                    ValorEmDolar = valorEmDolar,
                    Cotacao = cotacaoDolar,
                    Data = DateTime.Now
                };

                string json = JsonConvert.SerializeObject(resultado, Formatting.Indented);
                Console.WriteLine("\nJSON do resultado:\n" + json);

                // Logar no arquivo
                Log.Information("Conversão realizada: {Json}", json);
            }
            else
            {
                Console.WriteLine("Valor inválido.");
                Log.Error("Entrada inválida: {Entrada}", entrada);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro: " + ex.Message);
            Log.Error(ex, "Erro durante a execução");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        Console.WriteLine("\nPressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}

