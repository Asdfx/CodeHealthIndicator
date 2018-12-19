namespace CodeHealthIndicator.Models
{
    /// <summary>
    /// Code health model.
    /// </summary>
    public class CodeHealthModel
    {
        /// <summary>
        /// Gets or sets the lines of code.
        /// </summary>
        /// <value>
        /// The lines of code.
        /// </value>
        public int LinesOfCode { get; set; }

        /// <summary>
        /// Gets or sets the Halstead volume.
        /// </summary>
        /// <value>
        /// The Halstead volume.
        /// </value>
        public double HalsteadVolume { get; set; }

        /// <summary>
        /// Gets or sets the cyclomatic complexity.
        /// </summary>
        /// <value>
        /// The cyclomatic complexity.
        /// </value>
        public int CyclomaticComplexity { get; set; }

        /// <summary>
        /// Gets or sets the maintainability.
        /// </summary>
        /// <value>
        /// The maintainability.
        /// </value>
        public double Maintainability { get; set; }
    }
}
