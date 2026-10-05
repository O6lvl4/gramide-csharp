using System;
namespace Demo;
public class Worker {
    private int count = 0, limit = 3;
    private string café = @"C:\data\""file""";
    private char mark = '\n';
    public Worker() : this(1) { }
    public Worker(int start) { count = start; }
    public int Run() {
        Func<int, int> next = x => x + 1;
        int[] numbers = new int[] {1, 2, 3};
        for (int i = 0; i < numbers.Length; i++) { count += numbers[i]; }
        foreach (var item in numbers) { if (item == 0) continue; }
        while (count < 10) count++;
        do { count--; } while (count > 10);
        try { using (var input = Open()) { lock (this) { input.Read(); } } }
        catch (Exception error) when (error != null) { throw; }
        finally { count = 0; }
        switch (count) { case 0: count++; break; default: return -1; }
        return (count << 1) + (count >> 1);
    }
}
public class 東京 { public string 挨拶() => "こんにちは"; }
