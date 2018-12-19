using System;
using System.Threading;
using System.Threading.Tasks;

using CodeHealthIndicator.CodeHealth;
using CodeHealthIndicator.Models;
using Microsoft.VisualStudio.CodeSense;
using Microsoft.VisualStudio.CodeSense.Roslyn;

namespace CodeHealthIndicator.CodeLens
{
    public class CodeHealthDataPoint : DataPoint<CodeHealthModel>
    {
        private readonly ICodeElementDescriptor descriptor;
        private readonly SyntaxAnalyzer syntaxAnalyzer = new SyntaxAnalyzer();

        public CodeHealthDataPoint(ICodeElementDescriptor descriptor)
        {
            descriptor.SyntaxNodeContentsChanged += OnMethodBodyChanged;
            this.descriptor = descriptor;
        }

        public override Task<CodeHealthModel> GetDataAsync()
        {
            return Run(GetModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                descriptor.SyntaxNodeContentsChanged -= OnMethodBodyChanged;
            }

            base.Dispose(disposing);
        }

        private void OnMethodBodyChanged(object sender, EventArgs eventArgs)
        {
            Invalidate();
        }

        private Task<CodeHealthModel> GetModel(CancellationToken cancellationToken)
        {
            return Task.FromResult(syntaxAnalyzer.AnalyzeMethod(descriptor.SyntaxNode));
        }
    }
}