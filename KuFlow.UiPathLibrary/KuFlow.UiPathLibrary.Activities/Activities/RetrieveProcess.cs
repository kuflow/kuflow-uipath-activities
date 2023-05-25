using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using KuFlow.Rest;
using KuFlow.UiPathLibrary.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace KuFlow.UiPathLibrary.Activities
{
    [LocalizedDisplayName(nameof(Resources.RetrieveProcess_DisplayName))]
    [LocalizedDescription(nameof(Resources.RetrieveProcess_Description))]
    public class RetrieveProcess : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.RetrieveProcess_ProcessIdentifier_DisplayName))]
        [LocalizedDescription(nameof(Resources.RetrieveProcess_ProcessIdentifier_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> ProcessIdentifier { get; set; }

        [LocalizedDisplayName(nameof(Resources.RetrieveProcess_Process_DisplayName))]
        [LocalizedDescription(nameof(Resources.RetrieveProcess_Process_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<Rest.Models.Process> Process { get; set; }

        #endregion


        #region Constructors

        public RetrieveProcess()
        {
            Constraints.Add(ActivityConstraints.HasParentType<RetrieveProcess, KuFlowScope>(string.Format(Resources.ValidationScope_Error, Resources.KuFlowScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (ProcessIdentifier == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ProcessIdentifier)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(KuFlowScope.ParentContainerPropertyTag);

            // Inputs
            var processIdentifier = ProcessIdentifier.Get(context);

            var client = objectContainer.Get<KuFlowRestClient>();

            var processId = new Guid(processIdentifier);
            var processResponse = client.ProcessClient.RetrieveProcess(processId);
            var process = processResponse.Value;

            // Outputs
            return (ctx) => {
                Process.Set(ctx, process);
            };
        }

        #endregion
    }
}

