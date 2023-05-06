
float getObjectUserData() {
return object_uniforms.userData;
}
#if defined(HAS_ATTRIBUTE_COLOR)
vec4 getColor() {
return vertex_color;
}
#endif
#if defined(HAS_ATTRIBUTE_UV0)
highp vec2 getUV0() {
return vertex_uv01.xy;
}
#endif
#if defined(HAS_ATTRIBUTE_UV1)
highp vec2 getUV1() {
return vertex_uv01.zw;
}
#endif
#if defined(BLEND_MODE_MASKED)
float getMaskThreshold() {
return materialParams._maskThreshold;
}
#endif
highp mat3 getWorldTangentFrame() {
return shading_tangentToWorld;
}
highp vec3 getWorldPosition() {
return shading_position;
}
highp vec3 getUserWorldPosition() {
return mulMat4x4Float3(getUserWorldFromWorldMatrix(), getWorldPosition()).xyz;
}
vec3 getWorldViewVector() {
return shading_view;
}
bool isPerspectiveProjection() {
return frameUniforms.clipFromViewMatrix[2].w != 0.0;
}
vec3 getWorldNormalVector() {
return shading_normal;
}
vec3 getWorldGeometricNormalVector() {
return shading_geometricNormal;
}
vec3 getWorldReflectedVector() {
return shading_reflected;
}
float getNdotV() {
return shading_NoV;
}
highp vec3 getNormalizedPhysicalViewportCoord() {
return vec3(shading_normalizedViewportCoord, gl_FragCoord.z);
}
highp vec3 getNormalizedViewportCoord() {
highp vec2 scale = frameUniforms.logicalViewportScale;
highp vec2 offset = frameUniforms.logicalViewportOffset;
highp vec2 logicalUv = shading_normalizedViewportCoord * scale + offset;
return vec3(logicalUv, gl_FragCoord.z);
}
#if defined(VARIANT_HAS_SHADOWING) && defined(VARIANT_HAS_DYNAMIC_LIGHTING)
highp vec4 getSpotLightSpacePosition(uint index, highp vec3 dir, highp float zLight) {
highp mat4 lightFromWorldMatrix = shadowUniforms.shadows[index].lightFromWorldMatrix;
float bias = shadowUniforms.shadows[index].normalBias * zLight;
return computeLightSpacePosition(getWorldPosition(), getWorldNormalVector(),
dir, bias, lightFromWorldMatrix);
}
#endif
#if defined(MATERIAL_HAS_DOUBLE_SIDED_CAPABILITY)
bool isDoubleSided() {
return materialParams._doubleSided;
}
#endif
#if defined(VARIANT_HAS_SHADOWING) && defined(VARIANT_HAS_DIRECTIONAL_LIGHTING)
uint getShadowCascade() {
highp float z = mulMat4x4Float3(getViewFromWorldMatrix(), getWorldPosition()).z;
uvec4 greaterZ = uvec4(greaterThan(frameUniforms.cascadeSplits, vec4(z)));
uint cascadeCount = frameUniforms.cascades & 0xFu;
return clamp(greaterZ.x + greaterZ.y + greaterZ.z + greaterZ.w, 0u, cascadeCount - 1u);
}
highp vec4 getCascadeLightSpacePosition(uint cascade) {
if (cascade == 0u) {
return vertex_lightSpacePosition;
}
return computeLightSpacePosition(getWorldPosition(), getWorldNormalVector(),
frameUniforms.lightDirection,
shadowUniforms.shadows[cascade].normalBias,
shadowUniforms.shadows[cascade].lightFromWorldMatrix);
}
#endif
