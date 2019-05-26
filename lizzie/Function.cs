/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System.Threading.Tasks;

namespace lizzie
{
	/// <summary>
	/// Delegate for all asynchronous function invocations evaluated by Lizzie in its lambda delegate.
	/// </summary>
	public delegate Task<object> FunctionAsync<TContext>(TContext ctx, Binder<TContext> binder, object[] arguments);

	/// <summary>
	/// Delegate for all synchronous function invocations evaluated by Lizzie in its lambda delegate.
	/// </summary>
	public delegate object Function<TContext>(TContext ctx, Binder<TContext> binder, object[] arguments);

	/// <summary>
	/// Delegate for an asynchronous lambda object created by Lizzie.
	/// </summary>
	public delegate Task<object> LambdaAsync<TContext>(TContext ctx, Binder<TContext> binder);
}
