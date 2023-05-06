
#define SPECULAR_AO_OFF             0
#define SPECULAR_AO_SIMPLE          1
#define SPECULAR_AO_BENT_NORMALS    2
float unpack(vec2 depth) {
return (depth.x * (256.0 / 257.0) + depth.y * (1.0 / 257.0));
}
struct SSAOInterpolationCache {
highp vec4 weights;
#if defined(BLEND_MODE_OPAQUE) || defined(BLEND_MODE_MASKED) || defined(MATERIAL_HAS_REFLECTIONS)
highp vec2 uv;
#endif
};
float evaluateSSAO(inout SSAOInterpolationCache cache) {
#if defined(BLEND_MODE_OPAQUE) || defined(BLEND_MODE_MASKED)
if (frameUniforms.aoSamplingQualityAndEdgeDistance < 0.0) {
return 1.0;
}
if (frameUniforms.aoSamplingQualityAndEdgeDistance > 0.0) {
highp vec2 size = vec2(textureSize(light_ssao, 0));
#if defined(FILAMENT_HAS_FEATURE_TEXTURE_GATHER)
vec4 ao = textureGather(light_ssao, vec3(cache.uv, 0.0), 0);
vec4 dg = textureGather(light_ssao, vec3(cache.uv, 0.0), 1);
vec4 db = textureGather(light_ssao, vec3(cache.uv, 0.0), 2);
#else
vec3 s01 = textureLodOffset(light_ssao, vec3(cache.uv, 0.0), 0.0, ivec2(0, 1)).rgb;
vec3 s11 = textureLodOffset(light_ssao, vec3(cache.uv, 0.0), 0.0, ivec2(1, 1)).rgb;
vec3 s10 = textureLodOffset(light_ssao, vec3(cache.uv, 0.0), 0.0, ivec2(1, 0)).rgb;
vec3 s00 = textureLodOffset(light_ssao, vec3(cache.uv, 0.0), 0.0, ivec2(0, 0)).rgb;
vec4 ao = vec4(s01.r, s11.r, s10.r, s00.r);
vec4 dg = vec4(s01.g, s11.g, s10.g, s00.g);
vec4 db = vec4(s01.b, s11.b, s10.b, s00.b);
#endif
vec4 depths;
depths.x = unpack(vec2(dg.x, db.x));
depths.y = unpack(vec2(dg.y, db.y));
depths.z = unpack(vec2(dg.z, db.z));
depths.w = unpack(vec2(dg.w, db.w));
depths *= -frameUniforms.cameraFar;
vec2 f = fract(cache.uv * size - 0.5);
vec4 b;
b.x = (1.0 - f.x) * f.y;
b.y = f.x * f.y;
b.z = f.x * (1.0 - f.y);
b.w = (1.0 - f.x) * (1.0 - f.y);
highp mat4 m = getViewFromWorldMatrix();
highp float d = dot(vec3(m[0].z, m[1].z, m[2].z), shading_position) + m[3].z;
highp vec4 w = (vec4(d) - depths) * frameUniforms.aoSamplingQualityAndEdgeDistance;
w = max(vec4(MEDIUMP_FLT_MIN), 1.0 - w * w) * b;
cache.weights = w / (w.x + w.y + w.z + w.w);
return dot(ao, cache.weights);
} else {
return textureLod(light_ssao, vec3(cache.uv, 0.0), 0.0).r;
}
#else
return 1.0;
#endif
}
float SpecularAO_Lagarde(float NoV, float visibility, float roughness) {
return saturate(pow(NoV + visibility, exp2(-16.0 * roughness - 1.0)) - 1.0 + visibility);
}
float sphericalCapsIntersection(float cosCap1, float cosCap2, float cosDistance) {
float r1 = acosFastPositive(cosCap1);
float r2 = acosFastPositive(cosCap2);
float d  = acosFast(cosDistance);
if (min(r1, r2) <= max(r1, r2) - d) {
return 1.0 - max(cosCap1, cosCap2);
} else if (r1 + r2 <= d) {
return 0.0;
}
float delta = abs(r1 - r2);
float x = 1.0 - saturate((d - delta) / max(r1 + r2 - delta, 1e-4));
float area = sq(x) * (-2.0 * x + 3.0);
return area * (1.0 - max(cosCap1, cosCap2));
}
float SpecularAO_Cones(vec3 bentNormal, float visibility, float roughness) {
float cosAv = sqrt(1.0 - visibility);
float cosAs = exp2(-3.321928 * sq(roughness));
float cosB  = dot(bentNormal, shading_reflected);
float ao = sphericalCapsIntersection(cosAv, cosAs, cosB) / (1.0 - cosAs);
return mix(1.0, ao, smoothstep(0.01, 0.09, roughness));
}
vec3 unpackBentNormal(vec3 bn) {
return bn * 2.0 - 1.0;
}
float specularAO(float NoV, float visibility, float roughness, const in SSAOInterpolationCache cache) {
float specularAO = 1.0;
#if defined(BLEND_MODE_OPAQUE) || defined(BLEND_MODE_MASKED)
#if SPECULAR_AMBIENT_OCCLUSION == SPECULAR_AO_SIMPLE
specularAO = SpecularAO_Lagarde(NoV, visibility, roughness);
#elif SPECULAR_AMBIENT_OCCLUSION == SPECULAR_AO_BENT_NORMALS
#   if defined(MATERIAL_HAS_BENT_NORMAL)
specularAO = SpecularAO_Cones(shading_bentNormal, visibility, roughness);
#   else
specularAO = SpecularAO_Cones(shading_normal, visibility, roughness);
#   endif
#endif
if (frameUniforms.aoBentNormals > 0.0) {
vec3 bn;
if (frameUniforms.aoSamplingQualityAndEdgeDistance > 0.0) {
#if defined(FILAMENT_HAS_FEATURE_TEXTURE_GATHER)
vec4 bnr = textureGather(light_ssao, vec3(cache.uv, 1.0), 0);
vec4 bng = textureGather(light_ssao, vec3(cache.uv, 1.0), 1);
vec4 bnb = textureGather(light_ssao, vec3(cache.uv, 1.0), 2);
#else
vec3 s01 = textureLodOffset(light_ssao, vec3(cache.uv, 1.0), 0.0, ivec2(0, 1)).rgb;
vec3 s11 = textureLodOffset(light_ssao, vec3(cache.uv, 1.0), 0.0, ivec2(1, 1)).rgb;
vec3 s10 = textureLodOffset(light_ssao, vec3(cache.uv, 1.0), 0.0, ivec2(1, 0)).rgb;
vec3 s00 = textureLodOffset(light_ssao, vec3(cache.uv, 1.0), 0.0, ivec2(0, 0)).rgb;
vec4 bnr = vec4(s01.r, s11.r, s10.r, s00.r);
vec4 bng = vec4(s01.g, s11.g, s10.g, s00.g);
vec4 bnb = vec4(s01.b, s11.b, s10.b, s00.b);
#endif
bn.r = dot(bnr, cache.weights);
bn.g = dot(bng, cache.weights);
bn.b = dot(bnb, cache.weights);
} else {
bn = textureLod(light_ssao, vec3(cache.uv, 1.0), 0.0).xyz;
}
bn = unpackBentNormal(bn);
bn = normalize(bn);
float ssSpecularAO = SpecularAO_Cones(bn, visibility, roughness);
specularAO = min(specularAO, ssSpecularAO);
}
#endif
return specularAO;
}
#if MULTI_BOUNCE_AMBIENT_OCCLUSION == 1
vec3 gtaoMultiBounce(float visibility, const vec3 albedo) {
vec3 a =  2.0404 * albedo - 0.3324;
vec3 b = -4.7951 * albedo + 0.6417;
vec3 c =  2.7552 * albedo + 0.6903;
return max(vec3(visibility), ((visibility * a + b) * visibility + c) * visibility);
}
#endif
void multiBounceAO(float visibility, const vec3 albedo, inout vec3 color) {
#if MULTI_BOUNCE_AMBIENT_OCCLUSION == 1
color *= gtaoMultiBounce(visibility, albedo);
#endif
}
void multiBounceSpecularAO(float visibility, const vec3 albedo, inout vec3 color) {
#if MULTI_BOUNCE_AMBIENT_OCCLUSION == 1 && SPECULAR_AMBIENT_OCCLUSION != SPECULAR_AO_OFF
color *= gtaoMultiBounce(visibility, albedo);
#endif
}
float singleBounceAO(float visibility) {
#if MULTI_BOUNCE_AMBIENT_OCCLUSION == 1
return 1.0;
#else
return visibility;
#endif
}
