using DickinsonBros.Guid.Abstractions;
using DickinsonBros.IntegrationTest.Models.TestAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RollerCoaster.Account.API.Proxy;
using RollerCoaster.Account.Proxy.Models.CreateUserAccount;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Logic.AccountAPI.APIS.CreateUserAccountTests
{
    [TestAPIAttribute(Name = "CreateUserAccount", Group = "AccountAPI")]
    public class CreateUserAccountTests : ICreateUserAccountTests
    {
        internal readonly IGuidService _guidService;
        internal readonly IAccountProxyService _accountProxyService;
        internal readonly IAccountDBService _accountDBService;

        public CreateUserAccountTests
        (
            IGuidService guidService,
            IAccountProxyService accountProxyService,
            IAccountDBService accountDBService
        )
        {
            _guidService = guidService;
            _accountProxyService = accountProxyService;
            _accountDBService = accountDBService;
        }

        #region CreateUserAccountAsync
        public async Task CreateUserAccountAsync_InvaildEmail_Return400(List<string> successLog)
        {
            //Setup
            var request = new CreateUserAccountRequest
            {
                Email = $"",
                Password = _guidService.NewGuid().ToString(),
                Username = _guidService.NewGuid().ToString()
            };

            try
            {
                //Act
                var response = await _accountProxyService.CreateUserAccountAsync(request).ConfigureAwait(false);

                //Verify

                Assert.AreEqual(400, (int)response.HttpResponseMessage.StatusCode, $"StatusCode. Message: {await response.HttpResponseMessage.Content.ReadAsStringAsync()}");
                successLog.Add("StatusCode is 400");
            }
            finally
            {

            }
        }

        public async Task CreateUserAccountAsync_UsernameLessThen1Char_Return400(List<string> successLog)
        {
            //Setup
            var request = new CreateUserAccountRequest
            {
                Email = $"{_guidService.NewGuid()}@FakeMail.com",
                Password = _guidService.NewGuid().ToString(),
                Username = ""
            };

            try
            {
                //Act
                var response = await _accountProxyService.CreateUserAccountAsync(request).ConfigureAwait(false);

                //Verify

                Assert.AreEqual(400, (int)response.HttpResponseMessage.StatusCode, $"StatusCode. Message: {await response.HttpResponseMessage.Content.ReadAsStringAsync()}");
                successLog.Add("StatusCode is 400");
            }
            finally
            {

            }
        }

        public async Task CreateUserAccountAsync_PasswordLessThen8Chars_Return400(List<string> successLog)
        {
            //Setup
            var request = new CreateUserAccountRequest
            {
                Email = $"{_guidService.NewGuid()}@FakeMail.com",
                Password = "1234567",
                Username = _guidService.NewGuid().ToString(),
            };

            try
            {
                //Act
                var response = await _accountProxyService.CreateUserAccountAsync(request).ConfigureAwait(false);

                //Verify

                Assert.AreEqual(400, (int)response.HttpResponseMessage.StatusCode, $"StatusCode. Message: {await response.HttpResponseMessage.Content.ReadAsStringAsync()}");
                successLog.Add("StatusCode is 400");
            }
            finally
            {

            }
        }

        public async Task CreateUserAccountAsync_DuplicateEmail_Return409(List<string> successLog)
        {
            //Setup
            var firstRequest = new CreateUserAccountRequest
            {
                Email = $"{_guidService.NewGuid()}@FakeMail.com",
                Password = _guidService.NewGuid().ToString(),
                Username = _guidService.NewGuid().ToString(),
            };

            var secondRequest = new CreateUserAccountRequest
            {
                Email = firstRequest.Email,
                Password = _guidService.NewGuid().ToString(),
                Username = _guidService.NewGuid().ToString(),
            };

            try
            {
                //Act

                var firstCallResponse = await _accountProxyService.CreateUserAccountAsync(firstRequest).ConfigureAwait(false);
                var secondCallResponse = await _accountProxyService.CreateUserAccountAsync(secondRequest).ConfigureAwait(false);

                //Verify

                Assert.AreEqual(200, (int)firstCallResponse.HttpResponseMessage.StatusCode, $"StatusCode. First Call Message: {await firstCallResponse.HttpResponseMessage.Content.ReadAsStringAsync()}");
                successLog.Add("Setup - First Call StatusCode is 200");
                Assert.AreEqual(409, (int)secondCallResponse.HttpResponseMessage.StatusCode, $"StatusCode. Second Call Message");
                successLog.Add("Second Call StatusCode is 409");

            }
            finally
            {
                await _accountDBService.DeleteAccountByUsernameAsync(firstRequest.Username).ConfigureAwait(false);
            }
        }

        public async Task CreateUserAccountAsync_DuplicateUsername_Return409(List<string> successLog)
        {
            //Setup
            var firstRequest = new CreateUserAccountRequest
            {
                Email = $"{_guidService.NewGuid()}@FakeMail.com",
                Password = _guidService.NewGuid().ToString(),
                Username = _guidService.NewGuid().ToString(),
            };

            var secondRequest = new CreateUserAccountRequest
            {
                Email = $"{_guidService.NewGuid()}@FakeMail.com",
                Password = _guidService.NewGuid().ToString(),
                Username = firstRequest.Username
            };

            try
            {
                //Act
                var firstCallResponse = await _accountProxyService.CreateUserAccountAsync(firstRequest).ConfigureAwait(false);
                var secondCallResponse = await _accountProxyService.CreateUserAccountAsync(secondRequest).ConfigureAwait(false);

                //Verify

                Assert.AreEqual(200, (int)firstCallResponse.HttpResponseMessage.StatusCode, $"StatusCode. First Call Message: {await firstCallResponse.HttpResponseMessage.Content.ReadAsStringAsync()}");
                successLog.Add("Setup - First Call StatusCode is 200");
                Assert.AreEqual(409, (int)secondCallResponse.HttpResponseMessage.StatusCode, $"StatusCode. Second Call Message");
                successLog.Add("Second Call StatusCode is 409");
            }
            finally
            {
                await _accountDBService.DeleteAccountByUsernameAsync(firstRequest.Username).ConfigureAwait(false);
            }
        }

        public async Task CreateUserAccountAsync_NewUserWithoutEmail_Return200(List<string> successLog)
        {
            //Setup
            var request = new CreateUserAccountRequest
            {
                Email = null,
                Password = _guidService.NewGuid().ToString(),
                Username = _guidService.NewGuid().ToString(),
            };

            try
            {
                //Act

                var response = await _accountProxyService.CreateUserAccountAsync(request).ConfigureAwait(false);

                //Verify
                Assert.AreEqual(200, (int)response.HttpResponseMessage.StatusCode, $"StatusCode");
                successLog.Add("StatusCode is 200");

                var account = await _accountDBService.SelectAccountByUsernameAsync(request.Username).ConfigureAwait(false);

                Assert.IsNotNull(account, "AccountDBService.SelectAccountByUsernameAsync - Returned Null");
                successLog.Add($"AccountDBService.SelectAccountByUsernameAsync - Returned A Value");

                Assert.IsNull(account.Email, $"AccountDBService.SelectAccountByUsernameAsync - Email is not null");
                successLog.Add($"AccountDBService.SelectAccountByUsernameAsync - Email is null");

                Assert.AreEqual(request.Username, account.Username, $"AccountDBService.SelectAccountByUsernameAsync - Username does not match");
                successLog.Add($"AccountDBService.SelectAccountByUsernameAsync - Username matchs");

                Assert.AreEqual(false, account.Locked, $"AccountDBService.SelectAccountByUsernameAsync - Account Locked");
                successLog.Add($"AccountDBService.SelectAccountByUsernameAsync - Account is not locked");

                Assert.AreEqual(Role.User, account.Role, $"AccountDBService.SelectAccountByUsernameAsync - Role is not user");
                successLog.Add($"AccountDBService.SelectAccountByUsernameAsync - Role is user");

            }
            finally
            {
                await _accountDBService.DeleteAccountByUsernameAsync(request.Username).ConfigureAwait(false);
            }
        }

        public async Task CreateUserAccountAsync_NewUserWithEmail_Return200(List<string> successLog)
        {
            //Setup
            var request = new CreateUserAccountRequest
            {
                Email = $"{_guidService.NewGuid()}@FakeMail.com",
                Password = _guidService.NewGuid().ToString(),
                Username = _guidService.NewGuid().ToString(),
            };

            try
            {
                //Act

                var response = await _accountProxyService.CreateUserAccountAsync(request).ConfigureAwait(false);

                //Verify
                Assert.AreEqual(200, (int)response.HttpResponseMessage.StatusCode);
                successLog.Add("StatusCode is 200");

                var account = await _accountDBService.SelectAccountByEmailAsync(request.Email).ConfigureAwait(false);

                Assert.IsNotNull(account, "AccountDBService.SelectAccountByEmailAsync - Returned Null");
                successLog.Add($"Post - Db lookup User by email Returned A Value");

                Assert.AreEqual(request.Email, account.Email, $"Post - Db lookup Email does not match");
                successLog.Add($"Post - Db lookup User by email Email matchs");

                Assert.AreEqual(request.Username, account.Username, $"Post - Db lookup Username does not match");
                successLog.Add($"Post - Db lookup Username matchs");

                Assert.AreEqual(false, account.Locked, $"Post - Db lookup Account Locked");
                successLog.Add($"Post - Db lookup Account is not locked");

                Assert.AreEqual(Role.User, account.Role, $"Post - Db lookup Role is not user");
                successLog.Add($"Post - Db lookup Role is user");
            }
            finally
            {
                await _accountDBService.DeleteAccountByUsernameAsync(request.Username).ConfigureAwait(false);
            }
        }
        #endregion
    }
}
