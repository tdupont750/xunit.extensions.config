namespace Xunit.Extensions.Models
{
    public class DataModel : DataModel<object>
    {
    }

    public class DataModel<T>
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public T[] Data { get; set; }
    }
}
