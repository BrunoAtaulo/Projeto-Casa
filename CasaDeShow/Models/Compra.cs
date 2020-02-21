using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CasaDeShow.Models
{
    public class Compra
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Campo nome do evento necessário.", AllowEmptyStrings = false)]
        public string NomeEvento { get; set; }

        [Range(10, 200000, ErrorMessage = "Campo capacidade inválido.")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "Campo Data com hora do evento necessário.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public System.DateTime Data { get; set; }

        [Required(ErrorMessage = "Campo valor do ingresso necessário.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###,##0.00}")]
        public double ValorIngresso { get; set; }

        [Required(ErrorMessage = "Campo gênero da música necessário.")]
        public string GeneroMusica { get; set; }
         public int CasadeshowId { get; set; }
        //public Casadeshow Casadeshow { get; set; }
        public string NomeCasa { get; set; }
        public int Quantidade { get; set; }
        public int IngressosRestantes { get; set; }
        public string IdentityUser { get; set; }
    }
}