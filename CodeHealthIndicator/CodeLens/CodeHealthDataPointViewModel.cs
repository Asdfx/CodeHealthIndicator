using System.ComponentModel;
using System.Windows.Media;

using CodeHealthIndicator.Models;

using Microsoft.Alm.MVVM;
using Microsoft.VisualStudio.CodeSense.Editor;

namespace CodeHealthIndicator.CodeLens
{
    public class CodeHealthDataPointViewModel : GlyphDataPointViewModel
    {
        private readonly CodeHealthDataPoint dataPoint;

        public CodeHealthDataPointViewModel(CodeHealthDataPoint dataPoint) : base(dataPoint)
        {
            this.dataPoint = dataPoint;
            HasDetails = true;

            PropertyChanged += OnPropertyChanged;
        }

        [ValueDependsOnProperty(nameof(Data))]
        public override ImageSource GlyphSource
        {
            get
            {
                if (Maintainability >= 60)
                {
                    return Statics.GoodHealthGlyph;
                }

                if (Maintainability >= 40)
                {
                    return Statics.ModerateHealthGlyph;
                }

                return Statics.BadHealthGlyph;
            }
        }

        [ValueDependsOnProperty(nameof(Data))]
        public int Maintainability { get; private set; }

        [ValueDependsOnProperty(nameof(Data))]
        public int LinesOfCode { get; private set; }

        [ValueDependsOnProperty(nameof(Data))]
        public int HalsteadVolume { get; private set; }

        [ValueDependsOnProperty(nameof(Data))]
        public int CyclomaticComplexity { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PropertyChanged -= OnPropertyChanged;
            }

            base.Dispose(disposing);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Data))
            {
                if (Data is CodeHealthModel codeHealth)
                {
                    OnDataChanged(codeHealth);
                }
            }
        }

        private void OnDataChanged(CodeHealthModel codeHealth)
        {
            Descriptor = GetDescriptor(codeHealth);

            Maintainability = (int)codeHealth.Maintainability;
            CyclomaticComplexity = codeHealth.CyclomaticComplexity;
            HalsteadVolume = (int)codeHealth.HalsteadVolume;
            LinesOfCode = codeHealth.LinesOfCode;
        }

        private string GetDescriptor(CodeHealthModel codeHealth)
        {
            return $"maintainability: {(int)codeHealth.Maintainability}";
        }
    }
}
