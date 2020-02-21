using System;
using System.ComponentModel.DataAnnotations;

namespace CasaDeShow.Models
{
    public class Casadeshow
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage="Compo nome da casa de show necessário.")]
        public string Nome { get; set; }
        [Required(ErrorMessage="Campo endereço da casa de show necessário.")]
        public string Endereco { get; set; }

    }
}