using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AspNetZeroRadToolVisualStudioExtension.Dialogs;
using AspNetZeroRadToolVisualStudioExtension.Dialogs.EntityLoaders;
using AspNetZeroRadToolVisualStudioExtension.Dialogs.Providers;
using AspNetZeroRadToolVisualStudioExtension.EntityInfo;
using AspNetZeroRadToolVisualStudioExtension.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;

namespace ReSharperPlugin.ZeroToolsPluginNet.Actions
{
    public class AspNetZeroRadTool
    {
        private static int ProjectVersionToNumber(string version)
        {
            string[] strArray = version.Replace("v", "").Split('.');
            string str1 = "";
            foreach (string str2 in strArray)
            {
                string str3 = "";
                for (int index = 0; index < 2 - str2.Length; ++index)
                    str3 += "0";
                str1 = str1 + str3 + str2;
            }

            try
            {
                return Convert.ToInt32(str1);
            }
            catch (Exception ex)
            {
                return 50500;
            }
        }

        private Entity DetectInformationOfProject(string slnFile)
        {
            Entity entity = new Entity();
            string directoryName = Path.GetDirectoryName(slnFile);
            if (!File.Exists(directoryName + "\\AspNetZeroRadTool\\config.json"))
                throw new Exception("Config file not found!");
            JObject jobject = JObject.Parse(File.ReadAllText(directoryName + "\\AspNetZeroRadTool\\config.json"));
            entity.ProjectName = (string) jobject["ProjectName"];
            entity.CompanyName = (string) jobject["CompanyName"];
            entity.ProjectType = (string) jobject["ProjectType"];
            entity.ProjectVersion = (string) (jobject["ProjectVersion"] ?? "v1.0.0");
            EntityProvider.Namespace = entity.Namespace;
            entity.AppAreaName =
                entity.ProjectType.Equals("Mvc") ? (string) jobject["ApplicationAreaName"] : "NotNeeded";
            entity.RootPath = directoryName;
            entity.EntityHistoryDisabled = ProjectVersionToNumber(entity.ProjectVersion) < 60300;
            entity.Properties = new List<Property>();
            entity.PagePermission = new PagePermissionInfo();
            entity.EnumDefinitions = new List<EnumDefinition>();
            entity.NavigationProperties = new List<NavigationProperty>();
            return entity;
        }

        public void ShowAboutForm()
        {
            new AboutForm(() => { }).Show();
        }

        public void MenuItemCallback(string solutionFullName, bool regenerate = false, bool loadFromDatabase = false)
        {
            Entity entity;
            try
            {
                entity = DetectInformationOfProject(solutionFullName);
                EnumProvider.SetSolutionPathForEnums(Path.GetDirectoryName(solutionFullName) + "\\src");
                EntityProvider.SetSolutionPathForEntities(Path.GetDirectoryName(solutionFullName) + "\\src\\" +
                                                          entity.Namespace + ".Core");
                DbContextProvider.SetSolutionPathForDbContexts(Path.GetDirectoryName(solutionFullName) + "\\src\\" +
                                                               entity.Namespace + ".EntityFrameworkCore");
                AppSettings.PowerToolsDirectoryName = "AspNetZeroRadTool";
            }
            catch (Exception ex)
            {
                MsgBox.Exception("Couldn't load information of your ASP.NET Zero project!", ex);
                return;
            }

            try
            {
                if (regenerate)
                {
                    using (JsonEntityForm jsonEntityForm = new JsonEntityForm(Path.GetDirectoryName(solutionFullName)))
                    {
                        if (jsonEntityForm.ShowDialog() != DialogResult.OK)
                            return;
                        entity = EntityLoaderFromJson.Build(
                            File.ReadAllText(Path.GetDirectoryName(solutionFullName) + "\\AspNetZeroRadTool\\" +
                                             jsonEntityForm.SelectedFile + ".json"), entity);
                    }
                }
                else if (loadFromDatabase)
                {
                    using (DbTableSelectionForm tableSelectionForm =
                        new DbTableSelectionForm(Path.GetDirectoryName(solutionFullName), entity))
                    {
                        if (tableSelectionForm.ShowDialog() != DialogResult.OK)
                            return;
                        entity = tableSelectionForm.SelectedTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Exception("Couldn't load information of the entity!", ex);
                return;
            }

            try
            {
                var form = new EntityGeneratorForm(entity);

                try
                {
                    if (MultiTenancyChecker.IsEnabled(entity))
                        return;
                    form.AllowNoneMultiTenancy = true;
                }
                catch (Exception ex)
                {
                    MsgBox.Exception("Cannot determine multi-tenancy !", ex);
                }

                form.Show();
            }
            catch (Exception ex)
            {
                MsgBox.Exception("Encountered a fatal error!", ex);
            }
        }
    }
}