﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis.LanguageServer;
using Microsoft.Extensions.Logging;

using System.Diagnostics;

Console.Title = "Microsoft.CodeAnalysis.LanguageServer";

// TODO - Decide how and where we're logging.  For now just logging stderr (vscode reads stdout for LSP messages).
//     1.  File logs for feedback
//     2.  Logs to vscode output window.
//     3.  Telemetry
// https://github.com/microsoft/vscode-csharp-next/issues/12
using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole((options) => options.LogToStandardErrorThreshold = LogLevel.Trace));
var logger = loggerFactory.CreateLogger<ILogger>();

LaunchDebuggerIfEnabled(args);

var jsonRpc = new JsonRpcServer(Console.OpenStandardInput(), Console.OpenStandardOutput(), logger);

await jsonRpc.StartAsync();

void LaunchDebuggerIfEnabled(string[] args)
{
    if (args.Contains("--debug") && !Debugger.IsAttached)
    {
        logger.LogInformation("Launching debugger...");
        _ = Debugger.Launch();
    }
}
