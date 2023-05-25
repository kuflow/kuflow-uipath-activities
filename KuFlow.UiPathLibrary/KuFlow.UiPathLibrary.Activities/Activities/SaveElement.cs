using System;
using System.Activities;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using KuFlow.Rest;
using KuFlow.Rest.Models;
using KuFlow.UiPathLibrary.Activities.Properties;
using KuFlow.UiPathLibrary.Utils;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace KuFlow.UiPathLibrary.Activities
{
    [LocalizedDisplayName(nameof(Resources.SaveElement_DisplayName))]
    [LocalizedDescription(nameof(Resources.SaveElement_Description))]
    public class SaveElement : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElement_TaskIdentifier_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElement_TaskIdentifier_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> TaskIdentifier { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElement_Code_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElement_Code_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Code { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElement_Value_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElement_Value_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<object> Value { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElement_Valid_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElement_Valid_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<bool?> Valid { get; set; }

        [LocalizedDisplayName(nameof(Resources.SaveElement_Task_DisplayName))]
        [LocalizedDescription(nameof(Resources.SaveElement_Task_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public OutArgument<Rest.Models.Task> Task { get; set; }
        #endregion


        #region Constructors

        public SaveElement()
        {
            Constraints.Add(ActivityConstraints.HasParentType<SaveElement, KuFlowScope>(string.Format(Resources.ValidationScope_Error, Resources.KuFlowScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (TaskIdentifier == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TaskIdentifier)));
            if (Code == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Code)));
            if (Value == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Value)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(KuFlowScope.ParentContainerPropertyTag);

            // Inputs
            var taskIdentifier = TaskIdentifier.Get(context);
            var code = Code.Get(context);
            var value = Value.Get(context);
            var valid = Valid.Get(context);

            var client = objectContainer.Get<KuFlowRestClient>();

            var taskId = new Guid(taskIdentifier);
            var command = new TaskSaveElementCommand(code);
            var elementValues = ToTaskElementValues(value, valid);
            foreach (var elementValue in elementValues)
            {
                command.ElementValues.Add(elementValue);
            }

            var taskResponse = client.TaskClient.ActionsTaskSaveElement(taskId, command);
            var task = taskResponse.Value;

            // Outputs
            return (ctx) =>
            {
                Task.Set(ctx, task);
            };
        }

        private List<TaskElementValue> ToTaskElementValues(object value, bool? valid)
        {
            List<TaskElementValue> target = new List<TaskElementValue>();

            if (value is IList list)
            {
                foreach (var item in list)
                {
                    target.Add(ToTaskElementValue(item, valid));
                }
            }
            else if (value is Array array)
            {
                foreach (var item in array)
                {
                    target.Add(ToTaskElementValue(item, valid));
                }
            }
            else
            {
                target.Add(ToTaskElementValue(value, valid));
            }

            return target;
        }


        private TaskElementValue ToTaskElementValue(object value, bool? valid)
        {
            value ??= "";

            if (value is string stringValue)
            {
                return new TaskElementValueString
                {
                    Value = stringValue,
                    Valid = valid
                };
            }
            else if (ObjectUtils.IsNumber(value))
            {
                return new TaskElementValueNumber
                {
                    Value = Convert.ToDouble(value),
                    Valid = valid
                };
            }
            else if (value is TaskElementValuePrincipalItem principalItemValue)
            {
                return new TaskElementValuePrincipal
                {
                    Value = principalItemValue,
                    Valid = valid
                };
            }
            else if (value is TaskElementValueDocumentItem documentItemValue)
            {
                return new TaskElementValueDocument
                {
                    Value = documentItemValue,
                    Valid = valid
                };
            }
            else if (value is IDictionary<string, object> dictionaryValue)
            {
                var elementValueObject = new TaskElementValueObject
                {
                    Valid = valid
                };

                foreach (var kvp in dictionaryValue)
                {
                    elementValueObject.Value[kvp.Key] = kvp.Value;
                }

                return elementValueObject;
            }
            else if (value is string valueString && DateTime.TryParse(valueString, out DateTime valueDateTime))
            {
                return new TaskElementValueString
                {
                    Value = valueDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Valid = valid
                };
            }
            else
            {
                return new TaskElementValueString
                {
                    Value = value.ToString(),
                    Valid = valid
                };
            }
        }

        #endregion
    }
}

