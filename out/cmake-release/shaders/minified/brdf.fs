
#define DIFFUSE_LAMBERT             0
#define DIFFUSE_BURLEY              1
#define SPECULAR_D_GGX              0
#define SPECULAR_D_GGX_ANISOTROPIC  0
#define SPECULAR_D_CHARLIE          0
#define SPECULAR_V_SMITH_GGX        0
#define SPECULAR_V_SMITH_GGX_FAST   1
#define SPECULAR_V_GGX_ANISOTROPIC  2
#define SPECULAR_V_KELEMEN          3
#define SPECULAR_V_NEUBELT          4
#define SPECULAR_F_SCHLICK          0
#define BRDF_DIFFUSE                DIFFUSE_LAMBERT
#if FILAMENT_QUALITY < FILAMENT_QUALITY_HIGH
#define BRDF_SPECULAR_D             SPECULAR_D_GGX
#define BRDF_SPECULAR_V             SPECULAR_V_SMITH_GGX_FAST
#define BRDF_SPECULAR_F             SPECULAR_F_SCHLICK
#else
#define BRDF_SPECULAR_D             SPECULAR_D_GGX
#define BRDF_SPECULAR_V             SPECULAR_V_SMITH_GGX
#define BRDF_SPECULAR_F             SPECULAR_F_SCHLICK
#endif
#define BRDF_CLEAR_COAT_D           SPECULAR_D_GGX
#define BRDF_CLEAR_COAT_V           SPECULAR_V_KELEMEN
#define BRDF_ANISOTROPIC_D          SPECULAR_D_GGX_ANISOTROPIC
#define BRDF_ANISOTROPIC_V          SPECULAR_V_GGX_ANISOTROPIC
#define BRDF_CLOTH_D                SPECULAR_D_CHARLIE
#define BRDF_CLOTH_V                SPECULAR_V_NEUBELT
float D_GGX(float roughness, float NoH, const vec3 h) {
#if defined(TARGET_MOBILE)
vec3 NxH = cross(shading_normal, h);
float oneMinusNoHSquared = dot(NxH, NxH);
#else
float oneMinusNoHSquared = 1.0 - NoH * NoH;
#endif
float a = NoH * roughness;
float k = roughness / (oneMinusNoHSquared + a * a);
float d = k * k * (1.0 / PI);
return saturateMediump(d);
}
float D_GGX_Anisotropic(float at, float ab, float ToH, float BoH, float NoH) {
float a2 = at * ab;
highp vec3 d = vec3(ab * ToH, at * BoH, a2 * NoH);
highp float d2 = dot(d, d);
float b2 = a2 / d2;
return a2 * b2 * b2 * (1.0 / PI);
}
float D_Charlie(float roughness, float NoH) {
float invAlpha  = 1.0 / roughness;
float cos2h = NoH * NoH;
float sin2h = max(1.0 - cos2h, 0.0078125);
return (2.0 + invAlpha) * pow(sin2h, invAlpha * 0.5) / (2.0 * PI);
}
float V_SmithGGXCorrelated(float roughness, float NoV, float NoL) {
float a2 = roughness * roughness;
float lambdaV = NoL * sqrt((NoV - a2 * NoV) * NoV + a2);
float lambdaL = NoV * sqrt((NoL - a2 * NoL) * NoL + a2);
float v = 0.5 / (lambdaV + lambdaL);
return saturateMediump(v);
}
float V_SmithGGXCorrelated_Fast(float roughness, float NoV, float NoL) {
float v = 0.5 / mix(2.0 * NoL * NoV, NoL + NoV, roughness);
return saturateMediump(v);
}
float V_SmithGGXCorrelated_Anisotropic(float at, float ab, float ToV, float BoV,
float ToL, float BoL, float NoV, float NoL) {
float lambdaV = NoL * length(vec3(at * ToV, ab * BoV, NoV));
float lambdaL = NoV * length(vec3(at * ToL, ab * BoL, NoL));
float v = 0.5 / (lambdaV + lambdaL);
return saturateMediump(v);
}
float V_Kelemen(float LoH) {
return saturateMediump(0.25 / (LoH * LoH));
}
float V_Neubelt(float NoV, float NoL) {
return saturateMediump(1.0 / (4.0 * (NoL + NoV - NoL * NoV)));
}
vec3 F_Schlick(const vec3 f0, float f90, float VoH) {
return f0 + (f90 - f0) * pow5(1.0 - VoH);
}
vec3 F_Schlick(const vec3 f0, float VoH) {
float f = pow(1.0 - VoH, 5.0);
return f + f0 * (1.0 - f);
}
float F_Schlick(float f0, float f90, float VoH) {
return f0 + (f90 - f0) * pow5(1.0 - VoH);
}
float distribution(float roughness, float NoH, const vec3 h) {
#if BRDF_SPECULAR_D == SPECULAR_D_GGX
return D_GGX(roughness, NoH, h);
#endif
}
float visibility(float roughness, float NoV, float NoL) {
#if BRDF_SPECULAR_V == SPECULAR_V_SMITH_GGX
return V_SmithGGXCorrelated(roughness, NoV, NoL);
#elif BRDF_SPECULAR_V == SPECULAR_V_SMITH_GGX_FAST
return V_SmithGGXCorrelated_Fast(roughness, NoV, NoL);
#endif
}
vec3 fresnel(const vec3 f0, float LoH) {
#if BRDF_SPECULAR_F == SPECULAR_F_SCHLICK
#if FILAMENT_QUALITY == FILAMENT_QUALITY_LOW
return F_Schlick(f0, LoH);
#else
float f90 = saturate(dot(f0, vec3(50.0 * 0.33)));
return F_Schlick(f0, f90, LoH);
#endif
#endif
}
float distributionAnisotropic(float at, float ab, float ToH, float BoH, float NoH) {
#if BRDF_ANISOTROPIC_D == SPECULAR_D_GGX_ANISOTROPIC
return D_GGX_Anisotropic(at, ab, ToH, BoH, NoH);
#endif
}
float visibilityAnisotropic(float roughness, float at, float ab,
float ToV, float BoV, float ToL, float BoL, float NoV, float NoL) {
#if BRDF_ANISOTROPIC_V == SPECULAR_V_SMITH_GGX
return V_SmithGGXCorrelated(roughness, NoV, NoL);
#elif BRDF_ANISOTROPIC_V == SPECULAR_V_GGX_ANISOTROPIC
return V_SmithGGXCorrelated_Anisotropic(at, ab, ToV, BoV, ToL, BoL, NoV, NoL);
#endif
}
float distributionClearCoat(float roughness, float NoH, const vec3 h) {
#if BRDF_CLEAR_COAT_D == SPECULAR_D_GGX
return D_GGX(roughness, NoH, h);
#endif
}
float visibilityClearCoat(float LoH) {
#if BRDF_CLEAR_COAT_V == SPECULAR_V_KELEMEN
return V_Kelemen(LoH);
#endif
}
float distributionCloth(float roughness, float NoH) {
#if BRDF_CLOTH_D == SPECULAR_D_CHARLIE
return D_Charlie(roughness, NoH);
#endif
}
float visibilityCloth(float NoV, float NoL) {
#if BRDF_CLOTH_V == SPECULAR_V_NEUBELT
return V_Neubelt(NoV, NoL);
#endif
}
float Fd_Lambert() {
return 1.0 / PI;
}
float Fd_Burley(float roughness, float NoV, float NoL, float LoH) {
float f90 = 0.5 + 2.0 * roughness * LoH * LoH;
float lightScatter = F_Schlick(1.0, f90, NoL);
float viewScatter  = F_Schlick(1.0, f90, NoV);
return lightScatter * viewScatter * (1.0 / PI);
}
float Fd_Wrap(float NoL, float w) {
return saturate((NoL + w) / sq(1.0 + w));
}
float diffuse(float roughness, float NoV, float NoL, float LoH) {
#if BRDF_DIFFUSE == DIFFUSE_LAMBERT
return Fd_Lambert();
#elif BRDF_DIFFUSE == DIFFUSE_BURLEY
return Fd_Burley(roughness, NoV, NoL, LoH);
#endif
}
