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

            builder.AddCustomAttributes(typeof(KuFlowScope), categoryAttribute);
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


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
