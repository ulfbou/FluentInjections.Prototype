// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public interface ITestInterface { }
    public class TestAttribute : Attribute { }

    [TestAttribute]
    public class TestClassWithAttribute { }

    public class TestClassWithoutAttribute { }
    public class TestClassWithInterface : ITestInterface { }
    public class TestClassWithoutInterface { }
}
