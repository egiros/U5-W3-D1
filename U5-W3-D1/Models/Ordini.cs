namespace U5_W3_D1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordini()
        {
            Dettagli = new HashSet<Dettagli>();
        }

        [Key]
        public int idOrdine { get; set; }

        [Required]
        [StringLength(200)]
        public string Indirizzo { get; set; }

        public int idUtente_FK { get; set; }

        [Column(TypeName = "money")]
        public decimal Totale { get; set; }

        public bool isEvaso { get; set; }

        public string Note { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataOrdine { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dettagli> Dettagli { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
