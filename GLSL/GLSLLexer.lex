%option unicode, codepage:raw

%{
        // User code is all now in ScanHelper.cs
%}

%namespace Shane
%option verbose, summary, noparser, nofiles, unicode

%{
    public int nextToken() { return yylex(); }
    public int getPos() { return yypos; }
    public int getLength() { return yyleng; }
%}

//=============================================================
//=============================================================

number ([0-9])+
chars [A-Za-z]
cstring [A-Za-z_]
blank " "
delim [ \t\n]
word {chars}+
singleLineComment "//"[^\n]*
multiLineComment "/*"[^*]*\*(\*|([^*/]([^*])*\*))*\/

comment {multiLineComment}|{singleLineComment}
class float|double|int|uint|bool|vec2|vec3|vec4|dvec2|dvec3|dvec4|bvec2|bvec3|bvec4|ivec2|ivec3|ivec4|uvec2|uvec3|uvec4|mat2|mat3|mat4|mat2x2|mat2x3|mat2x4|mat3x2|mat3x3|mat3x4|mat4x2|mat4x3|mat4x4|dmat2|dmat3|dmat4|dmat2x2|dmat2x3|dmat2x4|dmat3x2|dmat3x3|dmat3x4|dmat4x2|dmat4x3|dmat4x4|atomic_uint|sampler1D|sampler2D|sampler3D|samplerCube|sampler1DShadow|sampler2DShadow|samplerCubeShadow|sampler1DArray|sampler2DArray|sampler1DArrayShadow|sampler2DArrayShadow|samplerCubeArray|samplerCubeArrayShadow|isampler1D|isampler2D|isampler3D|isamplerCube|isampler1DArray|isampler2DArray|isamplerCubeArray|usampler1D|usampler2D|usampler3D|usamplerCube|usampler1DArray|usampler2DArray|usamplerCubeArray|sampler2DRect|sampler2DRectShadow|isampler2DRect|usampler2DRect|samplerBuffer|isamplerBuffer|usamplerBuffer|sampler2DMS|isampler2DMS|usampler2DMS|sampler2DMSArray|isampler2DMSArray|usampler2DMSArray|image1D|iimage1D|uimage1D|image2D|iimage2D|uimage2D|image3D|iimage3D|uimage3D|image2DRect|iimage2DRect|uimage2DRect|imageCube|iimageCube|uimageCube|imageBuffer|iimageBuffer|uimageBuffer|image1DArray|iimage1DArray|uimage1DArray|image2DArray|iimage2DArray|uimage2DArray|imageCubeArray|iimageCubeArray|uimageCubeArray|image2DMS|iimage2DMS|uimage2DMS|image2DMSArray|iimage2DMSArray|uimage2DMSArray
keyword version|core|true|false|void|struct|if|else|switch|case|default|while|do|for|continue|break|return|discard
qualifier invariant|smooth|flat|noperspective|layout|precise|precision|const|inout|in|out|centroid|patch|sample|uniform|buffer|shared|coherent|volatile|restrict|readonly|writeonly|subroutine|high_precision|medium_precision|low_precision
systemVariable gl_VertexID|gl_InstanceID|gl_PerVertex|gl_Position|gl_PointSize|gl_ClipDistance|gl_in|gl_PatchVerticesIn|gl_PrimitiveID|gl_InvocationID|gl_out|gl_TessLevelOuter|gl_TessLevelInner|gl_PrimitiveIDIn|gl_Layer|gl_ViewportIndex|gl_FragCoord|gl_FrontFacing|gl_PointCoord|gl_SampleID|gl_SamplePosition|gl_SampleMaskIn|gl_Layer|gl_FragDepth|gl_SampleMask|gl_NumWorkGroups|gl_WorkGroupSize|gl_WorkGroupID|gl_LocalInvocationID|gl_GlobalInvocationID|gl_LocalInvocationIndex|gl_DepthRange|gl_NumSamples|gl_MaxComputeWorkGroupCount|gl_MaxComputeWorkGroupSize|gl_MaxComputeUniformComponents|gl_MaxComputeTextureImageUnits|gl_MaxComputeImageUniforms|gl_MaxComputeAtomicCounters|gl_MaxComputeAtomicCounterBuffers|gl_MaxVertexAttribs|gl_MaxVertexUniformComponents|gl_MaxVaryingComponents|gl_MaxVertexOutputComponents|gl_MaxGeometryInputComponents|gl_MaxGeometryOutputComponents|gl_MaxFragmentInputComponents|gl_MaxVertexTextureImageUnits|gl_MaxCombinedTextureImageUnits|gl_MaxTextureImageUnits|gl_MaxImageUnits|gl_MaxCombinedImageUnitsAndFragmentOutputs|gl_MaxImageSamples|gl_MaxVertexImageUniforms|gl_MaxTessControlImageUniforms|gl_MaxTessEvaluationImageUniforms|gl_MaxGeometryImageUniforms|gl_MaxFragmentImageUniforms|gl_MaxCombinedImageUniforms|gl_MaxFragmentUniformComponents|gl_MaxDrawBuffers|gl_MaxClipDistances|gl_MaxGeometryTextureImageUnits|gl_MaxGeometryOutputVertices|gl_MaxGeometryTotalOutputComponents|gl_MaxGeometryUniformComponents|gl_MaxGeometryVaryingComponents|gl_MaxTessControlInputComponents|gl_MaxTessControlOutputComponents|gl_MaxTessControlTextureImageUnits|gl_MaxTessControlUniformComponents|gl_MaxTessControlTotalOutputComponents|gl_MaxTessEvaluationInputComponents|gl_MaxTessEvaluationOutputComponents|gl_MaxTessEvaluationTextureImageUnits|gl_MaxTessEvaluationUniformComponents|gl_MaxTessPatchComponents|gl_MaxPatchVertices|gl_MaxTessGenLevel|gl_MaxViewports|gl_MaxVertexUniformVectors|gl_MaxFragmentUniformVectors|gl_MaxVaryingVectors|gl_MaxVertexAtomicCounters|gl_MaxTessControlAtomicCounters|gl_MaxTessEvaluationAtomicCounters|gl_MaxGeometryAtomicCounters|gl_MaxFragmentAtomicCounters|gl_MaxCombinedAtomicCounters|gl_MaxAtomicCounterBindings|gl_MaxVertexAtomicCounterBuffers|gl_MaxTessControlAtomicCounterBuffers|gl_MaxTessEvaluationAtomicCounterBuffers|gl_MaxGeometryAtomicCounterBuffers|gl_MaxFragmentAtomicCounterBuffers|gl_MaxCombinedAtomicCounterBuffers|gl_MaxAtomicCounterBufferSize|gl_MinProgramTexelOffset|gl_MaxProgramTexelOffset
systemFunction main|radians|degrees|sin|cos|tan|asin|acos|atan|sinh|cosh|tanh|asinh|acosh|atanh|pow|exp|log|exp2|log2|sqrt|inversesqrt|abs|sign|floor|trunc|round|roundEven|ceil|fract|mod|modf|min|max|clamp|mix|step|smoothstep|isnan|isinf|floatBitsToInt|floatBitsToUint|intBitsToFloat|uintBitsToFloat|fma|frexp|ldexp|packUnorm2x16|packSnorm2x16|packUnorm4x8|packSnorm4x8|unpackUnorm2x16|unpackSnorm2x16|unpackUnorm4x8|unpackSnorm4x8|packDouble2x32|unpackDouble2x32|packHalf2x16|unpackHalf2x16|length|distance|dot|cross|normalize|faceforward|reflect|refract|matrixCompMult|outerProduct|transpose|determinant|inverse|lessThan|lessThanEqual|greaterThan|greaterThanEqual|equal|notEqual|any|all|not|uaddCarry|usubBorrow|umulExtended|imulExtended|bitfieldExtract|bitfieldInsert|bitfieldReverse|bitCount|findLSB|findMSB|textureSize|textureQueryLod|textureQueryLevels|texture|textureProj|textureLod|textureOffset|texelFetch|texelFetchOffset|textureProjOffset|textureLodOffset|textureProjLod|textureProjLodOffset|textureGrad|textureGradOffset|textureProjGrad|textureProjGradOffset|textureGather|textureGatherOffset|textureGatherOffsets|atomicCounterIncrement|atomicCounterDecrement|atomicCounter|atomicAdd|atomicMin|atomicMax|atomicAnd|atomicOr|atomicXor|atomicExchange|atomicCompSwap|imageSize|imageLoad|imageStore|imageAtomicAdd|imageAtomicMin|imageAtomicMax|imageAtomicAnd|imageAtomicOr|imageAtomicXor|imageAtomicExchange|imageAtomicCompSwap|dFdx|dFdy|fwidth|interpolateAtCentroid|interpolateAtSample|interpolateAtOffset|noise1|noise2|noise3|noise4|EmitStreamVertex|EndStreamPrimitive|EmitVertex|EndPrimitive|barrier|memoryBarrier|memoryBarrierAtomicCounter|memoryBarrierBuffer|memoryBarrierShared|memoryBarrierImage|groupMemoryBarrier
identifier {cstring}+{number}*[{cstring}@]*{number}*
constNumber {number}+

%%

{keyword}           return (int)GLSLTokenType.Keyword;
{class}             return (int)GLSLTokenType.Class;
{qualifier}         return (int)GLSLTokenType.Qualifier;
{systemVariable}    return (int)GLSLTokenType.SystemVariable;
{systemFunction}    return (int)GLSLTokenType.SystemFunction;
{identifier}        return (int)GLSLTokenType.Identifier;
{comment}           return (int)GLSLTokenType.Comment;
{constNumber}       return (int)GLSLTokenType.Number;

%%