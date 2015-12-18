﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.CodeAnalysis.Sarif.Driver.Sdk
{
    public abstract class DriverCommand<T> 
    {
        public virtual IEnumerable<Assembly> DefaultPlugInAssemblies
        {
            get { return null;  }
            set { throw new InvalidOperationException(); }
        }

        public abstract string Prerelease { get; }

        abstract public int Run(T options);

        public const int FAILURE = 1;
        public const int SUCCESS = 0;
    }
}
