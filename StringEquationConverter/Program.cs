using StringEquationConverter;
using StringEquationConverter.ValueTypes;

// 2(3)*3*2
// 1*2*10+14/9*2/1
// 3*2^2-1
//2*10+

//-2/2-14-3.539(4-(5+4(8/1)*2))*-2 = (-950.14/2) or -475.07
//(8-(4^2-4*4)^.5)/2
var equ = "2/2-14-3.539(4-(5+4(8/1)*2))*2";//2/2-14-3.539(4-(5+4(8/1)*2))*2 = 447.07 or (894.14/2)
try
{
    var tree = new FTree(equ);
    var res = tree.ToFraction();
    Console.WriteLine($"{equ} = {res.Simplify()} or {res.D}");
}
catch(Exception ex)
{
    Console.WriteLine($"{ex.GetType()}: {ex.Message}");
}