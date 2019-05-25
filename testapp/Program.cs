using System;
using System.Threading.Tasks;
using System.Diagnostics;
using lizzie;

namespace testapp
{
    class Program
    {
        static void Main(string[] args)
        {
			int a = 10;
			float f = 5.0f;

			dynamic x1 = a;
			dynamic y1 = f;

			Console.WriteLine(a + f);

			var lambda = LambdaCompiler.Compile(new object(), @"+(5,3)");



			// Func<Task<object>> lambda = () => Task.FromResult<object>(-5-6-10);

 			var code = @"noop(noop(noop(10, 57)))";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["+"] = Functions<LambdaCompiler.Nothing>.Add;
			binder["noop"] = Functions<LambdaCompiler.Nothing>.Noop;

            var result = function(ctx, binder);

			for(var i =0; i < 1; i++) {
				var x = function(ctx, binder).Result;
			}

			// Func<Task<object>> lambdaa = async () => { return 5 + 3; };
			// Func<Task<object>> lambdab = async () => { return 5 + 3; };
			// Func<Task<object>> lambda2 = async () => { return (int)(await lambdaa()) + (int)(await lambdab()) + (int)(await lambdab()); };

			// Func<object> slambdaa = () => { return 5 + 3; };
			// Func<object> slambdab = () => { return 5 + 3; };
			// Func<object> slambda2 = () => { return (int)(slambdaa()) + (int)(slambdab()) + (int)(slambdab()); };

			var sw = Stopwatch.StartNew();
			
			// for(var i =0;i < 300000; i++) {
			// 	var x = lambda2().Result;
			// }

			for(var i =0;i < 300000; i++) {
				var x = function(ctx, binder).Result;
			}

			Console.Write(sw.ElapsedMilliseconds);

            Console.WriteLine("Hello World!");
        }
    }
}
