namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserType
    {
        public int UserTypeID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}
