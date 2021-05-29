using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Interface;
using TaskCollector.Db.Model;
using Xunit;
using Xunit.Abstractions;

namespace TaskCollector.IntegrationTests
{
    public class ApiTests : IClassFixture<CustomFixture>
    {
        private ITestOutputHelper _output;
        private CustomFixture _fixture;

        public ApiTests(CustomFixture fixture, ITestOutputHelper output)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task Scenario1Test()
        {            
            var projPath = $"TestRun{DateTime.Now:yyyyMMddhhmmss}";
            try
            {
                BuildProject(projPath);
                ReplaceConfig(projPath);
                var mainProcess = RunProject(projPath);
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://0.0.0.0:5721");
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var run = await WaitForRun(10, httpClient, "https://0.0.0.0:5721");
                Assert.True(run);



                await Task.Delay(10000);
                StopProject(mainProcess);
                _output.WriteLine($"Delete directory: {projPath}");
                Directory.Delete(projPath, true);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Exception while run test: {ex.Message} {ex.StackTrace}");
            }
            
        }

        [Fact]
        public async Task Scenario2Test()
        {
            var projPath = $"TestRun{DateTime.Now:yyyyMMddhhmmss}";
            Process mainProcess = null;
            try
            {
                BuildProject(projPath);
                ReplaceConfig(projPath);

                var userRepo = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
                var clientRepo = _fixture.ServiceProvider.GetRequiredService<IRepository<Client>>();

                var userId = Guid.NewGuid();
                var user = new User()
                {
                    Description = $"user_description_{userId}",
                    Id = userId,
                    IsDeleted = false,
                    Login = $"user_login_{userId}",
                    Name = $"user_name_select_{userId}",
                    Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{userId}")),
                    VersionDate = DateTimeOffset.Now
                };

                var clientId = Guid.NewGuid();
                var client = new Client()
                {
                    Description = $"client_description_{clientId}",
                    Id = clientId,
                    IsDeleted = false,
                    Login = $"client_login_{clientId}",
                    Name = $"client_name_select_{clientId}",
                    Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"client_password_{clientId}")),
                    MappingRules = "{}",
                    UserId = userId,
                    VersionDate = DateTimeOffset.Now
                };

                await userRepo.AddAsync(user, true, CancellationToken.None);
                await clientRepo.AddAsync(client, true, CancellationToken.None);

                mainProcess = RunProject(projPath);
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:5721");
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var run = await WaitForRun(10, httpClient, "https://localhost:5721");
                Assert.True(run);

                var htppClient = new HttpClient();
                var clientIdentity = new Contract.Model.ClientIdentity() { 
                   Login = client.Login,
                   Password = "wrong_password"
                };
                var authResult = await htppClient.PostAsync("https://localhost:5721/api/v1/client/auth", clientIdentity.SerializeRequest());
                Assert.True(authResult.StatusCode == System.Net.HttpStatusCode.BadRequest);

                var message = new Dictionary<string, object>() {
                    { "Field1", "Value1"},  { "Field2", "Value2"}
                };
                var sendResult = await htppClient.PostAsync("https://localhost:5721/api/v1/message/send", message.SerializeRequest());
                Assert.True(sendResult.StatusCode == System.Net.HttpStatusCode.Unauthorized);

                await Task.Delay(10000);
                
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Exception while run test: {ex.Message} {ex.StackTrace}");
                throw;
            }
            finally
            {
               
                if (mainProcess != null) StopProject(mainProcess);
                _output.WriteLine($"Delete directory: {projPath}");
                //Directory.Delete(projPath, true);
            }
        }
            


        private async Task<bool> WaitForRun(int tryCount, HttpClient httpClient, string baseUri)
        {
            bool run = false;
            int tries = 0;
            while (tries++< tryCount)
            {
                try
                {
                    var ping = await httpClient.GetAsync(baseUri + "/api/v1/common/ping");
                    if (ping.IsSuccessStatusCode)
                    {
                        run = true;
                        break;
                    }
                    _output.WriteLine($"Ping result: {ping.StatusCode} {ping.ReasonPhrase}");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"WaitForRun try: {tries} exception: {ex.Message}");
                }
                await Task.Delay(1000);
            }
            return run;
        }

        private void ReplaceConfig(string projPath)
        {
            var configFilePath = Path.Combine(projPath, "appsettings.json");
            string config = "";
            using (var stream = new StreamReader(configFilePath))
            {
                config = stream.ReadToEnd();
            }
            var configJson = JObject.Parse(config);
            configJson["ConnectionStrings"]["MainConnection"] = _fixture.ConnectionString;

            using (var writer = new StreamWriter(configFilePath, false))
            {
                writer.Write(configJson.ToString());
            }
        }        
                
        private Process RunProject(string projPath)
        {
            _output.WriteLine($"Run project");
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{projPath}\\TaskCollector.exe";
            cmd.Start();
            return cmd;
        }

        private void StopProject(Process cmd)
        {
            try
            {
                cmd?.Kill();
                cmd?.Close();
                cmd?.WaitForExit(10000);
            }
            catch (Exception)
            {
                
            }
        }

        private void BuildProject(string projPath)
        {
            _output.WriteLine($"Build project to path: {projPath}");
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;           
            cmd.StartInfo.UseShellExecute = false;            
            cmd.Start();

            var command = $"dotnet build ..\\..\\..\\..\\TaskCollector\\TaskCollector.csproj -o {projPath}";

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            Thread.Sleep(30000);
            _output.WriteLine(cmd.StandardOutput.ReadToEnd());
            cmd.Kill();
            cmd.WaitForExit(10000);            
        }
    }

    public static class HttpApiHelper
    {
        public static StringContent SerializeRequest<TReq>(this TReq entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }

        public static async Task<TResp> ParseResponse<TResp>(this HttpResponseMessage result) where TResp : class
        {
            if (result != null && result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                return JObject.Parse(response).ToObject<TResp>();
            }
            return null;
        }

        public static async Task<(int, IEnumerable<T>)> ParseResponseArray<T>(this HttpResponseMessage result) where T : class
        {
            if (result != null && result.IsSuccessStatusCode)
            {
                var ret = new List<T>();
                var response = await result.Content.ReadAsStringAsync();
                foreach (var item in JArray.Parse(response))
                {
                    ret.Add(item.ToObject<T>());
                }
                int count = 0;
                if (result.Headers.TryGetValues("x-pages", out IEnumerable<string> pageHeaders))
                {
                    int.TryParse(pageHeaders.FirstOrDefault(), out count);
                }
                return (count, ret);
            }
            return (0, null);
        }
    }
}
