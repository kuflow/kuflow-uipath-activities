using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using KuFlow.Rest;
using KuFlow.Rest.Models;
using KuFlow.UiPathLibrary.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace KuFlow.UiPathLibrary.Activities
{
    [LocalizedDisplayName(nameof(Resources.DeleteElement_DisplayName))]
    [LocalizedDescription(nameof(Resources.DeleteElement_Description))]
    public class DeleteElement : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.DeleteElement_TaskIdentifier_DisplayName))]
        [LocalizedDescription(nameof(Resources.DeleteElement_TaskIdentifier_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> TaskIdentifier { get; set; }

        [LocalizedDisplayName(nameof(Resources.DeleteElement_Code_DisplayName))]
        [LocalizedDescription(nameof(Resources.DeleteElement_Code_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Code { get; set; }

        [LocalizedDisplayName(nameof(Resources.DeleteElement_Task_DisplayName))]
        [LocalizedDescription(nameof(Resources.DeleteElement_Task_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<Rest.Models.Task> Task { get; set; }

        #endregion


        #region Constructors

        public DeleteElement()
        {
            Constraints.Add(ActivityConstraints.HasParentType<DeleteElement, KuFlowScope>(string.Format(Resources.ValidationScope_Error, Resources.KuFlowScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (TaskIdentifier == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TaskIdentifier)));
            if (Code == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Code)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(KuFlowScope.ParentContainerPropertyTag);

            // Inputs
            var taskIdentifier = TaskIdentifier.Get(context);
            var code = Code.Get(context);

            var client = objectContainer.Get<KuFlowRestClient>();

            var taskId = new Guid(taskIdentifier);
            var command = new TaskDeleteElementCommand(code);

            var taskResponse = client.TaskClient.ActionsTaskDeleteElement(taskId, command);
            var task = taskResponse.Value;

            // Outputs
            return (ctx) => {
                Task.Set(ctx, task);
            };
        }

        #endregion
    }
}

