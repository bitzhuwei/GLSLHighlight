using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace GLSL43Highlight
{
    internal static class GLSLHighlightClassificationDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLText")]
        internal static ClassificationTypeDefinition GLSLTextType = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLIdentifier")]
        internal static ClassificationTypeDefinition GLSLIdentifierType = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLComment")]
        internal static ClassificationTypeDefinition GLSLCommentType = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLKeyword")]
        internal static ClassificationTypeDefinition GLSLKeywordType = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLClass")]
        internal static ClassificationTypeDefinition GLSLClassType = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLQualifier")]
        internal static ClassificationTypeDefinition GLSLQualifierType = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLVariable")]
        internal static ClassificationTypeDefinition GLSLVariableType = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLFunction")]
        internal static ClassificationTypeDefinition GLSLFunctionType = null;


        [Export(typeof(ClassificationTypeDefinition))]
        [Name("GLSLNumber")]
        internal static ClassificationTypeDefinition GLSLNumberType = null;
    }
}
