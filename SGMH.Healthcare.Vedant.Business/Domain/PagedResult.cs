using System.Collections.Generic;

namespace SGMH.Healthcare.Vedant.Business.Domain
{
    public class PagedResult<T> where T : class
    {
        public int Index { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
