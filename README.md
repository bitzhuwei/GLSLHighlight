# ShaderEditor
GLSL shader syntax highlight plug-in(.vsix) 为Visual Studio制作一个GLSL Shader的语法高亮.vsix插件。
<p>本文转自（<a href="http://www.cnblogs.com/aanbpsd/p/3920067.html" target="_blank">http://www.cnblogs.com/aanbpsd/p/3920067.html</a>）</p>
<p>原作者<span style="color: #ff0000;">故意写错</span>了一点东西，这就让那些一点脑筋也不想动的小伙伴得不到想要的结果。我在这里把那些地方纠正过来了。</p>
<p style="text-align: center;"><span style="font-size: 18pt;"><strong>[转]使用Visual Studio SDK制作GLSL词法着色插件</strong></span></p>
<p>我们在Visual Studio上开发OpenGL ES项目时，避免不了写Shader。这时在vs里直接编辑shader就会显得很方便。但是vs默认是不支持GLSL的语法着色的，我们只好自己动手创造。最简单的实现自定义语法着色的方法就是<span style="color: #ff0000;">创建一个VSIX插件包</span>，我们只需要安装Visual Studio SDK，<span style="color: #ff0000;">使用内置的模版</span>就可以构建一个<span style="color: #ff0000;">插件项目</span>。</p>
<h1>1.&nbsp;安装Visual Studio SDK</h1>
<hr />
<p>在<a href="http://www.microsoft.com/en-us/download/details.aspx?id=40758" target="_blank">http://www.microsoft.com/en-us/download/details.aspx?id=40758</a>下载最新的Visual Studio 2013 SDK。</p>
<p>双击安装，一路next即可。</p>
<p>安装完毕后我们可以在新建项目-&gt;模版-&gt;C#中看到&ldquo;扩展性&rdquo;这一条目，这些就是开发插件用的模版了。</p>
<h1>2. 创建插件项目</h1>
<hr />
<p>新建项目，在扩展性标签中，选择<span style="color: #ff0000;">Editor Classifier</span>模版，命名为ShaderEditor，点击确定。</p>
<p>Visual Studio为我们生成了如下几个文件。</p>
<p><img src="http://images.cnitblog.com/i/656631/201408/181940526123987.png" alt="" /></p>
<p>ShaderEditorFormat.cs文件的默认代码如下：&nbsp;</p>
<div class="cnblogs_code">
<div class="cnblogs_code_toolbar"><span class="cnblogs_code_copy"><img src="http://common.cnblogs.com/images/copycode.gif" alt="复制代码" /></span></div>
<pre> 1     [Export(typeof(EditorFormatDefinition))]
 2     [ClassificationType(ClassificationTypeNames = "ShaderEditor")]
 3     [Name("ShaderEditor")]
 4     [UserVisible(true)] //this should be visible to the end user
 5     [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
 6     internal sealed class ShaderEditorFormat : ClassificationFormatDefinition {
 7         /// &lt;summary&gt;
 8         /// Defines the visual format for the "ShaderEditor" classification type
 9         /// &lt;/summary&gt;
10         public ShaderEditorFormat() {
11             this.DisplayName = "ShaderEditor"; //human readable version of the name
12             this.BackgroundColor = Colors.BlueViolet;
13             this.TextDecorations = System.Windows.TextDecorations.Underline;
14         }
15     }</pre>
<div class="cnblogs_code_toolbar"><span class="cnblogs_code_copy"><img src="http://common.cnblogs.com/images/copycode.gif" alt="复制代码" /></span></div>
</div>
<p>&nbsp;</p>
<p>这段代码定义了一个名为"ShaderEditor"的着色类型，编译工程并运行，我们可以在Visual Studio实验实例的工具-&gt;选项-&gt;字体和颜色中找到一个名为"ShaderEditor"的条目。同时我们会发现所有文本文件的颜色都变成了Colors.BlueViolet并带上了下划线。修改this.DisplayName = "ShaderEditor"的内容，可以改变在字体和颜色中显示的名字。下面的格式设置可以任意修改成喜欢的样式，但要注意在这里的格式只是插件首次安装时的默认设置，这些条目和其它着色选项一样，都可以被用户任意更改。</p>
<h1>3. 创建GLSL的着色类型<strong><br /></strong></h1>
<pre></pre>
<hr />
<p>我们已经了解了如何将着色类型添加到Visual Studio，现在修改<span style="color: #ff0000;">ShaderEditorFormat</span>.cs，添加我们的着色类型。</p>
<div class="cnblogs_code">
<pre><span style="color: #008080;">  1</span>     <span style="color: #0000ff;">#region</span> Format definition
<span style="color: #008080;">  2</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;">  3</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLText</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">  4</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLText</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">  5</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;">  6</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;">  7</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLTextFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;">  8</span> <span style="color: #000000;">    {
</span><span style="color: #008080;">  9</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLTextFormatDefinition()
</span><span style="color: #008080;"> 10</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 11</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL文本</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 12</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.Brown;
</span><span style="color: #008080;"> 13</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 14</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 15</span> 
<span style="color: #008080;"> 16</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;"> 17</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLIdentifier</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 18</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLIdentifier</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 19</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 20</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;"> 21</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLIdentifierFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;"> 22</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 23</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLIdentifierFormatDefinition()
</span><span style="color: #008080;"> 24</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 25</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL标识符</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 26</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.Brown;
</span><span style="color: #008080;"> 27</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 28</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 29</span> 
<span style="color: #008080;"> 30</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;"> 31</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLComment</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 32</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLComment</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 33</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 34</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;"> 35</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLCommentFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;"> 36</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 37</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLCommentFormatDefinition()
</span><span style="color: #008080;"> 38</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 39</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL注释</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 40</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.DarkGray;
</span><span style="color: #008080;"> 41</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 42</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 43</span> 
<span style="color: #008080;"> 44</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;"> 45</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLKeyword</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 46</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLKeyword</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 47</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 48</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;"> 49</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLKeywordFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;"> 50</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 51</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLKeywordFormatDefinition()
</span><span style="color: #008080;"> 52</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 53</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL关键字</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 54</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.Blue;
</span><span style="color: #008080;"> 55</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 56</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 57</span> 
<span style="color: #008080;"> 58</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;"> 59</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLClass</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 60</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLClass</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 61</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 62</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;"> 63</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLClassFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;"> 64</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 65</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLClassFormatDefinition()
</span><span style="color: #008080;"> 66</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 67</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL类型</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 68</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.Green;
</span><span style="color: #008080;"> 69</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 70</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 71</span> 
<span style="color: #008080;"> 72</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;"> 73</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLQualifier</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 74</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLQualifier</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 75</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 76</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;"> 77</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLQualifierFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;"> 78</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 79</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLQualifierFormatDefinition()
</span><span style="color: #008080;"> 80</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 81</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL限定符</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 82</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.Pink;
</span><span style="color: #008080;"> 83</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 84</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 85</span> 
<span style="color: #008080;"> 86</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;"> 87</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLVariable</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 88</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLVariable</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 89</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 90</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;"> 91</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLVariableFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;"> 92</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 93</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLVariableFormatDefinition()
</span><span style="color: #008080;"> 94</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 95</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL系统变量</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 96</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.DarkOrange;
</span><span style="color: #008080;"> 97</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 98</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 99</span> 
<span style="color: #008080;">100</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(EditorFormatDefinition))]
</span><span style="color: #008080;">101</span>     [ClassificationType(ClassificationTypeNames = <span style="color: #800000;">"</span><span style="color: #800000;">GLSLFunction</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">102</span>     [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLFunction</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">103</span>     [UserVisible(<span style="color: #0000ff;">true</span><span style="color: #000000;">)]
</span><span style="color: #008080;">104</span>     [Order(Before =<span style="color: #000000;"> Priority.Default)]
</span><span style="color: #008080;">105</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLFunctionFormatDefinition : ClassificationFormatDefinition
</span><span style="color: #008080;">106</span> <span style="color: #000000;">    {
</span><span style="color: #008080;">107</span>         <span style="color: #0000ff;">public</span><span style="color: #000000;"> GLSLFunctionFormatDefinition()
</span><span style="color: #008080;">108</span> <span style="color: #000000;">        {
</span><span style="color: #008080;">109</span>             <span style="color: #0000ff;">this</span>.DisplayName = <span style="color: #800000;">"</span><span style="color: #800000;">GLSL系统函数</span><span style="color: #800000;">"</span><span style="color: #000000;">;
</span><span style="color: #008080;">110</span>             <span style="color: #0000ff;">this</span>.ForegroundColor =<span style="color: #000000;"> Colors.DarkTurquoise;
</span><span style="color: #008080;">111</span> <span style="color: #000000;">        }
</span><span style="color: #008080;">112</span> <span style="color: #000000;">    }
</span><span style="color: #008080;">113</span>     <span style="color: #0000ff;">#endregion</span> <span style="color: #008000;">//</span><span style="color: #008000;">Format definition</span></pre>
</div>
<p>&nbsp;</p>
<h1>4. 导出着色类型</h1>
<hr />
<p>Editor Classifier使用了MEF框架，关于MEF的具体细节，请参考MSDN的相关文档。</p>
<p>我们需要注意的是，在MEF中，光定义了着色类型还不够，我们需要导出一个ClassificationTypeDefinition，才能在系统中生效。</p>
<p>打开<span style="color: #ff0000;">ShaderEditorType</span>.cs，我们看到系统生成的代码如下：</p>
<div class="cnblogs_code">
<pre>1     internal static class ShaderEditorClassificationDefinition {
2         [Export(typeof(ClassificationTypeDefinition))]
3         [Name("ShaderEditor")]
4         internal static ClassificationTypeDefinition ShaderEditorType = null;
5     }</pre>
</div>
<p>这里的Name与之前默认生成的ShaderEditor相同，同理，我们将这里的代码修改成方才定义的类型</p>
<div class="cnblogs_code">
<pre><span style="color: #008080;"> 1</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> ShaderEditorClassificationDefinition
</span><span style="color: #008080;"> 2</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 3</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;"> 4</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLText</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 5</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLTextType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 6</span> 
<span style="color: #008080;"> 7</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;"> 8</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLIdentifier</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 9</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLIdentifierType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">10</span> 
<span style="color: #008080;">11</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;">12</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLComment</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">13</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLCommentType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">14</span> 
<span style="color: #008080;">15</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;">16</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLKeyword</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">17</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLKeywordType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">18</span> 
<span style="color: #008080;">19</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;">20</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLClass</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">21</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLClassType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">22</span> 
<span style="color: #008080;">23</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;">24</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLQualifier</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">25</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLQualifierType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">26</span> 
<span style="color: #008080;">27</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;">28</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLVariable</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">29</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLVariableType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">30</span> 
<span style="color: #008080;">31</span>         [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTypeDefinition))]
</span><span style="color: #008080;">32</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLFunction</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">33</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ClassificationTypeDefinition GLSLFunctionType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">34</span>     }</pre>
</div>
<p>&nbsp;</p>
<h1>5. 关联文件类型</h1>
<hr />
<p>打开<span style="color: #ff0000;">ShaderEditor</span>.cs</p>
<div class="cnblogs_code">
<div class="cnblogs_code_toolbar"><span class="cnblogs_code_copy"><img src="http://common.cnblogs.com/images/copycode.gif" alt="复制代码" /></span></div>
<pre> 1     [Export(typeof(IClassifierProvider))]
 2     [ContentType("text")]
 3     internal class ShaderEditorProvider : IClassifierProvider {
 4         [Import]
 5         internal IClassificationTypeRegistryService ClassificationRegistry = null; // Set via MEF
 6 
 7         public IClassifier GetClassifier(ITextBuffer buffer) {
 8             return buffer.Properties.GetOrCreateSingletonProperty&lt;ShaderEditor&gt;(delegate { return new ShaderEditor(ClassificationRegistry); });
 9         }
10     }</pre>
<div class="cnblogs_code_toolbar"><span class="cnblogs_code_copy"><img src="http://common.cnblogs.com/images/copycode.gif" alt="复制代码" /></span></div>
</div>
<p>代码ContentType("text")创建了一个Provider并将它们对所有text类型的文件生效。</p>
<p>GLSL主要的文件扩展名为.vsh和.fsh，为了只对这两个扩展名生效，我们需要自定义一个ContentType，并创建两个扩展名关联。将上述代码修改为：</p>
<div class="cnblogs_code">
<pre><span style="color: #008080;"> 1</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ITaggerProvider))]
</span><span style="color: #008080;"> 2</span>     [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 3</span>     [TagType(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTag))]
</span><span style="color: #008080;"> 4</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLClassifierProvider : ITaggerProvider {
</span><span style="color: #008080;"> 5</span> 
<span style="color: #008080;"> 6</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;"> 7</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 8</span>         [BaseDefinition(<span style="color: #800000;">"</span><span style="color: #800000;">code</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 9</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ContentTypeDefinition GLSLContentType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">10</span> 
<span style="color: #008080;">11</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;">12</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.vsh</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">13</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">14</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLVshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">15</span> 
<span style="color: #008080;">16</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;">17</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.fsh</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">18</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">19</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLFshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">20</span> 
<span style="color: #008080;">21</span> <span style="color: #000000;">        [Import]
</span><span style="color: #008080;">22</span>         <span style="color: #0000ff;">internal</span> IClassificationTypeRegistryService classificationTypeRegistry = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">23</span> 
<span style="color: #008080;">24</span> <span style="color: #000000;">        [Import]
</span><span style="color: #008080;">25</span>         <span style="color: #0000ff;">internal</span> IBufferTagAggregatorFactoryService aggregatorFactory = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">26</span> 
<span style="color: #008080;">27</span>         <span style="color: #0000ff;">public</span> ITagger&lt;T&gt; CreateTagger&lt;T&gt;(ITextBuffer buffer) <span style="color: #0000ff;">where</span><span style="color: #000000;"> T : ITag {
</span><span style="color: #008080;">28</span>             <span style="color: #0000ff;">return</span> <span style="color: #0000ff;">new</span> GLSLClassifier(buffer, classificationTypeRegistry) <span style="color: #0000ff;">as</span> ITagger&lt;T&gt;<span style="color: #000000;">;
</span><span style="color: #008080;">29</span> <span style="color: #000000;">        }
</span><span style="color: #008080;">30</span>     }</pre>
</div>
<p>（这段代码有问题，请用我改过的）</p>
<div class="cnblogs_code">
<pre><span style="color: #008080;"> 1</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ITaggerProvider))]
</span><span style="color: #008080;"> 2</span>     [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 3</span>     [TagType(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTag))]
</span><span style="color: #008080;"> 4</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLClassifierProvider : ITaggerProvider
</span><span style="color: #008080;"> 5</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 6</span> 
<span style="color: #008080;"> 7</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;"> 8</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 9</span>         [BaseDefinition(<span style="color: #800000;">"</span><span style="color: #800000;">code</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">10</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ContentTypeDefinition GLSLContentType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">11</span> 
<span style="color: #008080;">12</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;">13</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.vert</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">14</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">15</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLVshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">16</span> 
<span style="color: #008080;">17</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;">18</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.frag</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">19</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">20</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLFshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">21</span> 
<span style="color: #008080;">22</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;">23</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.geom</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">24</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">25</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLGshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">26</span> 
<span style="color: #008080;">27</span> <span style="color: #000000;">        [Import]
</span><span style="color: #008080;">28</span>         <span style="color: #0000ff;">internal</span> IClassificationTypeRegistryService classificationTypeRegistry = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">29</span> 
<span style="color: #008080;">30</span> <span style="color: #000000;">        [Import]
</span><span style="color: #008080;">31</span>         <span style="color: #0000ff;">internal</span> IBufferTagAggregatorFactoryService aggregatorFactory = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;">32</span> 
<span style="color: #008080;">33</span>         <span style="color: #0000ff;">public</span> ITagger&lt;T&gt; CreateTagger&lt;T&gt;(ITextBuffer buffer) <span style="color: #0000ff;">where</span><span style="color: #000000;"> T : ITag
</span><span style="color: #008080;">34</span> <span style="color: #000000;">        {
</span><span style="color: #008080;">35</span>             <span style="color: #0000ff;">return</span> <span style="color: #0000ff;">new</span> GLSLClassifier(buffer, classificationTypeRegistry) <span style="color: #0000ff;">as</span> ITagger&lt;T&gt;<span style="color: #000000;">;
</span><span style="color: #008080;">36</span> <span style="color: #000000;">        }
</span><span style="color: #008080;">37</span>     }</pre>
</div>
<p>这样我们就创建了只针对vert、frag和geom文件生效的Editor。也就是顶点Shader、片段Shader和几何Shader。</p>
<p>&nbsp;</p>
<h1>6. 使用gplex进行词法分析</h1>
<hr />
<p>我们需要使用词法分析扫描器来实现具体的着色功能，gplex可以为我们生成C#语言的扫描器，下载地址：</p>
<p><a href="http://gplex.codeplex.com/" target="_blank">http://gplex.codeplex.com/</a></p>
<p>解压后在binaries文件夹下找到gplex.exe，把它拷贝到项目的根目录下。</p>
<p>在项目根目录下新建一个GLSL文件夹，新建GLSLLexer.lex文件。并把它们添加到proj中。</p>
<p>在proj上右键-&gt;属性，在生成事件选项卡中，在预先生成事件命令行中输入</p>
<p>cd $(ProjectDir)GLSL\<br />$(ProjectDir)\gplex GLSLLexer</p>
<p>打开GLSLLexer.lex，写入以下代码：</p>
<div class="cnblogs_code">
<div class="cnblogs_code_toolbar"><span class="cnblogs_code_copy"><img src="http://common.cnblogs.com/images/copycode.gif" alt="复制代码" /></span></div>
<pre> 1 %option unicode, codepage:raw
 2 
 3 %{
 4         // User code is all now in ScanHelper.cs
 5 %}
 6 
 7 %namespace Shane
 8 %option verbose, summary, noparser, nofiles, unicode
 9 
10 %{
11     public int nextToken() { return yylex(); }
12     public int getPos() { return yypos; }
13     public int getLength() { return yyleng; }
14 %}
15 
16 //=============================================================
17 //=============================================================
18 
19 number ([0-9])+
20 chars [A-Za-z]
21 cstring [A-Za-z_]
22 blank " "
23 delim [ \t\n]
24 word {chars}+
25 singleLineComment "//"[^\n]*
26 multiLineComment "/*"[^*]*\*(\*|([^*/]([^*])*\*))*\/
27 
28 comment {multiLineComment}|{singleLineComment}
29 class bool|int|float|bvec|ivec|vec|vec2|vec3|vec4|mat2|mat3|mat4|sampler1D|sampler2D|sampler3D|samplerCube|sampler1DShadow|sampler2DShadow
30 keyword return|if|else|while|do|for|foreach|break|continue|switch|case|default|goto|class|struct|enum|extern|interface|namespace|public|static|this|volatile|using|in|out|true|false
31 qualifier const|attribute|uniform|varying
32 systemVariable gl_BackColor|gl_BackLightModelProduct|gl_BackLightProduct|gl_BackMaterial|gl_BackSecondaryColor|gl_ClipPlane|gl_ClipVertex|gl_Color|gl_DepthRange|gl_DepthRangeParameters|gl_EyePlaneQ|gl_EyePlaneR|gl_EyePlaneS|gl_EyePlaneT|gl_Fog|gl_FogCoord|gl_FogFragCoord|gl_FogParameters|gl_FragColor|gl_FragCoord|gl_FragData|gl_FragDepth|gl_FrontColor|gl_FrontFacing|gl_FrontLightModelProduct|gl_FrontLightProduct|gl_FrontMaterial|gl_FrontSecondaryColor|gl_LightModel|gl_LightModelParameters|gl_LightModelProducts|gl_LightProducts|gl_LightSource|gl_LightSourceParameters|gl_MaterialParameters|gl_MaxClipPlanes|gl_MaxCombinedTextureImageUnits|gl_MaxDrawBuffers|gl_MaxFragmentUniformComponents|gl_MaxLights|gl_MaxTextureCoords|gl_MaxTextureImageUnits|gl_MaxTextureUnits|gl_MaxVaryingFloats|gl_MaxVertexAttribs|gl_MaxVertexTextureImageUnits|gl_MaxVertexUniformComponents|gl_ModelViewMatrix|gl_ModelViewMatrixInverse|gl_ModelViewMatrixInverseTranspose|gl_ModelViewMatrixTranspose|gl_ModelViewProjectionMatrix|gl_ModelViewProjectionMatrixInverse|gl_ModelViewProjectionMatrixInverseTranspose|gl_ModelViewProjectionMatrixTranspose|gl_MultiTexCoord0|gl_MultiTexCoord1|gl_MultiTexCoord10|gl_MultiTexCoord11|gl_MultiTexCoord2|gl_MultiTexCoord3|gl_MultiTexCoord4|gl_MultiTexCoord5|gl_MultiTexCoord6|gl_MultiTexCoord7|gl_MultiTexCoord8|gl_MultiTexCoord9|gl_Normal|gl_NormalMatrix|gl_NormalScale|gl_ObjectPlaneQ|gl_ObjectPlaneR|gl_ObjectPlaneS|gl_ObjectPlaneT|gl_Point|gl_PointParameters|gl_PointSize|gl_Position|gl_ProjectionMatrix|gl_ProjectionMatrixInverse|gl_ProjectionMatrixInverseTranspose|gl_ProjectionMatrixTranspose|gl_SecondaryColor|gl_TexCoord|gl_TextureEnvColor|gl_TextureMatrix|gl_TextureMatrixInverse|gl_TextureMatrixInverseTranspose|gl_TextureMatrixTranspose|gl_Vertex
33 systemFunction radians|degress|sin|cos|tan|asin|acos|atan|pow|exp|log|exp2|log2|sqrt|inversesqrt|abs|sign|floor|ceil|fract|mod|min|max|clamp|mix|step|smoothstep|length|distance|dot|cross|normalize|faceforward|reflect|matrixCompMult|lessThan|lessThanEqual|greaterThan|greaterThanEqual|equal|notEqual|any|all|not|texture2D|texture2DProj|texture2DLod|texture2DProjLod|textureCube|textureCubeLod
34 identifier {cstring}+{number}*[{cstring}@]*{number}*
35 
36 %%
37 
38 {keyword}            return (int)GLSLTokenType.Keyword;
39 {class}                return (int)GLSLTokenType.Class;
40 {qualifier}            return (int)GLSLTokenType.Qualifier;
41 {systemVariable}    return (int)GLSLTokenType.SystemVariable;
42 {systemFunction}    return (int)GLSLTokenType.SystemFunction;
43 {identifier}        return (int)GLSLTokenType.Identifier;
44 {comment}            return (int)GLSLTokenType.Comment;
45 
46 %%</pre>
<div class="cnblogs_code_toolbar"><span class="cnblogs_code_copy"><img src="http://common.cnblogs.com/images/copycode.gif" alt="复制代码" /></span></div>
</div>
<p>保存并关闭，这时生成一下项目，我们会看到在GLSL目录下生成了<span style="color: #ff0000;">GLSLLexer</span>.cs文件，同样把这个文件添加到proj中。</p>
<p><a href="http://files.cnblogs.com/files/bitzhuwei/GLSLLexer.rar" target="_blank">如果你很懒，可以直接用这个（点此下载），这是我已经生成了的GLSLLexer.cs。（源码太大，多次上传失败）</a></p>
<p>&nbsp;</p>
<h1>7. 处理扫描结果&nbsp;</h1>
<hr />
<p>接下来我们要在ShaderEditor.cs中处理我们的扫描结果，并最终对匹配的代码行进行着色。</p>
<p>首先删除默认创建的ShaderEditor类。</p>
<p>添加一个<span style="color: #ff0000;">GLSLToken</span>枚举，这个枚举就是GLSLLexer.cs返回的枚举类型，它用来通知我们当前的语句块是哪个类型。</p>
<p>代码如下：</p>
<div class="cnblogs_code">
<pre><span style="color: #008080;"> 1</span>     <span style="color: #0000ff;">public</span> <span style="color: #0000ff;">enum</span><span style="color: #000000;"> GLSLTokenType
</span><span style="color: #008080;"> 2</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 3</span>         Text = <span style="color: #800080;">1</span><span style="color: #000000;">,
</span><span style="color: #008080;"> 4</span> <span style="color: #000000;">        Keyword,
</span><span style="color: #008080;"> 5</span> <span style="color: #000000;">        Comment,
</span><span style="color: #008080;"> 6</span> <span style="color: #000000;">        Identifier,
</span><span style="color: #008080;"> 7</span> <span style="color: #000000;">        Class,
</span><span style="color: #008080;"> 8</span> <span style="color: #000000;">        Qualifier,
</span><span style="color: #008080;"> 9</span> <span style="color: #000000;">        SystemVariable,
</span><span style="color: #008080;">10</span> <span style="color: #000000;">        SystemFunction
</span><span style="color: #008080;">11</span>     }</pre>
</div>
<p>&nbsp;</p>
<p>创建我们自己的ShaderEditor类，代码如下：</p>
<div class="cnblogs_code" onclick="cnblogs_code_show('25bd3507-fd78-42b8-9eaa-0fbd047d117c')"><img id="code_img_closed_25bd3507-fd78-42b8-9eaa-0fbd047d117c" class="code_img_closed" src="http://images.cnblogs.com/OutliningIndicators/ContractedBlock.gif" alt="" /><img id="code_img_opened_25bd3507-fd78-42b8-9eaa-0fbd047d117c" class="code_img_opened" style="display: none;" onclick="cnblogs_code_hide('25bd3507-fd78-42b8-9eaa-0fbd047d117c',event)" src="http://images.cnblogs.com/OutliningIndicators/ExpandedBlockStart.gif" alt="" />
<div id="cnblogs_code_open_25bd3507-fd78-42b8-9eaa-0fbd047d117c" class="cnblogs_code_hide">
<pre><span style="color: #008080;">  1</span>     <span style="color: #0000ff;">#region</span> Provider definition
<span style="color: #008080;">  2</span>     [Export(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ITaggerProvider))]
</span><span style="color: #008080;">  3</span>     [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;">  4</span>     [TagType(<span style="color: #0000ff;">typeof</span><span style="color: #000000;">(ClassificationTag))]
</span><span style="color: #008080;">  5</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span><span style="color: #000000;"> GLSLClassifierProvider : ITaggerProvider
</span><span style="color: #008080;">  6</span> <span style="color: #000000;">    {
</span><span style="color: #008080;">  7</span> 
<span style="color: #008080;">  8</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;">  9</span>         [Name(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 10</span>         [BaseDefinition(<span style="color: #800000;">"</span><span style="color: #800000;">code</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 11</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> ContentTypeDefinition GLSLContentType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 12</span> 
<span style="color: #008080;"> 13</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;"> 14</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.vert</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 15</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 16</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLVshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 17</span> 
<span style="color: #008080;"> 18</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;"> 19</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.frag</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 20</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 21</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLFshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 22</span> 
<span style="color: #008080;"> 23</span> <span style="color: #000000;">        [Export]
</span><span style="color: #008080;"> 24</span>         [FileExtension(<span style="color: #800000;">"</span><span style="color: #800000;">.geom</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 25</span>         [ContentType(<span style="color: #800000;">"</span><span style="color: #800000;">glsl</span><span style="color: #800000;">"</span><span style="color: #000000;">)]
</span><span style="color: #008080;"> 26</span>         <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">static</span> FileExtensionToContentTypeDefinition GLSLGshType = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 27</span> 
<span style="color: #008080;"> 28</span> <span style="color: #000000;">        [Import]
</span><span style="color: #008080;"> 29</span>         <span style="color: #0000ff;">internal</span> IClassificationTypeRegistryService classificationTypeRegistry = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 30</span> 
<span style="color: #008080;"> 31</span> <span style="color: #000000;">        [Import]
</span><span style="color: #008080;"> 32</span>         <span style="color: #0000ff;">internal</span> IBufferTagAggregatorFactoryService aggregatorFactory = <span style="color: #0000ff;">null</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 33</span> 
<span style="color: #008080;"> 34</span>         <span style="color: #0000ff;">public</span> ITagger&lt;T&gt; CreateTagger&lt;T&gt;(ITextBuffer buffer) <span style="color: #0000ff;">where</span><span style="color: #000000;"> T : ITag
</span><span style="color: #008080;"> 35</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 36</span>             <span style="color: #0000ff;">return</span> <span style="color: #0000ff;">new</span> GLSLClassifier(buffer, classificationTypeRegistry) <span style="color: #0000ff;">as</span> ITagger&lt;T&gt;<span style="color: #000000;">;
</span><span style="color: #008080;"> 37</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 38</span> <span style="color: #000000;">    }
</span><span style="color: #008080;"> 39</span>     <span style="color: #0000ff;">#endregion</span> <span style="color: #008000;">//</span><span style="color: #008000;">provider def</span>
<span style="color: #008080;"> 40</span> 
<span style="color: #008080;"> 41</span>     <span style="color: #0000ff;">#region</span> Classifier
<span style="color: #008080;"> 42</span>     <span style="color: #0000ff;">internal</span> <span style="color: #0000ff;">sealed</span> <span style="color: #0000ff;">class</span> GLSLClassifier : ITagger&lt;ClassificationTag&gt;
<span style="color: #008080;"> 43</span> <span style="color: #000000;">    {
</span><span style="color: #008080;"> 44</span>         <span style="color: #0000ff;">internal</span><span style="color: #000000;"> GLSLClassifier(ITextBuffer buffer, IClassificationTypeRegistryService typeService)
</span><span style="color: #008080;"> 45</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 46</span>             textBuffer =<span style="color: #000000;"> buffer;
</span><span style="color: #008080;"> 47</span>             typeDic = <span style="color: #0000ff;">new</span> Dictionary&lt;GLSLTokenType, IClassificationType&gt;<span style="color: #000000;">();
</span><span style="color: #008080;"> 48</span>             typeDic[GLSLTokenType.Text] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLText</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 49</span>             typeDic[GLSLTokenType.Identifier] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLIdentifier</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 50</span>             typeDic[GLSLTokenType.Keyword] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLKeyword</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 51</span>             typeDic[GLSLTokenType.Class] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLClass</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 52</span>             typeDic[GLSLTokenType.Comment] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLComment</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 53</span>             typeDic[GLSLTokenType.Qualifier] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLQualifier</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 54</span>             typeDic[GLSLTokenType.SystemVariable] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLVariable</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 55</span>             typeDic[GLSLTokenType.SystemFunction] = typeService.GetClassificationType(<span style="color: #800000;">"</span><span style="color: #800000;">GLSLFunction</span><span style="color: #800000;">"</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 56</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 57</span> 
<span style="color: #008080;"> 58</span>         <span style="color: #0000ff;">public</span> <span style="color: #0000ff;">event</span> EventHandler&lt;SnapshotSpanEventArgs&gt;<span style="color: #000000;"> TagsChanged
</span><span style="color: #008080;"> 59</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 60</span> <span style="color: #000000;">            add { }
</span><span style="color: #008080;"> 61</span> <span style="color: #000000;">            remove { }
</span><span style="color: #008080;"> 62</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 63</span> 
<span style="color: #008080;"> 64</span>         <span style="color: #0000ff;">public</span> IEnumerable&lt;ITagSpan&lt;ClassificationTag&gt;&gt;<span style="color: #000000;"> GetTags(NormalizedSnapshotSpanCollection spans)
</span><span style="color: #008080;"> 65</span> <span style="color: #000000;">        {
</span><span style="color: #008080;"> 66</span>             IClassificationType classification =<span style="color: #000000;"> typeDic[GLSLTokenType.Text];
</span><span style="color: #008080;"> 67</span>             <span style="color: #0000ff;">string</span> text = spans[<span style="color: #800080;">0</span><span style="color: #000000;">].Snapshot.GetText();
</span><span style="color: #008080;"> 68</span>             <span style="color: #0000ff;">yield</span> <span style="color: #0000ff;">return</span> <span style="color: #0000ff;">new</span> TagSpan&lt;ClassificationTag&gt;<span style="color: #000000;">(
</span><span style="color: #008080;"> 69</span>                 <span style="color: #0000ff;">new</span> SnapshotSpan(spans[<span style="color: #800080;">0</span>].Snapshot, <span style="color: #0000ff;">new</span> Span(<span style="color: #800080;">0</span><span style="color: #000000;">, text.Length)),
</span><span style="color: #008080;"> 70</span>                 <span style="color: #0000ff;">new</span><span style="color: #000000;"> ClassificationTag(classification));
</span><span style="color: #008080;"> 71</span>             scanner.SetSource(text, <span style="color: #800080;">0</span><span style="color: #000000;">);
</span><span style="color: #008080;"> 72</span>             <span style="color: #0000ff;">int</span><span style="color: #000000;"> tok;
</span><span style="color: #008080;"> 73</span>             <span style="color: #0000ff;">do</span>
<span style="color: #008080;"> 74</span> <span style="color: #000000;">            {
</span><span style="color: #008080;"> 75</span>                 tok =<span style="color: #000000;"> scanner.nextToken();
</span><span style="color: #008080;"> 76</span>                 <span style="color: #0000ff;">int</span> pos =<span style="color: #000000;"> scanner.getPos();
</span><span style="color: #008080;"> 77</span>                 <span style="color: #0000ff;">int</span> len =<span style="color: #000000;"> scanner.getLength();
</span><span style="color: #008080;"> 78</span>                 <span style="color: #0000ff;">int</span> total =<span style="color: #000000;"> text.Length;
</span><span style="color: #008080;"> 79</span>                 <span style="color: #0000ff;">if</span> (pos &lt; <span style="color: #800080;">0</span> || len &lt; <span style="color: #800080;">0</span> || pos &gt;<span style="color: #000000;"> total)
</span><span style="color: #008080;"> 80</span> <span style="color: #000000;">                {
</span><span style="color: #008080;"> 81</span>                     <span style="color: #0000ff;">continue</span><span style="color: #000000;">;
</span><span style="color: #008080;"> 82</span> <span style="color: #000000;">                }
</span><span style="color: #008080;"> 83</span>                 <span style="color: #0000ff;">if</span> (pos + len &gt;<span style="color: #000000;"> total)
</span><span style="color: #008080;"> 84</span> <span style="color: #000000;">                {
</span><span style="color: #008080;"> 85</span>                     len = total -<span style="color: #000000;"> pos;
</span><span style="color: #008080;"> 86</span> <span style="color: #000000;">                }
</span><span style="color: #008080;"> 87</span>                 <span style="color: #0000ff;">if</span> (typeDic.TryGetValue((GLSLTokenType)tok, <span style="color: #0000ff;">out</span><span style="color: #000000;"> classification))
</span><span style="color: #008080;"> 88</span> <span style="color: #000000;">                {
</span><span style="color: #008080;"> 89</span>                     <span style="color: #0000ff;">yield</span> <span style="color: #0000ff;">return</span> <span style="color: #0000ff;">new</span> TagSpan&lt;ClassificationTag&gt;<span style="color: #000000;">(
</span><span style="color: #008080;"> 90</span>                         <span style="color: #0000ff;">new</span> SnapshotSpan(spans[<span style="color: #800080;">0</span>].Snapshot, <span style="color: #0000ff;">new</span><span style="color: #000000;"> Span(pos, len)),
</span><span style="color: #008080;"> 91</span>                         <span style="color: #0000ff;">new</span><span style="color: #000000;"> ClassificationTag(classification));
</span><span style="color: #008080;"> 92</span> <span style="color: #000000;">                }
</span><span style="color: #008080;"> 93</span>             } <span style="color: #0000ff;">while</span> (tok &gt; (<span style="color: #0000ff;">int</span><span style="color: #000000;">)Tokens.EOF);
</span><span style="color: #008080;"> 94</span> <span style="color: #000000;">        }
</span><span style="color: #008080;"> 95</span> 
<span style="color: #008080;"> 96</span> <span style="color: #000000;">        ITextBuffer textBuffer;
</span><span style="color: #008080;"> 97</span>         IDictionary&lt;GLSLTokenType, IClassificationType&gt;<span style="color: #000000;"> typeDic;
</span><span style="color: #008080;"> 98</span>         Scanner scanner = <span style="color: #0000ff;">new</span><span style="color: #000000;"> Scanner();
</span><span style="color: #008080;"> 99</span> <span style="color: #000000;">    }
</span><span style="color: #008080;">100</span>     <span style="color: #0000ff;">#endregion</span> <span style="color: #008000;">//</span><span style="color: #008000;">Classifier</span></pre>
</div>
<span class="cnblogs_code_collapse">GLSLClassifierProvider </span></div>
<p><span style="line-height: 1.5;">TagsChanged事件保证在代码发生改变时实时刷新编辑器。</span></p>
<p>构造方法中，通过typeService.GetClassificationType("GLSLIdentifier")取得我们定义的实例，并把它们和枚举关联起来，</p>
<p>GetClassificationType的参数传入着色类型的Name。</p>
<p>GetTags方法是由系统调用的迭代方法，yield return new TagSpan&lt;ClassificationTag&gt;()返回我们的着色对象，即可实现着色效果。</p>
<p>编译并运行，可以看到vert、fraghe geom已经有了语法着色了。</p>
<p>&nbsp;</p>
<p>如果你实在做不出来，不如<a href="https://github.com/bitzhuwei/GLSLHighlight" target="_blank">去我的Github下载源码</a>，直接编译即可。</p>
<p>&nbsp;</p>
