using Microsoft.Deployment.WindowsInstaller;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CustomActionPreInstall(Session session)
        {
            session.Log("Begin CustomActionPreInstall");
            try
            {
                System.Diagnostics.Process.Start($"sc", "stop TaskCollector");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                session.Log($"Error in CustomAction: {ex.Message} : {ex.StackTrace}");
            }
            session.Log("End CustomActionPreInstall");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult CustomActionPreUninstall(Session session)
        {
            session.Log("Begin CustomActionPreUninstall");
            try
            {
                System.Diagnostics.Process.Start($"sc", "stop TaskCollector");
                Thread.Sleep(2000);
                System.Diagnostics.Process.Start($"sc", "delete TaskCollector");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                session.Log($"Error in CustomAction: {ex.Message} : {ex.StackTrace}");
            }
            session.Log("End CustomActionPreUninstall");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult CustomActionInstall(Session session)
        {
            session.Log("Begin CustomAction");
            try
            {
                try
                {
                    for (int count = 0; count <= 5; count++)
                    {
                        session.Log($"appsettings try number {count}");
                        if (File.Exists($"{session.GetTargetPath("INSTALLFOLDER")}appsettings.json")) break;
                        session.Log($"{session.GetTargetPath("INSTALLFOLDER")}appsettings.json does not exists");
                        Thread.Sleep(1000);
                    }

                    string appsettings = "";
                    using (var reader = new StreamReader($"{session.GetTargetPath("INSTALLFOLDER")}appsettings.json"))
                    {
                        appsettings = reader.ReadToEnd();
                    }

                    JObject jObject = JObject.Parse(appsettings);
                    var log_path = @session["LOG_PATH"];

                    jObject["Serilog"]["WriteTo"][0]["Args"]["configure"][1]["Args"]["pathFormat"] = log_path + "\\log-{Date}.txt";
                    //todo: add certificates
                    //jObject["Kestrel"]["Endpoints"]["Https"]["Url"] = $"https://localhost:{session["HTTPS_PORT"]}";
                    //jObject["Kestrel"]["Endpoints"]["Https"]["Certificate"]["Path"] = session["SSL_CERT"];
                    //jObject["Kestrel"]["Endpoints"]["Https"]["Certificate"]["Password"] = session["SSL_CERT_PASS"];

                    jObject["ConnectionStrings"]["MainConnection"] = session["CONN_STRING"];

                    jObject["NotifyOptions"]["Credentials"]["FromEmail"] = session["FROM_EMAIL"];
                    jObject["NotifyOptions"]["Credentials"]["FromName"] = session["FROM_NAME"];
                    jObject["NotifyOptions"]["Credentials"]["FromPassword"] = session["FROM_PASSWORD"];
                  
                    using (var writer = new StreamWriter($"{session.GetTargetPath("INSTALLFOLDER")}appsettings.json", false))
                    {
                        writer.Write(jObject.ToString());
                    }
                }
                catch
                {
                    session["INSTALL_STEP"] = "Сохранение конфигурации";
                    throw;
                }

                try
                {
                    if (session["INSTALL_SERVICE"] == "1")
                    {
                        System.Diagnostics.Process.Start($"{session.GetTargetPath("INSTALLFOLDER")}TaskCollector.exe", "install");
                    }

                    if (session["START_SERVICE"] == "1")
                    {
                        Thread.Sleep(3000);
                        System.Diagnostics.Process.Start($"{session.GetTargetPath("INSTALLFOLDER")}TaskCollector.exe", "start");
                        Thread.Sleep(3000);
                    }
                }
                catch
                {
                    session["INSTALL_STEP"] = "Создание службы";
                    throw;
                }                               
            }
            catch (Exception ex)
            {
                session.Log($"Error in CustomAction: {ex.Message} : {ex.StackTrace}");
                session["CST_INSTALLED"] = "0";
                session["INSTALL_MESSAGE"] = ex.Message;
                return ActionResult.Failure;
            }
            session.Log("End CustomAction");
            return ActionResult.Success;
        }

    }
}
