//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShanuSchoppingCart_WCF.Module
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemDetails
    {
        public ItemDetails()
        {
            this.CartDetails = new HashSet<CartDetails>();
        }
    
        public int Item_ID { get; set; }
        public string Item_Name { get; set; }
        public int Item_Price { get; set; }
        public string Image_Name { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
    
        public virtual ICollection<CartDetails> CartDetails { get; set; }
    }
}