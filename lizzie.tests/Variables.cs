/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;
using lizzie;
using lizzie.exceptions;
using System.Threading.Tasks;

namespace lizzie.tests
{
    public class Variables
    {
        [Test]
        public async Task VariableAssignedToIntegerValue()
        {
            var code = @"
var(@foo, 57)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            var result = await function(ctx, binder);
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task VariableAssignedToFloatingPointValue()
        {
            var code = @"
var(@foo, 57.67)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            var result = await function(ctx, binder);
            Assert.AreEqual(57.67, result);
        }

        [Test]
        public async Task VariableAssignedToStringLiteralValue()
        {
            var code = @"
var(@foo, ""bar"")
foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            var result = await function(ctx, binder);
            Assert.AreEqual("bar", result);
        }

        [Test]
        public async Task VariableDeReferenced()
        {
            var code = @"
var(@foo, 57)
foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            var result = await function(ctx, binder);
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task NonExistentVariableThrows()
        {
            var code = "foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            var success = false;
            try
            {
                await function(ctx, binder);
            }
            catch (LizzieRuntimeException)
            {
                success = true;
            }
            Assert.IsTrue(success);
        }

        [Test]
        public async Task VariableDeclaredTwiceThrows()
        {
            var code = @"
var(@foo, 57)
var(@foo, 57)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            var success = false;
            try
            {
                await function(ctx, binder);
            }
            catch (LizzieRuntimeException)
            {
                success = true;
            }
            Assert.IsTrue(success);
        }

        [Test]
        public async Task VariableDeclarationWithoutInitialAssignment()
        {
            var code = @"
var(@foo)
foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            var result = await function(ctx, binder);
            Assert.IsNull(result);
        }

        [Test]
        public async Task VariableReAssignment()
        {
            var code = @"
var(@foo, 57)
set(@foo, 67)
foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            binder["set"] = Functions<LambdaCompiler.Nothing>.Set;
            var result = await function(ctx, binder);
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task VariableReAssignedToNull()
        {
            var code = @"
var(@foo, 57)
set(@foo)
foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            binder["set"] = Functions<LambdaCompiler.Nothing>.Set;
            var result = await function(ctx, binder);
            Assert.IsNull(result);
        }

        [Test]
        public async Task ReAssigningStaticallyCompiledValue()
        {
            var code = @"
var(@foo)";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            binder["foo"] = 57;
            var success = false;
            try
            {
                await function(ctx, binder);
            }
            catch (LizzieRuntimeException)
            {
                success = true;
            }
            Assert.AreEqual(true, success);
        }

        [Test]
        public async Task VariableChangedFromDoubleToString()
        {
            var code = @"
var(@foo, 57.67)
set(@foo, ""bar"")
foo";
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<LambdaCompiler.Nothing>(tokenizer, code);
            var ctx = new LambdaCompiler.Nothing();
            var binder = new Binder<LambdaCompiler.Nothing>();
            binder["var"] = Functions<LambdaCompiler.Nothing>.Var;
            binder["set"] = Functions<LambdaCompiler.Nothing>.Set;
            var result = await function(ctx, binder);
            Assert.AreEqual("bar", result);
        }
    }
}
