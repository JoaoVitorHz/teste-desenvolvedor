﻿using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        private const double TaxaSaque = 3.50;

        public int Numero { get; }
        public string Titular { get; set; } 
        public double Saldo { get; private set; } 

        public ContaBancaria(int numero, string titular, double depositoInicial){
            Numero = numero;
            Titular = titular;
            Deposito(depositoInicial);
        }

        public ContaBancaria(int numero, string titular){
            Numero = numero;
            Titular = titular;
            Saldo = 0.0;
        }

        public void Deposito(double valor){
            Saldo += valor;
        }

        public void Saque(double valor){
            Saldo -= (valor + TaxaSaque);
        }

        public override string ToString(){
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}
