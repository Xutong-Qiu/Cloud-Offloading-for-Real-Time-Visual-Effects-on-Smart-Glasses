
void computeShadingParams() {
#if defined(HAS_ATTRIBUTE_TANGENTS)
highp vec3 n = vertex_worldNormal;
#if defined(MATERIAL_NEEDS_TBN)
highp vec3 t = vertex_worldTangent.xyz;
highp vec3 b = cross(n, t) * sign(vertex_worldTangent.w);
#endif
#if defined(MATERIAL_HAS_DOUBLE_SIDED_CAPABILITY)
if (isDoubleSided()) {
n = gl_FrontFacing ? n : -n;
#if defined(MATERIAL_NEEDS_TBN)
t = gl_FrontFacing ? t : -t;
b = gl_FrontFacing ? b : -b;
#endif
}
#endif
shading_geometricNormal = normalize(n);
#if defined(MATERIAL_NEEDS_TBN)
shading_tangentToWorld = mat3(t, b, n);
#endif
#endif
shading_position = vertex_worldPosition.xyz;
highp vec3 sv = isPerspectiveProjection() ?
(frameUniforms.worldFromViewMatrix[3].xyz - shading_position) :
frameUniforms.worldFromViewMatrix[2].xyz;
shading_view = normalize(sv);
shading_normalizedViewportCoord = vertex_position.xy * (0.5 / vertex_position.w) + 0.5;
}
void prepareMaterial(const MaterialInputs material) {
#if defined(HAS_ATTRIBUTE_TANGENTS)
#if defined(MATERIAL_HAS_NORMAL)
shading_normal = normalize(shading_tangentToWorld * material.normal);
#else
shading_normal = getWorldGeometricNormalVector();
#endif
shading_NoV = clampNoV(dot(shading_normal, shading_view));
shading_reflected = reflect(-shading_view, shading_normal);
#if defined(MATERIAL_HAS_BENT_NORMAL)
shading_bentNormal = normalize(shading_tangentToWorld * material.bentNormal);
#endif
#if defined(MATERIAL_HAS_CLEAR_COAT)
#if defined(MATERIAL_HAS_CLEAR_COAT_NORMAL)
shading_clearCoatNormal = normalize(shading_tangentToWorld * material.clearCoatNormal);
#else
shading_clearCoatNormal = getWorldGeometricNormalVector();
#endif
#endif
#endif
}
