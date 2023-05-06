void addEmissive(const MaterialInputs material, inout vec4 color) {
#if defined(MATERIAL_HAS_EMISSIVE)
highp vec4 emissive = material.emissive;
highp float attenuation = mix(1.0, getExposure(), emissive.w);
color.rgb += emissive.rgb * (attenuation * color.a);
#endif
}
#if defined(BLEND_MODE_MASKED)
float computeMaskedAlpha(float a) {
return (a - getMaskThreshold()) / max(fwidth(a), 1e-3) + 0.5;
}
#endif
vec4 evaluateMaterial(const MaterialInputs material) {
vec4 color = material.baseColor;
#if defined(BLEND_MODE_MASKED)
color.a = computeMaskedAlpha(color.a);
if (color.a <= 0.0) {
discard;
}
if (frameUniforms.needsAlphaChannel == 1.0) {
color.a = 1.0;
}
#endif
#if defined(VARIANT_HAS_DIRECTIONAL_LIGHTING)
#if defined(VARIANT_HAS_SHADOWING)
float visibility = 1.0;
uint cascade = getShadowCascade();
bool cascadeHasVisibleShadows = bool(frameUniforms.cascades & ((1u << cascade) << 8u));
bool hasDirectionalShadows = bool(frameUniforms.directionalShadows & 1u);
if (hasDirectionalShadows && cascadeHasVisibleShadows) {
highp vec4 shadowPosition = getShadowPosition(cascade);
visibility = shadow(true, light_shadowMap, cascade, shadowPosition, 0.0f);
}
if ((frameUniforms.directionalShadows & 0x2u) != 0u && visibility > 0.0) {
if ((object_uniforms.flagsChannels & FILAMENT_OBJECT_CONTACT_SHADOWS_BIT) != 0u) {
visibility *= (1.0 - screenSpaceContactShadow(frameUniforms.lightDirection));
}
}
color *= 1.0 - visibility;
#else
color = vec4(0.0);
#endif
#elif defined(MATERIAL_HAS_SHADOW_MULTIPLIER)
color = vec4(0.0);
#endif
addEmissive(material, color);
return color;
}
