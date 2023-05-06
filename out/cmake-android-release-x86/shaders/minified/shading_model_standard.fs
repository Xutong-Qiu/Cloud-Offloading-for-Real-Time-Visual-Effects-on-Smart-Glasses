#if defined(MATERIAL_HAS_SHEEN_COLOR)
vec3 sheenLobe(const PixelParams pixel, float NoV, float NoL, float NoH) {
float D = distributionCloth(pixel.sheenRoughness, NoH);
float V = visibilityCloth(NoV, NoL);
return (D * V) * pixel.sheenColor;
}
#endif
#if defined(MATERIAL_HAS_CLEAR_COAT)
float clearCoatLobe(const PixelParams pixel, const vec3 h, float NoH, float LoH, out float Fcc) {
#if defined(MATERIAL_HAS_NORMAL) || defined(MATERIAL_HAS_CLEAR_COAT_NORMAL)
float clearCoatNoH = saturate(dot(shading_clearCoatNormal, h));
#else
float clearCoatNoH = NoH;
#endif
float D = distributionClearCoat(pixel.clearCoatRoughness, clearCoatNoH, h);
float V = visibilityClearCoat(LoH);
float F = F_Schlick(0.04, 1.0, LoH) * pixel.clearCoat;
Fcc = F;
return D * V * F;
}
#endif
#if defined(MATERIAL_HAS_ANISOTROPY)
vec3 anisotropicLobe(const PixelParams pixel, const Light light, const vec3 h,
float NoV, float NoL, float NoH, float LoH) {
vec3 l = light.l;
vec3 t = pixel.anisotropicT;
vec3 b = pixel.anisotropicB;
vec3 v = shading_view;
float ToV = dot(t, v);
float BoV = dot(b, v);
float ToL = dot(t, l);
float BoL = dot(b, l);
float ToH = dot(t, h);
float BoH = dot(b, h);
float at = max(pixel.roughness * (1.0 + pixel.anisotropy), MIN_ROUGHNESS);
float ab = max(pixel.roughness * (1.0 - pixel.anisotropy), MIN_ROUGHNESS);
float D = distributionAnisotropic(at, ab, ToH, BoH, NoH);
float V = visibilityAnisotropic(pixel.roughness, at, ab, ToV, BoV, ToL, BoL, NoV, NoL);
vec3  F = fresnel(pixel.f0, LoH);
return (D * V) * F;
}
#endif
vec3 isotropicLobe(const PixelParams pixel, const Light light, const vec3 h,
float NoV, float NoL, float NoH, float LoH) {
float D = distribution(pixel.roughness, NoH, h);
float V = visibility(pixel.roughness, NoV, NoL);
vec3  F = fresnel(pixel.f0, LoH);
return (D * V) * F;
}
vec3 specularLobe(const PixelParams pixel, const Light light, const vec3 h,
float NoV, float NoL, float NoH, float LoH) {
#if defined(MATERIAL_HAS_ANISOTROPY)
return anisotropicLobe(pixel, light, h, NoV, NoL, NoH, LoH);
#else
return isotropicLobe(pixel, light, h, NoV, NoL, NoH, LoH);
#endif
}
vec3 diffuseLobe(const PixelParams pixel, float NoV, float NoL, float LoH) {
return pixel.diffuseColor * diffuse(pixel.roughness, NoV, NoL, LoH);
}
vec3 surfaceShading(const PixelParams pixel, const Light light, float occlusion) {
vec3 h = normalize(shading_view + light.l);
float NoV = shading_NoV;
float NoL = saturate(light.NoL);
float NoH = saturate(dot(shading_normal, h));
float LoH = saturate(dot(light.l, h));
vec3 Fr = specularLobe(pixel, light, h, NoV, NoL, NoH, LoH);
vec3 Fd = diffuseLobe(pixel, NoV, NoL, LoH);
#if defined(MATERIAL_HAS_REFRACTION)
Fd *= (1.0 - pixel.transmission);
#endif
vec3 color = Fd + Fr * pixel.energyCompensation;
#if defined(MATERIAL_HAS_SHEEN_COLOR)
color *= pixel.sheenScaling;
color += sheenLobe(pixel, NoV, NoL, NoH);
#endif
#if defined(MATERIAL_HAS_CLEAR_COAT)
float Fcc;
float clearCoat = clearCoatLobe(pixel, h, NoH, LoH, Fcc);
float attenuation = 1.0 - Fcc;
#if defined(MATERIAL_HAS_NORMAL) || defined(MATERIAL_HAS_CLEAR_COAT_NORMAL)
color *= attenuation * NoL;
float clearCoatNoL = saturate(dot(shading_clearCoatNormal, light.l));
color += clearCoat * clearCoatNoL;
return (color * light.colorIntensity.rgb) *
(light.colorIntensity.w * light.attenuation * occlusion);
#else
color *= attenuation;
color += clearCoat;
#endif
#endif
return (color * light.colorIntensity.rgb) *
(light.colorIntensity.w * light.attenuation * NoL * occlusion);
}
