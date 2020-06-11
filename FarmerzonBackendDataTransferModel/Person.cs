﻿using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class Person
    {
        public long PersonId { get; set; }
        
        public IList<Article> Articles { get; set; }
        
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
    }
}