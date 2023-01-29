// I want csharp_result to be the same as python_result (ClientSettings.Sav)
// why is it different from python version
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

namespace test;

public class Program {
    static ScriptRuntime python = Python.CreateRuntime();
    static ScriptScope zlib = python.ImportModule("zlib");

    static byte[] decompress(byte[] d) {
        return ((Bytes)zlib.GetVariable("decompress")(d)).ToArray();
    }

    static byte[] compress(byte[] d) {
        return ((Bytes)zlib.GetVariable("compress")(d, 6)).ToArray();
    }

    static byte[] CombineByteArrays(params byte[][] arrays) {
        var r = new byte[arrays.Sum(array => array.Length)];
        var offset = 0;
        foreach (var array in arrays) {
            System.Buffer.BlockCopy(array, 0, r, offset, array.Length);
            offset += array.Length;
        }
        return r;
    }

    public static int Main() {
        var r = File.ReadAllBytes("Data.Sav");
        var header = r[..0x10];
        var original = r[0x10..];

        var decompressed = decompress(original);
        Console.WriteLine($"Wrote {decompressed.Length} bytes to csharp_decompressed");
        File.WriteAllBytes("csharp_decompressed", decompressed);

        var compressed = compress(decompressed);
        Console.WriteLine($"Wrote {compressed.Length} bytes to csharp_compressed");
        File.WriteAllBytes("csharp_compressed", compressed);

        var combined = CombineByteArrays(header, compressed);
        Console.WriteLine($"Wrote {combined.Length} bytes to csharp_result");
        File.WriteAllBytes("csharp_result", combined);
        return 0;
    }
}