using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Threading;
using System.Threading.Tasks;
using KuFlow.Rest;
using KuFlow.UiPathLibrary.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace KuFlow.UiPathLibrary.Activities
{
    [LocalizedDisplayName(nameof(Resources.AppendALogToTheTask_DisplayName))]
    [LocalizedDescription(nameof(Resources.AppendALogToTheTask_Description))]
    public class AppendALogToTheTask : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.AppendALogToTheTask_TaskIdentifier_DisplayName))]
        [LocalizedDescription(nameof(Resources.AppendALogToTheTask_TaskIdentifier_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> TaskIdentifier { get; set; }

        [LocalizedDisplayName(nameof(Resources.AppendALogToTheTask_Message_DisplayName))]
        [LocalizedDescription(nameof(Resources.AppendALogToTheTask_Message_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Message { get; set; }

        [LocalizedDisplayName(nameof(Resources.AppendALogToTheTask_Level_DisplayName))]
        [LocalizedDescription(nameof(Resources.AppendALogToTheTask_Level_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Rest.Models.LogLevel> Level { get; set; }

        #endregion


        #region Constructors

        public AppendALogToTheTask()
        {
            Constraints.Add(ActivityConstraints.HasParentType<AppendALogToTheTask, KuFlowScope>(string.Format(Resources.ValidationScope_Error, Resources.KuFlowScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (TaskIdentifier == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TaskIdentifier)));
            if (Message == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Message)));
            if (Level == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Level)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(KuFlowScope.ParentContainerPropertyTag);

            // Inputs
            var taskIdentifier = TaskIdentifier.Get(context);

            var message = Message.Get(context);
            var level = Level.Get(context);

            var client = objectContainer.Get<KuFlowRestClient>();

            var taskId = new Guid(taskIdentifier);
            var taskResponse = client.TaskClient.ActionsTaskAppendLog(taskId, new Rest.Models.Log(message, level));
            var task = taskResponse.Value;

            // Outputs
            return (ctx) => {
            };
        }

        #endregion
    }
}

