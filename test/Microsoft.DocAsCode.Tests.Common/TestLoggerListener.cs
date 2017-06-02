﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.Tests.Common
{
    using System;
    using System.Collections.Generic;

    using Microsoft.DocAsCode.Common;

    public class TestLoggerListener : ILoggerListener
    {
        public List<ILogItem> Items { get; } = new List<ILogItem>();

        private readonly Func<ILogItem, bool> _filter;

        public TestLoggerListener(Func<ILogItem, bool> filter)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        #region ILoggerListener

        public void WriteLine(ILogItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (_filter(item))
            {
                Items.Add(item);
            }
        }

        public void Flush()
        {
        }

        public void Dispose()
        {
        }

        #endregion

        #region Creators

        public static TestLoggerListener CreateLoggerListenerWithPhaseStartFilter(string phase, LogLevel logLevel = LogLevel.Warning)
        {
            return new TestLoggerListener(iLogItem =>
            {
                if (iLogItem.LogLevel < logLevel)
                {
                    return false;
                }
                if (phase == null ||
                   (iLogItem?.Phase != null && iLogItem.Phase.StartsWith(phase)))
                {
                    return true;
                }
                return false;
            });
        }

        public static TestLoggerListener CreateLoggerListenerWithPhaseEndFilter(string phase, LogLevel logLevel = LogLevel.Warning)
        {
            return new TestLoggerListener(iLogItem =>
            {
                if (iLogItem.LogLevel < logLevel)
                {
                    return false;
                }
                if (phase == null ||
                   (iLogItem?.Phase != null && iLogItem.Phase.EndsWith(phase)))
                {
                    return true;
                }
                return false;
            });
        }

        public static TestLoggerListener CreateLoggerListenerWithPhaseEqualFilter(string phase, LogLevel logLevel = LogLevel.Warning)
        {
            return new TestLoggerListener(iLogItem =>
            {
                if (iLogItem.LogLevel < logLevel)
                {
                    return false;
                }
                if (phase == null ||
                   (iLogItem?.Phase != null && iLogItem.Phase == phase))
                {
                    return true;
                }
                return false;
            });
        }

        #endregion
    }
}
