using System.Collections.Generic;

namespace Xunit.Extensions.Models
{
    public class DataModel : DataModel<object>
    {
    }

    public class DataModel<T>
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public T[] IndexedData { get; set; }

        public IDictionary<string, T> NamedData { get; set; }
    }
}
