﻿namespace XunitRetryHelper.Lib
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    [Serializable]
    public class RetryTestCase : XunitTestCase
    {
        private int maxRetries;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public RetryTestCase()
        {
        }

        public RetryTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay testMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod, int maxRetries)
            : base(diagnosticMessageSink, testMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments: null)
        {
            this.maxRetries = maxRetries;
        }

        public override async Task<RunSummary> RunAsync(
                                                IMessageSink diagnosticMessageSink,
                                                IMessageBus messageBus,
                                                object[] constructorArguments,
                                                ExceptionAggregator aggregator,
                                                CancellationTokenSource cancellationTokenSource)
        {
            return await RetryLogicWrapper.PerformRetry(base.RunAsync, this.maxRetries, diagnosticMessageSink, messageBus, constructorArguments, aggregator, this, cancellationTokenSource);
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);

            data.AddValue("MaxRetries", this.maxRetries);
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);

            this.maxRetries = data.GetValue<int>("MaxRetries");
        }
    }
}
