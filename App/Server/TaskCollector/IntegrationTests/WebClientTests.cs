using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public class WebClientTests : IClassFixture<CustomFixture>
    {
        private ITestOutputHelper _output;
        private readonly IServiceProvider _serviceProvider;
        private CustomFixture _fixture;

        public WebClientTests(CustomFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _serviceProvider = fixture.ServiceProvider;
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

        [Fact]
        public async Task Scenario3Test()
        {
            var projPath = $"TestRun{DateTime.Now:yyyyMMddhhmmss}";
            Process mainProcess = null;
            IWebDriver driver = null;
            try
            {
                var userRepo = _serviceProvider.GetRequiredService<IRepository<User>>();
                List<Guid> ids = new List<Guid>();
                for (int i = 0; i < 20; i++)
                {
                    var id = Guid.NewGuid();
                    ids.Add(id);
                    await userRepo.AddAsync(new User()
                    {
                        Description = $"user_description_{id}",
                        Id = id,
                        IsDeleted = false,
                        Login = $"user_login_{id}",
                        Name = $"user_name_select_{id}",
                        Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{id}")),
                        VersionDate = DateTimeOffset.Now
                    }, true, CancellationToken.None);
                }

                for (int i = 0; i < 20; i++)
                {
                    var id = Guid.NewGuid();
                    ids.Add(id);
                    await userRepo.AddAsync(new User()
                    {
                        Description = $"user_description_{id}",
                        Id = id,
                        IsDeleted = false,
                        Login = $"user_login_{id}",
                        Name = $"user_name_not_select_{id}",
                        Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{id}")),
                        VersionDate = DateTimeOffset.Now
                    }, true, CancellationToken.None);
                }

                BuildProject(projPath);
                ReplaceConfig(projPath);
                mainProcess = RunProject(projPath);

                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://localhost:5721/");
                Assert.True(driver.Url.Contains("localhost"), "Что-то не так =(");

                var userId = ids.First();
                Authorization(driver, userId);

                var menuButton = driver.FindElement(By.Id("dropdownMenuButton"));
                menuButton.Click();
                var menuUserButton = driver.FindElement(By.Id("MenuUserButton"));
                menuUserButton.Click();

                Assert.True(driver.Url.Contains("User"), "Страница пользователей не открылась");
                await CheckPaging(driver, "userTable", 40, 10);

                var filterName = driver.FindElement(By.Id("filter_name"));
                filterName.SendKeys("user_name_select");
                var filterButton = driver.FindElement(By.Id("refresh_filter_button"));
                filterButton.Click();
                await CheckPaging(driver, "userTable", 20, 10);

                filterName.Clear();
                filterButton.Click();

                var elementsPerPage = driver.FindElement(By.Id("count_items"));
                elementsPerPage.Clear();
                elementsPerPage.SendKeys("7");
                filterButton.Click();
                await CheckPaging(driver, "userTable", 40, 7);

                elementsPerPage.Clear();
                elementsPerPage.SendKeys("10");
                filterButton.Click();

                var row = (await GetRows(driver, "userTable")).First();
                var testId = row.GetProperty("id");
                var testUser = await userRepo.GetAsync(Guid.Parse(testId), CancellationToken.None);

                row.FindElement(By.ClassName("btn-edit")).Click();
                await Task.Delay(500);
                Assert.True(driver.Url.Contains("Edit"), "Страница редактирования не открылась");
                var testEditedUserName = $"user_name_changed_{testId}";
                var testEditedUserDesc = $"user_description_changed_{testId}";

                var nameElement = driver.FindElement(By.Id("Name"));
                nameElement.Clear();
                nameElement.SendKeys(testEditedUserName);

                var descriptionElement = driver.FindElement(By.Id("Description"));
                descriptionElement.Clear();
                descriptionElement.SendKeys(testEditedUserDesc);

                var saveButtonElement = driver.FindElement(By.Id("SaveButton"));
                saveButtonElement.Click();
                await Task.Delay(500);

                Assert.True(driver.Url.Contains("Details"), "Страница детализации не открылась");

                var nameElementCheck = driver.FindElement(By.Id("Name"));
                var descriptionElementCheck = driver.FindElement(By.Id("Description"));
                Assert.Equal(testEditedUserName, nameElementCheck.Text);
                Assert.Equal(testEditedUserDesc, descriptionElementCheck.Text);

                driver.FindElement(By.Id("BackButton")).Click();
                await Task.Delay(500);

                row = (await GetRows(driver, "userTable")).First();
                testId = row.GetProperty("id");
                testUser = await userRepo.GetAsync(Guid.Parse(testId), CancellationToken.None);
                row.FindElement(By.ClassName("btn-details")).Click();
                await Task.Delay(500);
                Assert.True(driver.Url.Contains("Details"), "Страница детализации не открылась");
                nameElementCheck = driver.FindElement(By.Id("Name"));
                descriptionElementCheck = driver.FindElement(By.Id("Description"));
                Assert.Equal(testUser.Name, nameElementCheck.Text);
                Assert.Equal(testUser.Description, descriptionElementCheck.Text);

                driver.FindElement(By.Id("BackButton")).Click();
                await Task.Delay(500);

                driver.FindElement(By.Id("AddButton")).Click();
                await Task.Delay(500);                               

                var newUserId = Guid.NewGuid();
                driver.FindElement(By.Id("Name")).SendKeys($"user_name_new_{newUserId}");
                driver.FindElement(By.Id("Login")).SendKeys($"user_login_new_{newUserId}");
                driver.FindElement(By.Id("Description")).SendKeys($"user_description_new_{newUserId}");
                driver.FindElement(By.Id("Password")).SendKeys($"user_password_new_{newUserId}");

                driver.FindElement(By.Id("SaveButton")).Click();
                await Task.Delay(500);

                Assert.True(driver.Url.Contains("Details"), "Страница детализации не открылась");
                nameElementCheck = driver.FindElement(By.Id("Name"));
                descriptionElementCheck = driver.FindElement(By.Id("Description"));
                Assert.Equal($"user_name_new_{newUserId}", nameElementCheck.Text);
                Assert.Equal($"user_description_new_{newUserId}", descriptionElementCheck.Text);

                var existsUser = await userRepo.GetAsync(new Filter<User>() { 
                   Page = 0, Selector = s=>s.Name == $"user_name_new_{newUserId}", Size = 10
                }, CancellationToken.None);
                var userCount = existsUser.Data.Count();
                Assert.Equal(1, userCount);

                driver.FindElement(By.Id("BackButton")).Click();
                await Task.Delay(500);

                row = (await GetRows(driver, "userTable")).First();
                testId = row.GetProperty("id");
                testUser = await userRepo.GetAsync(Guid.Parse(testId), CancellationToken.None);
                row.FindElement(By.ClassName("btn-delete")).Click();
                await Task.Delay(500);
                Assert.True(driver.Url.Contains("Delete"), "Страница удаления не открылась");

                row.FindElement(By.ClassName("SaveButton")).Click();
                await Task.Delay(500);

                filterName = driver.FindElement(By.Id("filter_name"));
                filterName.SendKeys(testId);
                filterButton = driver.FindElement(By.Id("refresh_filter_button"));
                filterButton.Click();

                var testRows = await GetRows(driver, "userTable");
                var testRowsCount = testRows.Count;
                Assert.Equal(0, testRowsCount);

                testUser = await userRepo.GetAsync(Guid.Parse(testId), CancellationToken.None);
                Assert.Null(testUser);
            }
            catch (Exception ex)
            {               
                _output.WriteLine($"Exception while run test: {ex.Message} {ex.StackTrace}");
                throw;
            }
            finally
            {
                if(driver!=null) driver.Quit();
                if(mainProcess!=null) StopProject(mainProcess);
                _output.WriteLine($"Delete directory: {projPath}");
                //Directory.Delete(projPath, true);
            }
        }

        private static void Authorization(IWebDriver driver, Guid userId)
        {
            var authButton = driver.FindElement(By.Id("AuthButton"));
            authButton.Click();
            var loginField = driver.FindElement(By.Id("Login"));
            loginField.SendKeys($"user_login_{userId}");
            var passwordField = driver.FindElement(By.Id("Password"));
            passwordField.SendKeys($"user_password_{userId}");
            var enterButton = driver.FindElement(By.Id("EnterButton"));
            enterButton.Click();
            Assert.False(driver.Url.Contains("Error"), "Авторизация неудачна");
        }


        private async Task CheckPaging(IWebDriver driver, string tableId, int elementCount, int elementPerPage)
        {
            var pagesCount = (elementCount % elementPerPage == 0) ? (elementCount / elementPerPage) : ((elementCount / elementPerPage) + 1);
            var secondPage = Math.Min(pagesCount, 2);
            var prevPage = Math.Max(pagesCount - 1, 1);
            var lastPageElementsCount = (elementCount % elementPerPage == 0) ? 10 : (elementCount % elementPerPage);

            Func<int, int> GetElementAtPage = s => ElementsAtPage(pagesCount, s, elementPerPage, lastPageElementsCount);

            var beginLink = driver.FindElement(By.Id("begin"));
            var backLink = driver.FindElement(By.Id("back"));
            var forwardLink = driver.FindElement(By.Id("forward"));
            var endLink = driver.FindElement(By.Id("end"));
            
            await CheckPaging(driver, tableId, beginLink, GetElementAtPage, pagesCount, 1);
            await CheckPaging(driver, tableId, backLink, GetElementAtPage, pagesCount, 1);
            await CheckPaging(driver, tableId, forwardLink, GetElementAtPage, pagesCount, secondPage);
            await CheckPaging(driver, tableId, backLink, GetElementAtPage, pagesCount, 1);
            await CheckPaging(driver, tableId, endLink, GetElementAtPage, pagesCount, pagesCount);
            await CheckPaging(driver, tableId, forwardLink, GetElementAtPage, pagesCount, pagesCount);
            await CheckPaging(driver, tableId, backLink, GetElementAtPage, pagesCount, prevPage);
            await CheckPaging(driver, tableId, beginLink, GetElementAtPage, pagesCount, 1);
        }

        private async Task CheckPaging(IWebDriver driver, string tableId, IWebElement link, Func<int, int> getElementAtPage, int pagesCount, int pageNumber)
        {
            link.Click();
            await Task.Delay(500);
            var rows = await GetRows(driver, tableId);
            Assert.Equal(getElementAtPage(pageNumber), rows.Count);
            var all_pages = driver.FindElement(By.Id("all_pages"));
            Assert.Equal(pagesCount.ToString(), all_pages.Text);
            var page = driver.FindElement(By.Id("page"));
            Assert.Equal(pageNumber.ToString(), page.Text);
        }

        private int ElementsAtPage(int pagesCount, int pageNumber, int elementPerPage, int lastPageElementsCount)
        {
            if (pageNumber == pagesCount) return lastPageElementsCount;
            return elementPerPage;
        }

        private async Task<ReadOnlyCollection<IWebElement>> GetRows(IWebDriver driver, string tableName)
        {
            ReadOnlyCollection<IWebElement> rows = null;
            int tryCount = 10;
            while (rows == null)
            {
                try
                {
                    var table = await GetTable(driver, "userTable");
                    rows = table.FindElements(By.CssSelector("tbody tr"));
                }
                catch (StaleElementReferenceException)
                {
                    if (tryCount-- == 0) throw;
                    rows = null;
                }
                await Task.Delay(1000);
            }
            return rows;
        }

        private async Task<IWebElement> GetTable(IWebDriver driver, string id)
        {
            IWebElement table = null;
            int tryCount = 10;
            while (table == null)
            {
                try
                {
                    table = driver.FindElement(By.Id(id));
                }
                catch (NoSuchElementException)
                {
                    if (tryCount-- == 0) throw;
                }
                await Task.Delay(1000);
            }

            return table;
        }

        [Fact]
        public void WebDriverProbeTest()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://habr.com/");
            Assert.True(driver.Url.Contains("habr.com"), "Что-то не так =(");
            driver.Quit();
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
            catch
            {
                
            }
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
