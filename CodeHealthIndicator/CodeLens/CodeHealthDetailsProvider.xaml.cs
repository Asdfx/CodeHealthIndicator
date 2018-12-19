using Microsoft.VisualStudio.CodeSense.Editor;

namespace CodeHealthIndicator.CodeLens
{
    /// <summary>
    /// Interaction logic for CodeHealthDataPointView.xaml
    /// </summary>
    [DetailsTemplateProvider(typeof(CodeHealthDataPointViewModel))]
    public partial class CodeHealthDetailsProvider
    {
        public CodeHealthDetailsProvider()
        {
            InitializeComponent();
        }
    }
}
