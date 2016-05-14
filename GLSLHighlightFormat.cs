using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace GLSL43Highlight
{
    #region Format definition
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLText")]
    [Name("GLSLText")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLTextFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLTextFormatDefinition()
        {
            this.DisplayName = "GLSL文本";
            this.ForegroundColor = Colors.Black;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLIdentifier")]
    [Name("GLSLIdentifier")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLIdentifierFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLIdentifierFormatDefinition()
        {
            this.DisplayName = "GLSL标识符";
            this.ForegroundColor = Colors.Gray;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLComment")]
    [Name("GLSLComment")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLCommentFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLCommentFormatDefinition()
        {
            this.DisplayName = "GLSL注释";
            this.ForegroundColor = Colors.Green;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLKeyword")]
    [Name("GLSLKeyword")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLKeywordFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLKeywordFormatDefinition()
        {
            this.DisplayName = "GLSL关键字";
            this.ForegroundColor = Colors.Blue;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLClass")]
    [Name("GLSLClass")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLClassFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLClassFormatDefinition()
        {
            this.DisplayName = "GLSL类型";
            this.ForegroundColor = Colors.LightSeaGreen;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLQualifier")]
    [Name("GLSLQualifier")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLQualifierFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLQualifierFormatDefinition()
        {
            this.DisplayName = "GLSL限定符";
            this.ForegroundColor = Colors.DarkRed;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLVariable")]
    [Name("GLSLVariable")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLVariableFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLVariableFormatDefinition()
        {
            this.DisplayName = "GLSL系统变量";
            this.ForegroundColor = Colors.Orange;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLFunction")]
    [Name("GLSLFunction")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLFunctionFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLFunctionFormatDefinition()
        {
            this.DisplayName = "GLSL系统函数";
            this.ForegroundColor = Colors.Red;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "GLSLNumber")]
    [Name("GLSLNumber")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class GLSLNumberFormatDefinition : ClassificationFormatDefinition
    {
        public GLSLNumberFormatDefinition()
        {
            this.DisplayName = "GLSL常量数值";
            this.ForegroundColor = Colors.Crimson;
        }
    }
    #endregion //Format definition
}
