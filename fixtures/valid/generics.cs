using Map = System.Collections.Generic.Dictionary<string, int>;
namespace Data {
    public interface IRepository<in T, out U> { U Read(T key); }
    public record struct Entry<T>(string Name, T Value) where T : class;
    public partial class Service<T> where T : class, new() {
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<T>> Values;
        public T Create() => new T();
        public int @class { get; init; }
        public long Number() => 0xFFUL + 0b1010 + 1_000L;
        public decimal Price() => 1.25m;
        public string Read(object item) => item as string ?? "";
        public bool Test(object item) => item is string;
    }
}
