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
    
    public partial class Pavilion
    {
        public int id { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public int SEC { get; set; }
    
        public virtual SEC SEC1 { get; set; }
    }
}
