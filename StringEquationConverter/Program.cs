using StringEquationConverter;
using StringEquationConverter.ValueTypes;

// 2(3)*3*2
// 1*2*10+14/9*2/1
// 3*2^2-1
//2*10+
var equ = "100/100-0-1-2-3-4*1";//14-3.539(4-(5+4(8)))
try
{
    var tree = new FTree(equ);
    Console.WriteLine($"{equ} = {tree.ToFraction()}");
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}