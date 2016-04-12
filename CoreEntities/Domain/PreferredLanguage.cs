

namespace Core.Domain
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   public partial class PreferredLanguage : BaseEntity
    {
       [Key]
       public int LanguageId { get; set; }
       public string LanguageName { get; set; }
    }
}
