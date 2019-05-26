/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System.Collections.Generic;
using NUnit.Framework;
using lizzie.tests.context_types;
using System.Threading.Tasks;

namespace lizzie.tests
{
    public class List
    {
        [Test]
        public async Task CreateList()
        {
            var lambda = LambdaCompiler.CompileAsync("list(57, 67, 77)");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(57, list[0]);
            Assert.AreEqual(67, list[1]);
            Assert.AreEqual(77, list[2]);
        }

        [Test]
        public async Task CountListContent()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77))
count(foo)");
            var result = await lambda();
            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task GetListValue()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77))
get(foo, 2)");
            var result = await lambda();
            Assert.AreEqual(77, result);
        }

        [Test]
        public async Task AddToList()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57))
add(foo, 67, 77)
foo");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(57, list[0]);
            Assert.AreEqual(67, list[1]);
            Assert.AreEqual(77, list[2]);
        }

        [Test]
        public async Task SliceList_01()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77, 87))
slice(foo, 1, 3)");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(67, list[0]);
            Assert.AreEqual(77, list[1]);
        }

        [Test]
        public async Task SliceList_02()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77, 87))
slice(foo, 1)");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(67, list[0]);
            Assert.AreEqual(77, list[1]);
            Assert.AreEqual(87, list[2]);
        }

        [Test]
        public async Task SliceList_03()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77, 87))
slice(foo)");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(57, list[0]);
            Assert.AreEqual(67, list[1]);
            Assert.AreEqual(77, list[2]);
            Assert.AreEqual(87, list[3]);
        }

        [Test]
        public async Task SliceList_04()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77, 87))
slice(foo, 1, 1)");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public async Task Each_01()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77, 87))
var(@bar, list())
each(@ix, foo, {
  if(any(@eq(57, ix), @eq(77, ix)), {
    add(bar, ix)
  })
})
bar");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(57, list[0]);
            Assert.AreEqual(77, list[1]);
        }

        [Test]
        public async Task Each_02()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77, 87))
var(@bar, list())
each(@ix, foo, {
  if(any(@eq(57, ix), @eq(77, ix)), {
    add(bar, string(ix))
  })
})
bar");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("57", list[0]);
            Assert.AreEqual("77", list[1]);
        }

        [Test]
        public async Task Each_03()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(""57"", ""67"", ""77"", ""88.88"", ""97""))
var(@bar, list())
each(@ix, foo, {
  if(any(@eq(""57"", ix), @eq(""77"", ix), @eq(""88.88"", ix)), {
    add(bar, number(ix))
  })
})
bar");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(57, list[0]);
            Assert.AreEqual(77, list[1]);
            Assert.AreEqual(88.88, list[2]);
        }

        [Test]
        public async Task Each_04()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, list(57, 67, 77))
var(@bar, each(@ix, foo, {
  +(ix, 10)
}))
bar");
            var result = await lambda();
            Assert.IsTrue(result is List<object>);
            var list = result as List<object>;
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(67, list[0]);
            Assert.AreEqual(77, list[1]);
            Assert.AreEqual(87, list[2]);
        }

        [Test]
        public async Task ApplyArgumentsToAdd()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
var(@foo, +(apply(list(57,10,10))))
");
            var result = await lambda();
            Assert.AreEqual(77, result);
        }
    }
}
