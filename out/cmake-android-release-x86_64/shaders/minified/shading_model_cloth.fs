
vec3 surfaceShading(const PixelParams pixel, const Light light, float occlusion) {
vec3 h = normalize(shading_view + light.l);
float NoL = light.NoL;
float NoH = saturate(dot(shading_normal, h));
float LoH = saturate(dot(light.l, h));
float D = distributionCloth(pixel.roughness, NoH);
float V = visibilityCloth(shading_NoV, NoL);
vec3  F = pixel.f0;
vec3 Fr = (D * V) * F;
float diffuse = diffuse(pixel.roughness, shading_NoV, NoL, LoH);
#if defined(MATERIAL_HAS_SUBSURFACE_COLOR)
diffuse *= Fd_Wrap(dot(shading_normal, light.l), 0.5);
#endif
vec3 Fd = diffuse * pixel.diffuseColor;
#if defined(MATERIAL_HAS_SUBSURFACE_COLOR)
Fd *= saturate(pixel.subsurfaceColor + NoL);
vec3 color = Fd + Fr * NoL;
color *= light.colorIntensity.rgb * (light.colorIntensity.w * light.attenuation * occlusion);
#else
vec3 color = Fd + Fr;
color *= light.colorIntensity.rgb * (light.colorIntensity.w * light.attenuation * NoL * occlusion);
#endif
return color;
}
