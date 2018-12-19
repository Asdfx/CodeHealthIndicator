using Microsoft.VisualStudio.CodeSense.Editor;
using Microsoft.VisualStudio.Language.Intellisense;

namespace CodeHealthIndicator.CodeLens
{
    [DataPointViewModelProvider(typeof(CodeHealthDataPoint))]
    public class CodeHealthDataPointViewModelProvider : GlyphDataPointViewModelProvider<CodeHealthDataPointViewModel>
    {
        protected override CodeHealthDataPointViewModel GetViewModel(ICodeLensDataPoint dataPoint)
        {
            if (dataPoint is CodeHealthDataPoint codeHealthDataPoint)
            {
                return new CodeHealthDataPointViewModel(codeHealthDataPoint);
            }

            return null;
        }
    }
}
