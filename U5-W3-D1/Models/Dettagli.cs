namespace U5_W3_D1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dettagli")]
    public partial class Dettagli
    {
        [Key]
        public int idDettagli { get; set; }

        public int idOrdini_FK { get; set; }

        public int idProdotto_FK { get; set; }

        [NotMapped]
        public int Quantità { get; set; }

        public virtual Ordini Ordini { get; set; }

        public virtual Prodotti Prodotti { get; set; }
    }
}
