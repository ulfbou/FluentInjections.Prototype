// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


namespace FluentInjections.Utilities.Collections
{
    internal class AsyncEnumerable
    {
        internal static async IAsyncEnumerable<T> Empty<T>()
        {
            await Task.CompletedTask;
            yield break;
        }
    }
}
