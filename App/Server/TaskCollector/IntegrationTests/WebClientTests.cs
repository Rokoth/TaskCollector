using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TaskCollector.IntegrationTests
{
    public class WebClientTests : IClassFixture<CustomFixture>
    {
        private ITestOutputHelper _output;
        public WebClientTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Scenario1Test()
        {
            var projPath = $"TestRun{DateTime.Now:yyyyMMddhhmmss}";
            try
            {                
                BuildProject(projPath);
                var mainProcess = RunProject(projPath);
                await Task.Delay(60000);
                StopProject(mainProcess);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Exception while run test: {ex.Message} {ex.StackTrace}");
            }
            finally 
            {
                _output.WriteLine($"Delete directory: {projPath}");
                Directory.Delete(projPath, true);
            }
        }

        [Fact]
        public async Task Scenario2Test()
        {
            var projPath = $"TestRun{DateTime.Now:yyyyMMddhhmmss}";
            try
            {
                BuildProject(projPath);
                var mainProcess = RunProject(projPath);



                await Task.Delay(60000);
                StopProject(mainProcess);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Exception while run test: {ex.Message} {ex.StackTrace}");
            }
            finally
            {
                _output.WriteLine($"Delete directory: {projPath}");
                Directory.Delete(projPath, true);
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
