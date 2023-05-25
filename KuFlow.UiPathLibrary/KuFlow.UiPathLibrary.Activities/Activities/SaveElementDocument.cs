using System;
using System.Activities;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KuFlow.Rest;
using KuFlow.UiPathLibrary.Activities.Properties;
using KuFlow.UiPathLibrary.Utils;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace KuFlow.UiPathLibrary.Activities
{
    [LocalizedDisplayName(nameof(Resources.SaveElementDocument_DisplayName))]
    [LocalizedDescription(nameof(Resources.SaveElementDocument_Description))]
    public class SaveElementDocument : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElementDocument_TaskIdentifier_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElementDocument_TaskIdentifier_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> TaskIdentifier { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElementDocument_Code_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElementDocument_Code_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Code { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElementDocument_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElementDocument_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElementDocument_Id_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElementDocument_Id_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Id { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElementDocument_Valid_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElementDocument_Valid_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<bool?> Valid { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElementDocument_Task_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElementDocument_Task_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<Rest.Models.Task> Task { get; set; }

        #endregion


        #region Constructors

        public SaveElementDocument()
        {
            Constraints.Add(ActivityConstraints.HasParentType<SaveElementDocument, KuFlowScope>(string.Format(Resources.ValidationScope_Error, Resources.KuFlowScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (TaskIdentifier == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TaskIdentifier)));
            if (Code == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Code)));
            if (FilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilePath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(KuFlowScope.ParentContainerPropertyTag);

            // Inputs
            var taskIdentifier = TaskIdentifier.Get(context);
            var code = Code.Get(context);
            var filePath = FilePath.Get(context);
            var id = Id.Get(context);
            var valid = Valid.Get(context);

            var client = objectContainer.Get<KuFlowRestClient>();
            var taskId = new Guid(taskIdentifier);
            Guid? elementValueId = string.IsNullOrWhiteSpace(id) ? null : new Guid(id);

            Rest.Models.Task task = null;

            var fileContentType = FiletUtils.GuessMimeTypeFromPath(filePath);
            var fileName = Path.GetFileName(filePath);

            using (Stream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var taskResponse = client.TaskClient.ActionsTaskSaveElementValueDocument(taskId, fileContentType, fileName, code, fileStream, elementValueId, valid);
                task = taskResponse.Value;
            }


            // Outputs
            return (ctx) =>
            {
                Task.Set(ctx, task);
            };
        }

        #endregion
    }
}

