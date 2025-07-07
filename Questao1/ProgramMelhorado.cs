/*
    Sugestão de melhorias para o codigo de param, onde temos 

    - Validação de entrada para evitar exceções e erros inesperados
    - Métodos auxiliares para reuso e clareza de propósito
    - Interpolação de string para ter um código mais moderno e legível
    - Organização codigo em etapas para facilitar manutenção e leitura
*/

using System;
using System.Globalization;

namespace Questao1 {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("=== Cadastro de Conta Bancária ===");

            int numeroConta = LerInt("Entre o número da conta: ");
            string titularConta = LerTexto("Entre o titular da conta: ");
            char respostaDeposito = LerChar("Haverá depósito inicial (s/n)? ");

            ContaBancaria conta;

            if (char.ToLower(respostaDeposito) == 's') {
                double depositoInicial = LerDouble("Entre o valor de depósito inicial: ");
                conta = new ContaBancaria(numeroConta, titularConta, depositoInicial);
            } else {
                conta = new ContaBancaria(numeroConta, titularConta);
            }

            MostrarDadosConta("Dados da conta:", conta);

            double valorDeposito = LerDouble("Entre um valor para depósito: ");
            conta.Deposito(valorDeposito);
            MostrarDadosConta("Dados da conta atualizados:", conta);

            double valorSaque = LerDouble("Entre um valor para saque: ");
            conta.Saque(valorSaque);
            MostrarDadosConta("Dados da conta atualizados:", conta);
        }

        static int LerInt(string mensagem) {
            Console.Write(mensagem);
            while (!int.TryParse(Console.ReadLine(), out int valor)) {
                Console.Write("Valor inválido. Tente novamente: ");
            }
            return valor;
        }

        static double LerDouble(string mensagem) {
            Console.Write(mensagem);
            while (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double valor)) {
                Console.Write("Valor inválido. Tente novamente: ");
            }
            return valor;
        }

        static string LerTexto(string mensagem) {
            Console.Write(mensagem);
            return Console.ReadLine();
        }

        static char LerChar(string mensagem) {
            Console.Write(mensagem);
            while (!char.TryParse(Console.ReadLine(), out char c)) {
                Console.Write("Entrada inválida. Tente novamente: ");
            }
            return c;
        }

        static void MostrarDadosConta(string titulo, ContaBancaria conta) {
            Console.WriteLine();
            Console.WriteLine(titulo);
            Console.WriteLine(conta);
        }
    }
}
