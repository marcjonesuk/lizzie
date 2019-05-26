﻿/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using NUnit.Framework;
using lizzie.exceptions;
using lizzie.tests.context_types;
using System.Threading.Tasks;

namespace lizzie.tests
{
    public class LambdaBuilder
    {
        [Test]
        public async Task WithType()
        {
            var lambda = LambdaCompiler.Compile("57");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task MathTest()
        {
            var lambda = LambdaCompiler.Compile("%(/(+(5, *(5,-(20,15))),3),7)");
            var result = await lambda();
            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task VariableTest()
        {
            var code = @"
var(@foo, 57)
var(@bar, +(foo, *(10,2)))
bar";
            var lambda = LambdaCompiler.Compile(code);
            var result = await lambda();
            Assert.AreEqual(77, result);
        }

        [Test]
        public async Task SettingVariableToResultOfFunctionInvocation()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, var(@bar, 67))
bar");
            var result = await lambda();
            Assert.AreEqual(67, result);
        }

        [Test]
        public async Task SettingVariableToFunctionInvocation_01()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, @var(@bar, 67))
foo");
            var result = await lambda();
            Assert.IsTrue(result is FunctionAsync<LambdaCompiler.Nothing>);
        }

        [Test]
        public async Task SettingVariableToFunctionInvocation_02()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, @var(@bar, 57))
bar");
            var success = false;
            try
            {
                await lambda();
            }
            catch (LizzieRuntimeException)
            {
                success = true;
            }
            Assert.IsTrue(success);
        }

        [Test]
        public async Task SettingVariableToFunctionInvocation_03()
        {
            var lambda = LambdaCompiler.Compile(@"
var(@foo, @var(@bar, 57))
foo()
bar");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task CompileWithExplicitlyClonedContext()
        {
            /*
             * Creating a "master binder" that you never directly use, but rather
             * clones for each time you evaluate a snippet of Lizzie code, allows
             * you to avoid having to run through all the reflection initialization
             * when binding to your context type, probably saving you a lot of
             * resources when binding to the same type in multiple threads.
             */
            var masterBinder = new Binder<SimpleValues>();
            LambdaCompiler.BindFunctions<SimpleValues>(masterBinder);
            masterBinder["bar"] = 10;

            // Cloning our binder and evaluating a snippet of Lizzie code.
            var binder1 = masterBinder.Clone();
            binder1["bar2"] = 2;
            var lambda1 = LambdaCompiler.Compile<SimpleValues>(new SimpleValues(), (Binder<SimpleValues>)binder1, @"
var(@foo, +(55, bar, bar2))
");
            var result1 = lambda1();

            // Cloning a new binder and evaluating a new snippet of Lizzie code.
            var binder2 = masterBinder.Clone();
            Assert.IsFalse(binder2.ContainsKey("bar2"));
            var lambda2 = LambdaCompiler.Compile<SimpleValues>(new SimpleValues(), (Binder<SimpleValues>)binder2, @"
var(@foo, +(67, bar))
");
            var result2 = await lambda2();

            // Sanity checking result.
            Assert.AreEqual(67, result1);
            Assert.AreEqual(77, result2);
        }
    }
}
