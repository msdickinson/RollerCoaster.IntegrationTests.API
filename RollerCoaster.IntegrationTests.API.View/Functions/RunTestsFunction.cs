using DickinsonBros.IntegrationTest;
using DickinsonBros.IntegrationTest.Models.TestAutomation;
using DickinsonBros.Logger.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using RollerCoaster.IntegrationTests.API.Logic.AccountAPI.APIS.CreateUserAccountTests;
using RollerCoaster.IntegrationTests.API.Logic.TestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.View.Functions
{
    public class RunTestsFunction
    {
        #region constants
        internal const string FunctionName = "TestAutomationFunction";
        internal const string ZIP_CONTENT_TYPE = "application/zip";
        #endregion

        #region members
        internal readonly ILoggingService<RunTestsFunction> _loggingService;
        internal readonly IIntegrationTestService _integrationTestService;
        internal readonly IServiceProvider _serviceProvider;
        #endregion

        #region .ctor
        public RunTestsFunction
        (
            IServiceProvider serviceProvider,
            IIntegrationTestService integrationTestService,
            ILoggingService<RunTestsFunction> loggingService
        )
        {
            _serviceProvider = serviceProvider;
            _integrationTestService = integrationTestService;
            _loggingService = loggingService;
        }
        #endregion
        #region function
        [FunctionName(FunctionName)]
        public async Task<IActionResult> RunTestsAsync
        (
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "RunTests")] HttpRequest req
        )
        {
            var transactionId = Guid.NewGuid().ToString();
            var methodIdentifier = $"{nameof(RunTestsFunction)}.{nameof(RunTestsFunction.RunTestsAsync)}";
            try
            {
                _loggingService.LogInformationRedacted
                (
                    $"+ {methodIdentifier}",
                    new Dictionary<string, object>()
                    {
                        { nameof(transactionId), transactionId }
                    }
                );

                var tests = new List<Test>();

                //Add Tests
                tests.AddRange(AccountAPITests());

                //Test Group Filter
                if (req != null && req.Query["TestGroup"].Any())
                {
                    tests = tests.Where(e => e.TestGroup == req.Query["TestGroup"].First()).ToList();
                }

                //Addtional Filter
                tests = tests.Where(e => e.MethodInfo.Name.Contains("")).ToList();

                //Run Tests
                var testSummary = await _integrationTestService.RunTests(tests).ConfigureAwait(false);
                
                //Process Tests
                var reportTRX = _integrationTestService.GenerateTRXReport(testSummary);
                var log = _integrationTestService.GenerateLog(testSummary, false);
                var zipBytes = await _integrationTestService.GenerateZip(reportTRX, log).ConfigureAwait(false);

                //Return Results
                _loggingService.LogInformationRedacted(log);

                return new ContentResult
                {
                    Content = log,
                    ContentType = "text",
                    StatusCode = 200
                };

                //return new FileContentResult(zipBytes.ToArray(), ZIP_CONTENT_TYPE)
                //{
                //    FileDownloadName = $"Report {DateTime.Now:MM/dd/yyyy h:mm tt}.zip"
                //};
            }
            catch (Exception exception)
            {
                _loggingService.LogErrorRedacted
                (
                    methodIdentifier,
                    exception,
                    new Dictionary<string, object>()
                    {
                        { nameof(transactionId), transactionId }
                    }
                );
                var result = new ObjectResult("");
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }
            finally
            {
                _loggingService.LogInformationRedacted
                (
                    $"- {methodIdentifier}",
                    new Dictionary<string, object>()
                    {
                        { nameof(transactionId), transactionId }
                    }
                );
            }
        }

        #endregion

        private IEnumerable<Test> AccountAPITests()
        {
            var tests = new List<Test>();

            tests.AddRange(_integrationTestService.SetupTests(_serviceProvider.GetService(typeof(ICreateUserAccountTests))));

            return tests;
        }
    }
}
