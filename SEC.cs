//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SEC_Control
{
    using System;
    using System.Collections.Generic;
    
    public partial class SEC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SEC()
        {
            this.Pavilions = new HashSet<Pavilion>();
        }
    
        public int id { get; set; }
        public int inn { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string pass { get; set; }
        public int type { get; set; }
        public Nullable<int> phone { get; set; }
        public Nullable<int> delete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pavilion> Pavilions { get; set; }
        public virtual Type Type1 { get; set; }
    }
}
