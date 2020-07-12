using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Psi.Files;


namespace ReSharperPlugin.ZeroToolsPluginNet.Actions
{
    [Action("LoadEntityFromDatabaseAction", "Load Entity From Database")]
    public class LoadEntityFromDatabaseAction : IActionWithExecuteRequirement, IExecutableAction
    {
        private AspNetZeroRadTool RadTool { get; }

        public LoadEntityFromDatabaseAction()
        {
            RadTool = new AspNetZeroRadTool();
        }

        public IActionRequirement GetRequirement(IDataContext dataContext)
        {
            return CommitAllDocumentsRequirement.TryGetInstance(dataContext);
        }

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var solutionPath = context.Projects().Solution.SolutionFilePath.FullPath;
            RadTool.MenuItemCallback(solutionPath, false, true);
        }
    }
}