﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Microsoft.PSharp.ServiceFabric
{
    using Microsoft.PSharp;
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Services.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Threading;
    using System.Threading.Tasks;

    public class ResourceBasedRemoteMachineManager : IRemoteMachineManager
    {
        private const string Delimiter = "|||";

        private string partitionName;
        private IStatefulServicePartition partition;
        private IReliableStateManager stateManager;
        private StatefulServiceContext context;
        private IPSharpEventSourceLogger logger;
        private ResourceTypeLearnerBackgroundTask resourceTypeLearnerTask;

        public ResourceBasedRemoteMachineManager(IStatefulServicePartition partition, IReliableStateManager stateManager, StatefulServiceContext context, IPSharpEventSourceLogger logger)
        {
            this.partition = partition;
            this.stateManager = stateManager;
            this.context = context;
            this.logger = logger;
        }

        public Task<string> CreateMachineIdEndpoint(Type machineType)
        {
            // TODO: ASK the background task
            // !!!!!!!!! - FOR THE TIME BEING RETURN A HARD CODED PARTITION
            return Task.FromResult("fabric:/DemoApp/PoolManager" + Delimiter + "0");
        }

        public string GetLocalEndpoint()
        {
            return this.context.ServiceName + Delimiter + this.partitionName;
        }

        public async Task Initialize(CancellationToken token)
        {
            this.partitionName = this.GetPartitionName();
            // Spawn off background tasks
            resourceTypeLearnerTask = new ResourceTypeLearnerBackgroundTask(this.stateManager, TimeSpan.FromSeconds(10), this.logger);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            resourceTypeLearnerTask.Start(new CancellationToken());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await Task.Yield();
        }

        public bool IsLocalMachine(MachineId mid)
        {
            var parts = mid.Endpoint.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);
            if(parts.Length != 2)
            {
                throw new InvalidOperationException($"Did not expect an endpoint without 2 parts - {mid}");
            }

            if(parts[0] == this.context.ServiceName.ToString() && parts[1] == this.partitionName)
            {
                return true;
            }

            return false;
        }

        public void ParseMachineIdEndpoint(string endpoint, out string serviceName, out string partitionName)
        {
            var parts = endpoint.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new InvalidOperationException($"Did not expect an endpoint without 2 parts - {endpoint}");
            }

            serviceName = parts[0];
            partitionName = parts[1];
        }

        private string GetPartitionName()
        {
            var namedPartition = (NamedPartitionInformation)this.partition.PartitionInfo;
            return namedPartition.Name;
        }
    }
}