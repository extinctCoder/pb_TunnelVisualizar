//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pb_TunnelVisualizar.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class rule
    {
        public int idrule { get; set; }
        public string rule1 { get; set; }
        public int sensor_iddata { get; set; }
    
        public virtual sensor sensor { get; set; }
    }
}
