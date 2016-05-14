using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Shane;

namespace GLSL43Highlight
{

    #region Provider definition
    [Export(typeof(ITaggerProvider))]
    [ContentType("glsl")]
    [TagType(typeof(ClassificationTag))]
    internal sealed class GLSLClassifierProvider : ITaggerProvider
    {

        [Export]
        [Name("glsl")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition GLSLContentType = null;

        /// <summary>
        /// vertex shader
        /// </summary>
        [Export]
        [FileExtension(".vert")]
        [ContentType("glsl")]
        internal static FileExtensionToContentTypeDefinition GLSLVshType = null;

        /// <summary>
        /// fragment shader
        /// </summary>
        [Export]
        [FileExtension(".frag")]
        [ContentType("glsl")]
        internal static FileExtensionToContentTypeDefinition GLSLFshType = null;

        /// <summary>
        /// geometry shader
        /// </summary>
        [Export]
        [FileExtension(".geom")]
        [ContentType("glsl")]
        internal static FileExtensionToContentTypeDefinition GLSLGshType = null;

        /// <summary>
        /// tessellation control shader
        /// </summary>
        [Export]
        [FileExtension(".tesc")]
        [ContentType("glsl")]
        internal static FileExtensionToContentTypeDefinition GLSLCshType = null;

        /// <summary>
        /// tessellation evaluation shader
        /// </summary>
        [Export]
        [FileExtension(".tese")]
        [ContentType("glsl")]
        internal static FileExtensionToContentTypeDefinition GLSLEshType = null;

        /// <summary>
        /// compute shader
        /// </summary>
        [Export]
        [FileExtension(".comp")]
        [ContentType("glsl")]
        internal static FileExtensionToContentTypeDefinition GLSLPshType = null;

        [Import]
        internal IClassificationTypeRegistryService classificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new GLSLClassifier(buffer, classificationTypeRegistry) as ITagger<T>;
        }
    }
    #endregion //provider def

    #region Classifier
    internal sealed class GLSLClassifier : ITagger<ClassificationTag>
    {
        internal GLSLClassifier(ITextBuffer buffer, IClassificationTypeRegistryService typeService)
        {
            textBuffer = buffer;
            typeDic = new Dictionary<GLSLTokenType, IClassificationType>();
            typeDic[GLSLTokenType.Text] = typeService.GetClassificationType("GLSLText");
            typeDic[GLSLTokenType.Identifier] = typeService.GetClassificationType("GLSLIdentifier");
            typeDic[GLSLTokenType.Keyword] = typeService.GetClassificationType("GLSLKeyword");
            typeDic[GLSLTokenType.Class] = typeService.GetClassificationType("GLSLClass");
            typeDic[GLSLTokenType.Comment] = typeService.GetClassificationType("GLSLComment");
            typeDic[GLSLTokenType.Qualifier] = typeService.GetClassificationType("GLSLQualifier");
            typeDic[GLSLTokenType.SystemVariable] = typeService.GetClassificationType("GLSLVariable");
            typeDic[GLSLTokenType.SystemFunction] = typeService.GetClassificationType("GLSLFunction");
            typeDic[GLSLTokenType.Number] = typeService.GetClassificationType("GLSLNumber");
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            IClassificationType classification = typeDic[GLSLTokenType.Text];
            string text = spans[0].Snapshot.GetText();
            yield return new TagSpan<ClassificationTag>(
                new SnapshotSpan(spans[0].Snapshot, new Span(0, text.Length)),
                new ClassificationTag(classification));
            scanner.SetSource(text, 0);
            int tok;
            do
            {
                tok = scanner.nextToken();
                int pos = scanner.getPos();
                int len = scanner.getLength();
                int total = text.Length;
                if (pos < 0 || len < 0 || pos > total)
                {
                    continue;
                }
                if (pos + len > total)
                {
                    len = total - pos;
                }
                if (typeDic.TryGetValue((GLSLTokenType)tok, out classification))
                {
                    yield return new TagSpan<ClassificationTag>(
                        new SnapshotSpan(spans[0].Snapshot, new Span(pos, len)),
                        new ClassificationTag(classification));
                }
            } while (tok > (int)Tokens.EOF);
        }

        ITextBuffer textBuffer;
        IDictionary<GLSLTokenType, IClassificationType> typeDic;
        Scanner scanner = new Scanner();
    }
    #endregion //Classifier


    public enum GLSLTokenType
    {
        Text = 1,
        Keyword,
        Comment,
        Identifier,
        Class,
        Qualifier,
        SystemVariable,
        SystemFunction,
        Number,
    }
}
