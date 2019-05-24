﻿///*
// * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
// *
// * Licensed under the terms of the MIT license, see the enclosed LICENSE
// * file for details.
// */

//using System.Collections.Generic;
//using NUnit.Framework;
//using lizzie.tests.context_types;

//namespace lizzie.tests
//{
//    public class Map
//    {
//        [Test]
//        public void CreateEmpty()
//        {
//            var lambda = LambdaCompiler.Compile("map()");
//            var result = lambda();
//            Assert.IsTrue(result is Dictionary<string, object>);
//        }

//        [Test]
//        public void CreateWithInitialValues()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//map(
//  'foo', 57,
//  'bar', 77
//)
//");
//            var result = lambda();
//            var map = result as Dictionary<string, object>;
//            Assert.AreEqual(2, map.Count);
//            Assert.AreEqual(57, map["foo"]);
//            Assert.AreEqual(77, map["bar"]);
//        }

//        [Test]
//        public void Count()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//count(map(
//  'foo', 57,
//  'bar', 77
//))
//");
//            var result = lambda();
//            Assert.AreEqual(2, result);
//        }

//        [Test]
//        public void RetrieveValues()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//var(@my-map, map(
//  'foo', 57,
//  'bar', 77
//))
//get(my-map, 'foo')
//");
//            var result = lambda();
//            Assert.AreEqual(57, result);
//        }

//        [Test]
//        public void Add()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//var(@my-map, map(
//  'foo', 57
//))
//add(my-map, 'bar', 77, 'howdy', 99)
//my-map
//");
//            var result = lambda();
//            var map = result as Dictionary<string, object>;
//            Assert.AreEqual(3, map.Count);
//            Assert.AreEqual(57, map["foo"]);
//            Assert.AreEqual(77, map["bar"]);
//            Assert.AreEqual(99, map["howdy"]);
//        }

//        [Test]
//        public void ComplexAdd_01()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//var(@my-map, map(
//  'foo', 57
//))
//add(my-map, 'bar', 77, 'howdy', 99)
//add(my-map, 'world', list(1,2,3))
//my-map
//");
//            var result = lambda();
//            var map = result as Dictionary<string, object>;
//            Assert.AreEqual(4, map.Count);
//            Assert.AreEqual(57, map["foo"]);
//            Assert.AreEqual(77, map["bar"]);
//            Assert.AreEqual(99, map["howdy"]);
//            Assert.IsTrue(map["world"] is List<object>);
//            var list = map["world"] as List<object>;
//            Assert.AreEqual(3, list.Count);
//            Assert.AreEqual(1, list[0]);
//            Assert.AreEqual(2, list[1]);
//            Assert.AreEqual(3, list[2]);
//        }

//        [Test]
//        public void ComplexAdd_02()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//var(@my-map, map(
//  'world', list(1,2,3)
//))
//add(get(my-map, 'world'),4,5)
//my-map
//");
//            var result = lambda();
//            var map = result as Dictionary<string, object>;
//            Assert.AreEqual(1, map.Count);
//            var list = map["world"] as List<object>;
//            Assert.IsNotNull(list);
//            Assert.AreEqual(5, list.Count);
//            Assert.AreEqual(1, list[0]);
//            Assert.AreEqual(2, list[1]);
//            Assert.AreEqual(3, list[2]);
//            Assert.AreEqual(4, list[3]);
//            Assert.AreEqual(5, list[4]);
//        }

//        [Test]
//        public void ComplexAdd_03()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//var(@my-map, map(
//  'world', list(1,2,3)
//))
//add(
//  get(my-map, 'world'),
//  map(
//    'foo', 'bar'))
//my-map
//");
//            var result = lambda();
//            var map = result as Dictionary<string, object>;
//            Assert.AreEqual(1, map.Count);
//            var list = map["world"] as List<object>;
//            Assert.IsNotNull(list);
//            Assert.AreEqual(4, list.Count);
//            Assert.AreEqual(1, list[0]);
//            Assert.AreEqual(2, list[1]);
//            Assert.AreEqual(3, list[2]);
//            var innerMap = list[3] as Dictionary<string, object>;
//            Assert.IsNotNull(innerMap);
//            Assert.AreEqual("bar", innerMap["foo"]);
//        }

//        [Test]
//        public void Each_01()
//        {
//            var lambda = LambdaCompiler.Compile(@"
//var(@my-map, map(
//  'foo', 47,
//  'bar', 10
//))
//var(@result, 0)
//each(@ix, my-map, {
//  set(@result, +(result, get(my-map, ix)))
//})
//result
//");
//            var result = lambda();
//            Assert.AreEqual(57, result);
//        }
//    }
//}
