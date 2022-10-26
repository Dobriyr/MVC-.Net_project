﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Receipt:BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime OperationDate { get; set; }
        public bool IsCheckedOut { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
