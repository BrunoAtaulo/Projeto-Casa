namespace CasaDeShow.Models
{
    public class ListaCompra
    {
        public int Id { get; set; }
        public Compra Compra { get; set; }
        public Evento Evento { get; set; }
        public int Qtd { get; set; }
    }
}