﻿/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;
using lizzie.tests.context_types;
using lizzie.exceptions;
using System.Threading.Tasks;

namespace lizzie.tests
{
    public class Functions
    {

        [Test]
        public async Task AddTwoIntegers()
        {
            var code = "+(10, 57)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["+"] = Functions<LambdaCompiler.Nothing>.Add;
            var result = await function(ctx, binder);
            Assert.AreEqual(67, result);
        }



        [Test]
        public async Task ReturnsNumberConstant()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
        var(@foo, function({
          57
        }))
        foo()");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

                [Test]
                public async Task ReturnsStringConstant()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@foo, function({
          ""Hello World""
        }))
        foo()");
                    var result = await lambda();
                    Assert.AreEqual("Hello World", result);
                }

                [Test]
                public async Task SingleParameter()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@foo, function({
          +(""Hello "", input)
        }, @input))
        foo(""Thomas"")");
                    var result = await lambda();
                    Assert.AreEqual("Hello Thomas", result);
                }

                [Test]
                public async Task MultipleParameters()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@foo, function({
          +(""Hello "", name, "" it seems you are "", old, "" years old"")
        }, @name, @old))
        foo(""Thomas"", 44)");
                    var result = await lambda();
                    Assert.AreEqual("Hello Thomas it seems you are 44 years old", result);
                }

                [Test]
                public async Task EvaluateFunctionFromWithinFunction()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@bar, function({
          77
        }))
        var(@foo, function({
          bar()
        }))
        foo()");
                    var result = await lambda();
                    Assert.AreEqual(77, result);
                }

                [Test]
                public async Task ChangeStackFromWithinFunction()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@bar, function({
          var(@arg, 50)
        }))
        var(@foo, function({
          var(@arg, 27)
          +(arg, bar())
        }))
        foo()");
                    var result = await lambda();
                    Assert.AreEqual(77, result);
                }

                [Test]
                public async Task VariableDeclaredWithinFunctionDoesNotExistThrows()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@foo, function({
          var(@bar, 50)
        }))
        foo()
        bar");
                    var success = false;
                    try {
                        await lambda();
                    } catch(LizzieRuntimeException) {
                        success = true;
                    }
                    Assert.AreEqual(true, success);
                }

                [Test]
                public async Task NestedFunctions()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@func1, function({
          var(@func2, function({
            +(bar2, '_yup')
          }, @bar2))
          func2(bar1)
        }, @bar1))

        func1('success')
        ");
                    var result = await lambda();
                    Assert.AreEqual("success_yup", result);
                }

                [Test]
                public async Task NestedFunctionsThrows()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@func1, function({
          var(@func2, function({
            bar2
          }, @bar2))
          func2(bar1)
        }, @bar1))

        func2('success')
        ");
                    var success = false;
                    try {
                        await lambda();
                    } catch(LizzieRuntimeException) {
                        success = true;
                    }
                    Assert.AreEqual(true, success);
                }

                [Test]
                public async Task TwoFunctions()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@func1, function({
          +('success1_',bar1)
        }, @bar1))
        var(@func2, function({
          +('_success2_',bar2)
        }, @bar2))
        +(func1('foo'), func2('bar'))
        ");
                    var result = await lambda();
                    Assert.AreEqual("success1_foo_success2_bar", result);
                }

                [Test]
                public async Task NoCode()
                {
                    var lambda = LambdaCompiler.CompileAsync("");
                    var result = await lambda();
                    Assert.IsNull(result);
                }

                [Test]
                public async Task EmptyFunction()
                {
                    var lambda = LambdaCompiler.CompileAsync(@"
        var(@func, function({
        }))
        func()
        ");
                    var result = await lambda();
                    Assert.IsNull(result);
                }
    }
}
