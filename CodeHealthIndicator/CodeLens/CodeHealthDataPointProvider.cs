using System.ComponentModel.Composition;

using Microsoft.VisualStudio.CodeSense.Roslyn;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;

namespace CodeHealthIndicator.CodeLens
{
    [Name("codeHealth")]
    [Microsoft.VisualStudio.Utilities.LocalizedName(typeof(Resources), "LocalizedName")]
    [Export(typeof(ICodeLensDataPointProvider))]
    public class CodeHealthDataPointProvider : ICodeLensDataPointProvider
    {
        public bool CanCreateDataPoint(ICodeLensDescriptor descriptor)
        {
            return (descriptor is ICodeElementDescriptor codeElementDescriptor) && (codeElementDescriptor.Kind == SyntaxNodeKind.Method);
        }

        public ICodeLensDataPoint CreateDataPoint(ICodeLensDescriptor descriptor)
        {
            return new CodeHealthDataPoint((ICodeElementDescriptor)descriptor);
        }
    }
}
