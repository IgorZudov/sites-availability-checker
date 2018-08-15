﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.App
{
	public class BackgroundService : IHostedService
	{
		private Task task;
		private readonly IServiceProvider serviceProvider;
		private readonly ILogger logger;

		public BackgroundService(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
			logger = loggerFactory.CreateLogger<BackgroundService>();
		}

		private async Task Monitore(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				using (var scope = serviceProvider.CreateScope())
				{
					var monitoringService = scope.ServiceProvider.GetService<IMonitoringService>();
					var delay = monitoringService.GetTimeDelay();
					await monitoringService.Update();
					await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
				}
			}
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Start checking sites");
			task = Task.Run(() => Monitore(cancellationToken), cancellationToken);
			task = task.ContinueWith(t => { logger.LogError("Background task fault", t.Exception); },
				cancellationToken, TaskContinuationOptions.OnlyOnFaulted,
				TaskScheduler.Default);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) =>
			Task.CompletedTask;
	}
}