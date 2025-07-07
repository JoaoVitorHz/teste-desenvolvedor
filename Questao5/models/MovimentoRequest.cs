public class MovimentoRequest{
    public string IdMovimento { get; set; }
    public string IdContaCorrente { get; set; }
    public decimal Valor { get; set; }
    public string TipoMovimento { get; set; } // "C" ou "D"
}