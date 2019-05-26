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
    public class Parser
    {
        [Test]
        public async Task InlineIntegerSymbol()
        {
            var code = "57";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            var result = await function(ctx, binder);
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task InlineFloatingPointSymbol()
        {
            var code = "57.67";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            var result = await function(ctx, binder);
            Assert.AreEqual(57.67, result);
        }

        [Test]
        public async Task InlineFloatingPointInScientificNotationSymbol()
        {
            var code = "57.67e2";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            var result = await function(ctx, binder);
            Assert.AreEqual(5767.0, result);
        }

        [Test]
        public async Task InlineFloatingPointInScientificNotationWithNegativeExponentSymbol()
        {
            var code = "5767e-2";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            var result = await function(ctx, binder);
            Assert.AreEqual(57.67, result);
        }

        [Test]
        public async Task InlineStringSymbol()
        {
            var code = @"""57""";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            var result = await function(ctx, binder);
            Assert.AreEqual("57", result);
        }

        [Test]
        public async Task SymbolicallyReferencedConstantString()
        {
            var code = "foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["foo"] = "bar";
            var result = await function(ctx, binder);
            Assert.AreEqual("bar", result);
        }

        [Test]
        public async Task SymbolicallyReferencedConstantInteger()
        {
            var code = "foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["foo"] = 57;
            var result = await function(ctx, binder);
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task SymbolicallyReferencedFunctionInvocationReturningInteger()
        {
            var code = "foo()";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["foo"] = new FunctionAsync<LambdaCompiler.Nothing>((ctx2, binder2, arguments) =>
            {
                return Task.FromResult((object)57);
            });
            var result = await function(ctx, binder);
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task SymbolicallyReferencedComplexType()
        {
            var code = "foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["foo"] = new LambdaCompiler.Nothing();
            var result = await function(ctx, binder);
            Assert.IsTrue(result is LambdaCompiler.Nothing);
        }

        //[Test]
        //public async Task SymbolicallyReferencedFunctionInvocationReturningComplexType()
        //{
        //    var code = "foo()";
        //    var tokenizer = new Tokenizer(new LizzieTokenizer());
        //    var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
        //    var ctx = new LambdaCompiler.Nothing();
        //    var binder = new Binder<LambdaCompiler.Nothing>();
        //    binder["foo"] = new Function<LambdaCompiler.Nothing>((ctx2, binder2, arguments) =>
        //    {
        //        return new LambdaCompiler.Nothing();
        //    });
        //    var result = await function(ctx, binder);
        //    Assert.IsTrue(result is LambdaCompiler.Nothing);
        //}

        [Test]
        public async Task LiterallyReferencingSymbolName()
        {
            var code = "@foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["foo"] = 57;
            var result = await function(ctx, binder);
            Assert.AreEqual("foo", result);
        }

        [Test]
        public async Task BinderGetFunctionInvocation()
        {
            var code = "get-constant-integer()";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<SimpleValues>(tokenizer, code);
            var ctx = new SimpleValues() { ValueInteger = 57 };
            var binder = new Binder<SimpleValues>();
            var result = await function(ctx, binder);
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task BinderSetFunctionInvocation()
        {
            var code = "set-value-integer(57)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<SimpleValues>(tokenizer, code);
            var ctx = new SimpleValues();
            var binder = new Binder<SimpleValues>();
            var result = await function(ctx, binder);
            Assert.AreEqual(57, ctx.ValueInteger);
        }

        [Test]
        public async Task BinderNestedFunctionGetAndSetInvocations()
        {
            var code = "set-value-string(get-constant-integer())";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<SimpleValues>(tokenizer, code);
            var ctx = new SimpleValues() { ValueInteger = 57 };
            var binder = new Binder<SimpleValues>();
            var result = await function(ctx, binder);
            Assert.IsNull(result);
            Assert.AreEqual("57", ctx.ValueString);
        }

        [Test]
        public async Task BinderMultipleSetFunctionInvocations()
        {
            var code = @"
        set-value-integer(57)
        set-value-integer(67)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<SimpleValues>(tokenizer, code);
            var ctx = new SimpleValues();
            var binder = new Binder<SimpleValues>();
            var result = await function(ctx, binder);
            Assert.IsNull(result);
            Assert.AreEqual(67, ctx.ValueInteger);
        }

        [Test]
        public async Task BinderTwoNestedFunctionInvocationArguments()
        {
            var code = @"
        add-integers(get-constant-integer(), get-value-integer())
        ";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<SimpleValues>(tokenizer, code);
            var ctx = new SimpleValues() { ValueInteger = 10 };
            var binder = new Binder<SimpleValues>();
            var result = await function(ctx, binder);
            Assert.IsNull(result);
            Assert.AreEqual(67, ctx.ValueInteger);
        }

        [Test]
        public async Task BinderComplexInvocation()
        {
            var code = @"
        set-value-string(""2"")
        set-value-integer(2)
        add-integers(
          get-constant-integer(), 
          8, 
          get-value-integer(), 
          6, 
          mirror(
            get-value-integer()), 
          get-value-string())
        ";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<SimpleValues>(tokenizer, code);
            var ctx = new SimpleValues();
            var binder = new Binder<SimpleValues>();
            var result = await function(ctx, binder);
            Assert.IsNull(result);
            Assert.AreEqual(77, ctx.ValueInteger);
        }

        [Test]
        public async Task Body()
        {
            var code = "{57}";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            var result = await function(ctx, binder);
            Assert.IsTrue(result is FunctionAsync<LambdaCompiler.Nothing>);
        }
    }
}
