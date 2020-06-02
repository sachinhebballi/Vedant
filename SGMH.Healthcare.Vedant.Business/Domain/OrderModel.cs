using System;
using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int CentreId { get; set; }
        public string CentreName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public short? Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public List<DrugModel> OrderedDrugs { get; set; }
    }
}