using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecommerce_iniciativatena.Model
{
    public class Produto
    {
        [Key] // Primary Keu (Id)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // IDENTITY (1,1)
        public long Id { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        [Column(TypeName = "Varchar")]
        [StringLength(255)]
        public string? Duracao { get; set; }

        [Column(TypeName = "Decimal(8,2)")]
        public string Valor { get; set; } = string.Empty;

        [Column(TypeName = "Int")]
        public int Quantidade { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(5000)]
        public string Foto { get; set; } = string.Empty;

        public virtual Categoria? Categoria { get; set; }
    }
}
