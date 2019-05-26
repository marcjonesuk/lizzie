using System;
using System.Threading.Tasks;
using System.Diagnostics;
using lizzie;

namespace testapp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			// var code = @"
			// write('hello')
			// if ('true', {
			// 	write(len('yo')); write('yo2') 
			// 	return(1)
			// })

			// return(5)
			// write('hello2')
			// ";

			// var tokenizer = new Tokenizer(new LizzieTokenizer());
			// var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
			// var ctx = new LambdaCompiler.Nothing();
			// var binder = new Binder<LambdaCompiler.Nothing>();
			// binder["+"] = Functions<LambdaCompiler.Nothing>.Add;
			// binder["write"] = Functions<LambdaCompiler.Nothing>.Write;
			// binder["len"] = Functions<LambdaCompiler.Nothing>.Length;

			// binder["if"] = Functions<LambdaCompiler.Nothing>.If;
			// binder["return"] = Functions<LambdaCompiler.Nothing>.Return;
			
			// Console.WriteLine(await function(ctx, binder));

			// Func<Task<object>> lambda = () => Task.FromResult<object>(-5-6-10);

			var code = @"noop(noop(noop(noop(10, 57))))";


			var tokenizer = new Tokenizer(new LizzieTokenizer());
			var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
			var ctx = new LambdaCompiler.Nothing();
			var binder = new Binder<LambdaCompiler.Nothing>();
			binder["+"] = Functions<LambdaCompiler.Nothing>.Add;
			binder["noop"] = Functions<LambdaCompiler.Nothing>.Noop;

			var result = function(ctx, binder);

			for (var i = 0; i < 1; i++)
			{
				var x = await function(ctx, binder);
			}

			var sw = Stopwatch.StartNew();

			// for(var i =0;i < 300000; i++) {
			// 	var x = lambda2().Result;
			// }

			for (var i = 0; i < 300000; i++)
			{
				var x = await function(ctx, binder);
			}

			Console.Write(sw.ElapsedMilliseconds);
		}
	}
}
