﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the repo root for full license information.
// ------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PSharp.Timers;
using Microsoft.PSharp;
using Xunit;

namespace Microsoft.PSharp.TestingServices.Tests.Unit
{
    public class StartStopTimerTest : BaseTest
    {
		#region internal events
		class TimeoutReceivedEvent : Event { }

		#endregion

		#region machine/monitors
		class Client : TimedMachine
		{
			#region fields
			object payload = new object();
			#endregion

			#region states
			[Start]
			[OnEntry(nameof(Initialize))]
			[OnEventDoAction(typeof(TimerElapsedEvent), nameof(HandleTimeout))]
			class Init : MachineState { }
			#endregion

			#region handlers
			async Task Initialize()
			{
				// Start a timer, and stop it immediately
				TimerId tid = StartTimer(payload, 10, true);
				await this.StopTimer(tid, flush: false);
			}

			// Timer fired in the interval between StartTimer and StopTimer
			void HandleTimeout()
			{
				this.Monitor<LivenessMonitor>(new TimeoutReceivedEvent());
			}
			#endregion
		}

		class LivenessMonitor : Monitor
		{
			[Start]
			[Hot]
			[OnEventGotoState(typeof(TimeoutReceivedEvent), typeof(TimeoutReceived))]
			class NoTimeoutReceived : MonitorState { }

			[Cold]
			class TimeoutReceived : MonitorState { }
		}
		#endregion

		#region test
		// Test the fact that no timeouts may arrive between StartTimer and StopTimer
		[Fact]
		public void StartStopTest()
		{
			var config = base.GetConfiguration();
			config.LivenessTemperatureThreshold = 150;
			config.MaxSchedulingSteps = 300;
            config.SchedulingIterations = 1000;

            var test = new Action<PSharpRuntime>((r) => {
				r.RegisterMonitor(typeof(LivenessMonitor));
				r.CreateMachine(typeof(Client));
			});
            
			base.AssertFailed(config, test,
                "Monitor 'LivenessMonitor' detected liveness bug in hot state 'Microsoft.PSharp.TestingServices.Tests.Unit.StartStopTimerTest+LivenessMonitor.NoTimeoutReceived' at the end of program execution.",
                true);
		}
		#endregion

	}
}
