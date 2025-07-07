using Dapper;
using Microsoft.AspNetCore.Mvc;
using Questao5.Infrastructure.Sqlite;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Questao5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly DatabaseConfig _config;

        public ContaCorrenteController(DatabaseConfig config)
        {
            _config = config;
        }

        [HttpPost("movimento")]
        public async Task<IActionResult> PostMovimento([FromBody] MovimentoRequest req)
        {
            if (req.Valor <= 0)
                return BadRequest(new { message = "Valor deve ser positivo.", type = "INVALID_VALUE" });

            if (req.TipoMovimento != "C" && req.TipoMovimento != "D")
                return BadRequest(new { message = "Tipo de movimento inválido.", type = "INVALID_TYPE" });

            using var conn = new SQLiteConnection(_config.Name);
            var conta = await conn.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = req.IdContaCorrente });

            if (conta == null)
                return BadRequest(new { message = "Conta inexistente.", type = "INVALID_ACCOUNT" });

            if (conta.Ativo != 1)
                return BadRequest(new { message = "Conta inativa.", type = "INACTIVE_ACCOUNT" });

            var sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                        VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
            
            var mov = new Movimento
            {
                IdMovimento = req.IdMovimento,
                IdContaCorrente = req.IdContaCorrente,
                DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                TipoMovimento = req.TipoMovimento,
                Valor = req.Valor
            };

            await conn.ExecuteAsync(sql, mov);

            return Ok(new { IdMovimento = req.IdMovimento });
        }

        [HttpGet("saldo/{id}")]
        public async Task<IActionResult> GetSaldo(string id)
        {
            using var conn = new SQLiteConnection(_config.Name);
            var conta = await conn.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = id });

            if (conta == null)
                return BadRequest(new { message = "Conta inexistente.", type = "INVALID_ACCOUNT" });

            if (conta.Ativo != 1)
                return BadRequest(new { message = "Conta inativa.", type = "INACTIVE_ACCOUNT" });

            var creditos = await conn.ExecuteScalarAsync<decimal?>(
                "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'C'", new { Id = id }) ?? 0;

            var debitos = await conn.ExecuteScalarAsync<decimal?>(
                "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'D'", new { Id = id }) ?? 0;

            var saldo = creditos - debitos;

            var result = new SaldoResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataConsulta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Saldo = saldo
            };

            return Ok(result);
        }
    }
}
