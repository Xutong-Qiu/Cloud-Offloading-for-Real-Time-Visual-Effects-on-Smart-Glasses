struct Light {
vec4 colorIntensity;
vec3 l;
float attenuation;
highp vec3 worldPosition;
float NoL;
highp vec3 direction;
float zLight;
bool castsShadows;
bool contactShadows;
uint type;
uint shadowIndex;
uint channels;
};
struct PixelParams {
vec3  diffuseColor;
float perceptualRoughness;
float perceptualRoughnessUnclamped;
vec3  f0;
float roughness;
vec3  dfg;
vec3  energyCompensation;
#if defined(MATERIAL_HAS_CLEAR_COAT)
float clearCoat;
float clearCoatPerceptualRoughness;
float clearCoatRoughness;
#endif
#if defined(MATERIAL_HAS_SHEEN_COLOR)
vec3  sheenColor;
#if !defined(SHADING_MODEL_CLOTH)
float sheenRoughness;
float sheenPerceptualRoughness;
float sheenScaling;
float sheenDFG;
#endif
#endif
#if defined(MATERIAL_HAS_ANISOTROPY)
vec3  anisotropicT;
vec3  anisotropicB;
float anisotropy;
#endif
#if defined(SHADING_MODEL_SUBSURFACE) || defined(MATERIAL_HAS_REFRACTION)
float thickness;
#endif
#if defined(SHADING_MODEL_SUBSURFACE)
vec3  subsurfaceColor;
float subsurfacePower;
#endif
#if defined(SHADING_MODEL_CLOTH) && defined(MATERIAL_HAS_SUBSURFACE_COLOR)
vec3  subsurfaceColor;
#endif
#if defined(MATERIAL_HAS_REFRACTION)
float etaRI;
float etaIR;
float transmission;
float uThickness;
vec3  absorption;
#endif
};
float computeMicroShadowing(float NoL, float visibility) {
float aperture = inversesqrt(1.0 - min(visibility, 0.9999));
float microShadow = saturate(NoL * aperture);
return microShadow * microShadow;
}
vec3 getReflectedVector(const PixelParams pixel, const vec3 v, const vec3 n) {
#if defined(MATERIAL_HAS_ANISOTROPY)
vec3  anisotropyDirection = pixel.anisotropy >= 0.0 ? pixel.anisotropicB : pixel.anisotropicT;
vec3  anisotropicTangent  = cross(anisotropyDirection, v);
vec3  anisotropicNormal   = cross(anisotropicTangent, anisotropyDirection);
float bendFactor          = abs(pixel.anisotropy) * saturate(5.0 * pixel.perceptualRoughness);
vec3  bentNormal          = normalize(mix(n, anisotropicNormal, bendFactor));
vec3 r = reflect(-v, bentNormal);
#else
vec3 r = reflect(-v, n);
#endif
return r;
}
void getAnisotropyPixelParams(const MaterialInputs material, inout PixelParams pixel) {
#if defined(MATERIAL_HAS_ANISOTROPY)
vec3 direction = material.anisotropyDirection;
pixel.anisotropy = material.anisotropy;
pixel.anisotropicT = normalize(shading_tangentToWorld * direction);
pixel.anisotropicB = normalize(cross(getWorldGeometricNormalVector(), pixel.anisotropicT));
#endif
}
