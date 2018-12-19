using System;
using System.Collections.Generic;
using System.Linq;

using CodeHealthIndicator.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeHealthIndicator.CodeHealth
{
    internal class SyntaxAnalyzer
    {
        private static readonly List<SyntaxKind> ConditionNodeKinds = new List<SyntaxKind>
        {
            SyntaxKind.IfStatement,
            SyntaxKind.WhileStatement,
            SyntaxKind.ForStatement,
            SyntaxKind.ForEachStatement,
            SyntaxKind.GotoStatement,
            SyntaxKind.CatchClause,
            SyntaxKind.CatchFilterClause,
            SyntaxKind.CaseSwitchLabel,
            SyntaxKind.CasePatternSwitchLabel,
            SyntaxKind.LogicalAndExpression,
            SyntaxKind.LogicalOrExpression,
            SyntaxKind.ConditionalExpression,
            SyntaxKind.CoalesceExpression
        };

        private static readonly List<SyntaxKind> NotOperatorOrOperationKinds = new List<SyntaxKind>
        {
            SyntaxKind.CloseParenToken,
            SyntaxKind.CloseBraceToken,
            SyntaxKind.CloseBracketToken,
            SyntaxKind.OmittedArraySizeExpressionToken
        };

        public CodeHealthModel AnalyzeMethod(SyntaxNode node)
        {
            var nodesAndTokens = node.DescendantNodesAndTokens();
            var descendantNodes = new List<SyntaxNode>();
            var descendantTokens = new List<SyntaxToken>();
            foreach (var nodeOrToken in nodesAndTokens)
            {
                if (nodeOrToken.IsNode)
                {
                    descendantNodes.Add(nodeOrToken.AsNode());
                }
                else if (nodeOrToken.IsToken)
                {
                    descendantTokens.Add(nodeOrToken.AsToken());
                }
            }

            var linesOfCode = GetLinesOfCode(descendantNodes);
            var cyclomaticComplexity = GetCyclomaticComplexity(descendantNodes);
            var halsteadVolume = GetHalsteadVolume(descendantTokens);
            var maintainabilityIndex = GetMaintainabilityIndex(linesOfCode, cyclomaticComplexity, halsteadVolume);

            return new CodeHealthModel
            {
                CyclomaticComplexity = cyclomaticComplexity,
                HalsteadVolume = halsteadVolume,
                LinesOfCode = linesOfCode,
                Maintainability = maintainabilityIndex
            };
        }

        private int GetLinesOfCode(IEnumerable<SyntaxNode> descendantNodes)
        {
            return descendantNodes.Where(IsCodeLine).Count();
        }

        private bool IsCodeLine(SyntaxNode node)
        {
            return node is StatementSyntax && !(node is BlockSyntax || node is EmptyStatementSyntax);
        }

        private int GetCyclomaticComplexity(IEnumerable<SyntaxNode> descendantNodes)
        {
            return descendantNodes.Where(IsConditionNode).Count() + 1;
        }

        private bool IsConditionNode(SyntaxNode node)
        {
            return ConditionNodeKinds.Contains(node.Kind());
        }

        private double GetHalsteadVolume(IEnumerable<SyntaxToken> descendantTokens)
        {
            var operatorsDictionary = GetOperatorsAndOperationsDictionary(descendantTokens);
            var distinctOperatorsAndOperationCount = operatorsDictionary.Count;
            var totalOperatorsAndOperationCount = operatorsDictionary.Sum(x => x.Value);

            return totalOperatorsAndOperationCount * Math.Log(distinctOperatorsAndOperationCount, 2);
        }

        private Dictionary<SyntaxToken, int> GetOperatorsAndOperationsDictionary(IEnumerable<SyntaxToken> descendantTokens)
        {
            var dictionary = new Dictionary<SyntaxToken, int>(new TokenComparer());
            foreach (var token in descendantTokens.Where(IsOperatorOrOperation))
            {
                if (!dictionary.TryGetValue(token, out var tokensCount))
                {
                    tokensCount = 0;
                }

                dictionary[token] = tokensCount + 1;
            }

            return dictionary;
        }

        private bool IsOperatorOrOperation(SyntaxToken token)
        {
            return !NotOperatorOrOperationKinds.Contains(token.Kind());
        }

        private double GetMaintainabilityIndex(int linesOfCode, int cyclomaticComplexity, double halsteadVolume)
        {
            var halsteadVolumeLog = Math.Max(0.0, Math.Log(halsteadVolume));
            var linesOfCodeLog = Math.Max(0.0, Math.Log(linesOfCode));
            return (171 - 5.2 * halsteadVolumeLog - 0.23 * cyclomaticComplexity - 16.2 * linesOfCodeLog) / 171 * 100;
        }

        private class TokenComparer : IEqualityComparer<SyntaxToken>
        {
            public bool Equals(SyntaxToken x, SyntaxToken y)
            {
                if (!x.RawKind.Equals(y.RawKind))
                {
                    return false;
                }

                return string.Equals(x.Text, y.Text, StringComparison.Ordinal);
            }

            public int GetHashCode(SyntaxToken obj)
            {
                return obj.Text.GetHashCode();
            }
        }
    }
}
