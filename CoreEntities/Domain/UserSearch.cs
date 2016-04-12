namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserSearch
    {
        public int UserSearchID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> SignalLevel { get; set; }
        public Nullable<int> Inerval { get; set; }
    }
}
