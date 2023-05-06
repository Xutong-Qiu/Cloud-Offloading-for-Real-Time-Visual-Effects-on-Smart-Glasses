#if defined(VARIANT_HAS_VSM)
layout(location = 0) out vec4 fragColor;
#elif defined(VARIANT_HAS_PICKING)
layout(location = 0) out highp uint2 outPicking;
#else
#endif
highp vec2 computeDepthMomentsVSM(const highp float depth);
void main() {
filament_lodBias = frameUniforms.lodBias;
initObjectUniforms(object_uniforms);
#if defined(BLEND_MODE_MASKED) || ((defined(BLEND_MODE_TRANSPARENT) || defined(BLEND_MODE_FADE)) && defined(MATERIAL_HAS_TRANSPARENT_SHADOW))
MaterialInputs inputs;
initMaterial(inputs);
material(inputs);
float alpha = inputs.baseColor.a;
#if defined(BLEND_MODE_MASKED)
if (alpha < getMaskThreshold()) {
discard;
}
#endif
#if defined(MATERIAL_HAS_TRANSPARENT_SHADOW)
float noise = interleavedGradientNoise(gl_FragCoord.xy);
if (noise >= alpha) {
discard;
}
#endif
#endif
#if defined(VARIANT_HAS_VSM)
highp float depth = vertex_worldPosition.w;
depth = exp(frameUniforms.vsmExponent * depth);
fragColor.xy = computeDepthMomentsVSM(depth);
fragColor.zw = computeDepthMomentsVSM(-1.0 / depth);
#elif defined(VARIANT_HAS_PICKING)
outPicking.x = object_uniforms.objectId;
outPicking.y = floatBitsToUint(vertex_position.z / vertex_position.w);
#else
#endif
}
highp vec2 computeDepthMomentsVSM(const highp float depth) {
highp vec2 moments;
moments.x = depth;
moments.y = depth * depth;
return moments;
}
