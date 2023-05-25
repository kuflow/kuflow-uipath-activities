using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using KuFlow.UiPathLibrary.Activities.Design.Designers;
using KuFlow.UiPathLibrary.Activities.Design.Properties;

namespace KuFlow.UiPathLibrary.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");
            var scopeCategoryAttribute = new CategoryAttribute($"{Resources.ScopeCategory}");

            builder.AddCustomAttributes(typeof(KuFlowScope), scopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(KuFlowScope), new DesignerAttribute(typeof(KuFlowScopeDesigner)));
            builder.AddCustomAttributes(typeof(KuFlowScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(AppendALogToTheTask), categoryAttribute);
            builder.AddCustomAttributes(typeof(AppendALogToTheTask), new DesignerAttribute(typeof(AppendALogToTheTaskDesigner)));
            builder.AddCustomAttributes(typeof(AppendALogToTheTask), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(RetrieveTask), categoryAttribute);
            builder.AddCustomAttributes(typeof(RetrieveTask), new DesignerAttribute(typeof(RetrieveTaskDesigner)));
            builder.AddCustomAttributes(typeof(RetrieveTask), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(RetrieveProcess), categoryAttribute);
            builder.AddCustomAttributes(typeof(RetrieveProcess), new DesignerAttribute(typeof(RetrieveProcessDesigner)));
            builder.AddCustomAttributes(typeof(RetrieveProcess), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ClaimTask), categoryAttribute);
            builder.AddCustomAttributes(typeof(ClaimTask), new DesignerAttribute(typeof(ClaimTaskDesigner)));
            builder.AddCustomAttributes(typeof(ClaimTask), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SaveElement), categoryAttribute);
            builder.AddCustomAttributes(typeof(SaveElement), new DesignerAttribute(typeof(SaveElementDesigner)));
            builder.AddCustomAttributes(typeof(SaveElement), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SaveElementDocument), categoryAttribute);
            builder.AddCustomAttributes(typeof(SaveElementDocument), new DesignerAttribute(typeof(SaveElementDocumentDesigner)));
            builder.AddCustomAttributes(typeof(SaveElementDocument), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DeleteElement), categoryAttribute);
            builder.AddCustomAttributes(typeof(DeleteElement), new DesignerAttribute(typeof(DeleteElementDesigner)));
            builder.AddCustomAttributes(typeof(DeleteElement), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DeleteElementDocument), categoryAttribute);
            builder.AddCustomAttributes(typeof(DeleteElementDocument), new DesignerAttribute(typeof(DeleteElementDocumentDesigner)));
            builder.AddCustomAttributes(typeof(DeleteElementDocument), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
