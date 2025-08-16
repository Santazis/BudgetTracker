using BenchmarkDotNet.Running;
using BudgetTracker.Infrastructure.Database;
using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;

BenchmarkRunner.Run<EfCoreBenchmarks>();