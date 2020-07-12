using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Psi.Files;


namespace ReSharperPlugin.ZeroToolsPluginNet.Actions
{
    [Action("InfoAction", "About Power Tools")]
    public class InfoAction : IActionWithExecuteRequirement, IExecutableAction
    {
        private AspNetZeroRadTool RadTool { get; }

        public InfoAction()
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
            RadTool.ShowAboutForm();
        }
    }
}