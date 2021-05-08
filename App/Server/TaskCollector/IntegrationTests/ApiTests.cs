using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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

        private async Task<bool> WaitForRun(int tryCount, HttpClient httpClient, string baseUri)
        {
            bool run = false;
            int tries = 0;
            while (tries++< tryCount)
            {
                try
                {
                    var ping = await httpClient.GetAsync(baseUri + "/api/v1/ping");
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
            cmd?.Kill();
            cmd?.Close();
            cmd?.WaitForExit(10000);
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
            cmd.WaitForExit();
            _output.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }
}
