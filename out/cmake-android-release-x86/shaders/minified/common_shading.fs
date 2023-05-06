
highp mat3  shading_tangentToWorld;
highp vec3  shading_position;
vec3  shading_view;
vec3  shading_normal;
vec3  shading_geometricNormal;
vec3  shading_reflected;
float shading_NoV;
#if defined(MATERIAL_HAS_BENT_NORMAL)
vec3  shading_bentNormal;
#endif
#if defined(MATERIAL_HAS_CLEAR_COAT)
vec3  shading_clearCoatNormal;
#endif
highp vec2 shading_normalizedViewportCoord;
