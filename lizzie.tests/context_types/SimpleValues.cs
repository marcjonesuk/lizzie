﻿/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;
using System.Threading.Tasks;

namespace lizzie.tests.context_types
{
    public class SimpleValues
    {
        public int ValueInteger { get; set; }
        public string ValueString { get; set; }

        [Bind(Name = "set-value-integer")]
        async Task<object> SetValueInteger(Binder<SimpleValues> ctx, Arguments arguments)
        {
            ValueInteger = arguments.Get<int>(0);
            return null;
        }

        [Bind(Name = "get-value-integer")]
        async Task<object> GetValueInteger(Binder<SimpleValues> ctx, Arguments arguments)
        {
            return ValueInteger;
        }

        [Bind(Name = "get-static")]
        public static async Task<object> GetStatic(SimpleValues context, Binder<SimpleValues> ctx, Arguments arguments)
        {
            return 7;
        }

        [Bind(Name = "set-value-string")]
        async Task<object> SetValueString(Binder<SimpleValues> ctx, Arguments arguments)
        {
            ValueString = arguments.Get<string>(0);
            return null;
        }

        [Bind(Name = "get-value-string")]
        async Task<object> GetValueString(Binder<SimpleValues> ctx, Arguments arguments)
        {
            return ValueString;
        }

        [Bind(Name = "get-constant-integer-2")]
        public async Task<object> GetConstantPublicInteger(Binder<SimpleValues> ctx, Arguments arguments)
        {
            return 57;
        }

        [Bind(Name = "get-constant-integer")]
        protected async Task<object> GetConstantInteger(Binder<SimpleValues> ctx, Arguments arguments)
        {
            return 57;
        }

        [Bind(Name = "add-integers")]
        async Task<object> AddIntegers(Binder<SimpleValues> ctx, Arguments arguments)
        {
            ValueInteger = 0;
            foreach (var ix in arguments) {
                ValueInteger += Convert.ToInt32(ix);
            }
            return null;
        }

        [Bind(Name = "mirror")]
        async Task<object> Mirror(Binder<SimpleValues> ctx, Arguments arguments)
        {
            return arguments.Get(0);
        }
    }
}
