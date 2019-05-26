/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace lizzie.tests
{
    public class String
    {
        [Test]
        public async Task SubstringOnlyOffset()
        {
            var lambda = LambdaCompiler.CompileAsync(@"substr(""foobarxyz"", 3)");
            var result = await lambda();
            Assert.AreEqual("barxyz", result);
        }

        [Test]
        public async Task SubstringWithCount()
        {
            var lambda = LambdaCompiler.CompileAsync(@"substr(""foobarxyz"", 3, 3)");
            var result = await lambda();
            Assert.AreEqual("bar", result);
        }

        [Test]
        public async Task LengthOfString()
        {
            var lambda = LambdaCompiler.CompileAsync(@"length(""foo"")");
            var result = await lambda();
            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task Replace()
        {
            var lambda = LambdaCompiler.CompileAsync(@"replace(""foo"", ""o"", ""xx"")");
            var result = await lambda();
            Assert.AreEqual("fxxxx", result);
        }

        [Test]
        public async Task SingleQuoteStrings()
        {
            var lambda = LambdaCompiler.CompileAsync(@"replace('foo', 'o', 'xx')");
            var result = await lambda();
            Assert.AreEqual("fxxxx", result);
        }

        [Test]
        public async Task EscapedSingleQuotedString()
        {
            var lambda = LambdaCompiler.CompileAsync(@"'foo\'bar'");
            var result = await lambda();
            Assert.AreEqual("foo'bar", result);
        }

        [Test]
        public async Task ConvertFromNumber()
        {
            var lambda = LambdaCompiler.CompileAsync(@"string(57)");
            var result = await lambda();
            Assert.AreEqual("57", result);
        }

        [Test]
        public async Task ConvertToNumber()
        {
            var lambda = LambdaCompiler.CompileAsync(@"number('57')");
            var result = await lambda();
            Assert.AreEqual(57, result);
        }

        [Test]
        public async Task ConvertFromStringToString()
        {
            var lambda = LambdaCompiler.CompileAsync(@"string('57')");
            var result = await lambda();
            Assert.AreEqual("57", result);
        }

        [Test]
        public async Task EscapedDoubleQuotedString()
        {
            var lambda = LambdaCompiler.CompileAsync(@"""foo\""bar""");
            var result = await lambda();
            Assert.AreEqual("foo\"bar", result);
        }

        [Test]
        public async Task JSONString_01()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
string(map(
  'foo', 57,
  'bar', 67
))
");
            var result = await lambda();
            Assert.AreEqual(@"{""foo"":57,""bar"":67}", result);
        }

        [Test]
        public async Task JSONString_02()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
string(map(
  'foo', 'howdy',
  'bar', 'world'
))
");
            var result = await lambda();
            Assert.AreEqual(@"{""foo"":""howdy"",""bar"":""world""}", result);
        }

        [Test]
        public async Task JSONString_03()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
string(map(
  'foo', 'howdy',
  'bar', 'wor""ld'
))
");
            var result = await lambda();
            Assert.AreEqual(@"{""foo"":""howdy"",""bar"":""wor\""ld""}", result);
        }

        [Test]
        public async Task JSONString_04()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
string(list(
  'foo',
  'bar'
))
");
            var result = await lambda();
            Assert.AreEqual(@"[""foo"",""bar""]", result);
        }

        [Test]
        public async Task JSONString_05()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
string(list(
  'foo',
  map(
    'bar1',57,
    'bar2',77,
    'bar3',list(1,2,map('hello','world'))
  )
))
");
            var result = await lambda();
            Assert.AreEqual(@"[""foo"",{""bar1"":57,""bar2"":77,""bar3"":[1,2,{""hello"":""world""}]}]", result);
        }

        [Test]
        public async Task JSONStringToObject_01()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
json(""{'foo':57}"")
");
            var result = await lambda();
            var map = result as Dictionary<string, object>;
            Assert.IsNotNull(map);
            Assert.AreEqual(57, map["foo"]);
        }

        [Test]
        public async Task JSONStringToObject_02()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
json(""[0,1,2]"")
");
            var result = await lambda();
            var list = result as List<object>;
            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(0, list[0]);
            Assert.AreEqual(1, list[1]);
            Assert.AreEqual(2, list[2]);
        }

        [Test]
        public async Task JSONStringToObject_03()
        {
            var lambda = LambdaCompiler.CompileAsync(@"
json(""[0,1,{'foo':57,'bar':77,'hello':'world'}]"")
");
            var result = await lambda();
            var list = result as List<object>;
            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(0, list[0]);
            Assert.AreEqual(1, list[1]);
            var map = list[2] as Dictionary<string, object>;
            Assert.IsNotNull(map);
            Assert.AreEqual(3, map.Count);
            Assert.AreEqual(57, map["foo"]);
            Assert.AreEqual(77, map["bar"]);
            Assert.AreEqual("world", map["hello"]);
        }

        [Test]
        public async Task NonTerminatedStringThrows()
        {
            var success = false;
            try
            {
                var lambda = LambdaCompiler.CompileAsync(@"'foo");
            }
            catch
            {
                success = true;
            }
            Assert.AreEqual(true, success);
        }

        [Test]
        public async Task NewLineInStringThrows_01()
        {
            var success = false;
            try
            {
                var lambda = LambdaCompiler.CompileAsync("'foo\n'");
            }
            catch
            {
                success = true;
            }
            Assert.AreEqual(true, success);
        }

        [Test]
        public async Task NewLineInStringThrows_02()
        {
            var success = false;
            try
            {
                var lambda = LambdaCompiler.CompileAsync("'foo\r'");
            }
            catch
            {
                success = true;
            }
            Assert.AreEqual(true, success);
        }
    }
}
