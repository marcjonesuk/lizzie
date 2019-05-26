using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lizzie.tests
{
    class AsyncClass
    {
        [Bind(Name = "foo")]
        async Task<object> Foo(Binder<AsyncClass> ctx, object[] arguments)
        {
            var x = 10;
            await Task.Run(() => x++);
            x++;
            return x;
        }
    }

    class Async
    {
        [Test]
        public async Task TaskIsAwaited()
        {
            AsyncClass simple = new AsyncClass();
            var lambda = LambdaCompiler.CompileAsync(simple, "foo()", true);
            var result = await lambda();
            Assert.AreEqual(12, result);
        }
    }
}
