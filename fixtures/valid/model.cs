using System;
using System.Collections.Generic;
namespace Demo.Models {
    [Obsolete("sample")]
    public class Box<T> : IView<T> where T : class, new() {
        private readonly List<T> values = new List<T>();
        public Box(T value) { values.Add(value); }
        public T First() { return values[0]; }
        public U Map<U>(U value) where U : class { return value; }
        public int Count { get; private set; } = 0;
        public string Name => "box";
        public event EventHandler Changed;
        public class Nested { public int Value; }
    }
    public interface IView<T> { T First(); int Count { get; } }
    public record Pair<T>(T First, T Second);
    public readonly struct Point { public readonly int X; public Point(int x) { X = x; } }
    public enum Color : byte { Red = 1, Green, Blue = 4 }
    public delegate TResult Factory<T, TResult>(T value) where T : class;
}
