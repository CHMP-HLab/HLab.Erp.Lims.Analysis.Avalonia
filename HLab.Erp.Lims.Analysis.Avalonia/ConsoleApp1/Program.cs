// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

Console.WriteLine("Hello, World!");

var ok = Vector512.IsHardwareAccelerated;
var kk = Avx512F.IsSupported;

Console.WriteLine(kk);