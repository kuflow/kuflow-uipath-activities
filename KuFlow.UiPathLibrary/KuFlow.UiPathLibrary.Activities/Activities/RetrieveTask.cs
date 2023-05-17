using System;
using System.Activities;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using KuFlow.Rest;
using KuFlow.UiPathLibrary.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace KuFlow.UiPathLibrary.Activities
{
    [LocalizedDisplayName(nameof(Resources.RetrieveTask_DisplayName))]
    [LocalizedDescription(nameof(Resources.RetrieveTask_Description))]
    public class RetrieveTask : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.RetrieveTask_TaskIdentifier_DisplayName))]
        [LocalizedDescription(nameof(Resources.RetrieveTask_TaskIdentifier_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> TaskIdentifier { get; set; }

        [LocalizedDisplayName(nameof(Resources.RetrieveTask_Task_DisplayName))]
        [LocalizedDescription(nameof(Resources.RetrieveTask_Task_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public OutArgument<Rest.Models.Task> Task { get; set; }
        #endregion


        #region Constructors

        public RetrieveTask()
        {
            Constraints.Add(ActivityConstraints.HasParentType<RetrieveTask, KuFlowScope>(string.Format(Resources.ValidationScope_Error, Resources.KuFlowScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (TaskIdentifier == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TaskIdentifier)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(KuFlowScope.ParentContainerPropertyTag);

            // Inputs
            var taskIdentifier = TaskIdentifier.Get(context);

            var client = objectContainer.Get<KuFlowRestClient>();

            var taskId = new Guid(taskIdentifier);
            var taskResponse = client.TaskClient.RetrieveTask(taskId);
            var task = taskResponse.Value;

            // Outputs
            return (ctx) => {
                Task.Set(ctx, task);
            };
        }

        #endregion
    }
}

