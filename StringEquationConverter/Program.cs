using StringEquationConverter;
using StringEquationConverter.ValueTypes;
using System.Diagnostics;
// 2(3)*3*2
// 1*2*10+14/9*2/1
// 3*2^2-1
//pow(3)
//log(360, 10)
//-2/2-14-3.539(4-(5+4(8/1)*2))*-2 = (-950.14/2) or -475.07
//(8-(4^2-4*4)^.5)/2
//sqrt(X)-pow(16)*3
//pow(sqrt(4))*-pow(2)
//2/2-14-3.539(4-(5+4(8/1)*2))*2 = 447.07 or (894.14/2)
Stopwatch watch = new Stopwatch();
var equ = "rand(0, 10)";
try
{
    Console.WriteLine("---------------Before creating a tree---------------");
    watch.Start();
    var tree = new FTree(equ);
    Console.WriteLine($"{equ} = {tree.ToFraction().D}\nTime taken (ms): {watch.ElapsedMilliseconds}\n");
    watch.Reset();
    Console.WriteLine("---------------After creating a tree----------------");
    watch.Start();
    Console.WriteLine($"{equ} = {tree.ToFraction().D}\nTime taken (ms): {watch.ElapsedMilliseconds}\n");
    watch.Stop();
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.GetType()}: {ex.Message}");
}