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
    
    public partial class data_waterLavel
    {
        public int iddata_waterLavel { get; set; }
        public string data { get; set; }
        public int sensor_iddata { get; set; }
    
        public virtual sensor sensor { get; set; }
    }
}
