/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;
using lizzie.tests.context_types;
using System.Threading.Tasks;

namespace lizzie.tests
{
    public class Branching
    {
        [Test]
        public async Task IfVariableHasValueTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 1)
if(foo, {
  57
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfVariableHasValueFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo)
if(foo, {
  57
})
");
            var result = await lambda();
            Assert.IsNull(result);
        }

        [Test]
        public async Task LazyIfConditionYieldsTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, function({
  67
}))
if(@foo(), {
  57
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task DeclaredArgumentPassedIn()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo,function({
  input
}, @input))
if(@foo(""howdy""), {
  57
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task ElseYieldsTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo)
if(foo, {
  67
}, {
  57
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task ElseYieldsFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 1)
if(foo, {
  67
}, {
  57
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfEqualsTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 7)
if(eq(foo, 7), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfEqualsFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 5)
if(eq(foo, 7), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfNotEqualsTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 7)
if(not(eq(foo, 7)), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfNotEqualsFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 5)
if(not(eq(foo, 7)), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfMoreThanTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 7)
if(mt(foo, 5), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfMoreThanFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 5)
if(mt(foo, 7), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfLessThanTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 7)
if(lt(foo, 9), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfLessThanFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 5)
if(lt(foo, 3), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfMoreThanEqualsTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 7)
if(mte(foo, 7), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfMoreThanEqualsFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 5)
if(mte(foo, 7), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfLessThanEqualsTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 7)
if(lte(foo, 9), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfLessThanEqualsFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, 5)
if(lte(foo, 3), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfAnyTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo1)
var(@foo2, 1)
var(@foo3)
if(any(@foo1, @foo2, @foo3), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfAnyTrueFunction()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo1)
var(@foo2, function({
  'foo'
}))
var(@foo3)
if(any(@foo1, @foo2(), @foo3), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfAnyFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo1)
var(@foo2)
var(@foo3)
if(any(@foo1, @foo2, @foo3), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfAnyEmpty()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
if(any(), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfAllTrue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo1, 1)
var(@foo2, 2)
var(@foo3, 3)
if(all(@foo1, @foo2, @foo3), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task IfAllFalse()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo1, 1)
var(@foo2, 2)
var(@foo3)
if(all(@foo1, @foo2, @foo3), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task IfAllEmpty()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
if(all(), {
  57
}, {
  67
})
");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }
    }
}
