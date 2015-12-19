// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis.Sarif.Sdk;

namespace Microsoft.CodeAnalysis.Sarif.Driver.Sdk
{
    public interface IResultLogger
    {
        void Log(ResultKind messageKind, IAnalysisContext context, string message);

        void Log(ResultKind messageKind, IAnalysisContext context, FormattedMessage message);
    }
}
