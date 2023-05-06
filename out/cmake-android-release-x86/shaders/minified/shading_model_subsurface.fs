
vec3 surfaceShading(const PixelParams pixel, const Light light, float occlusion) {
vec3 h = normalize(shading_view + light.l);
float NoL = light.NoL;
float NoH = saturate(dot(shading_normal, h));
float LoH = saturate(dot(light.l, h));
vec3 Fr = vec3(0.0);
if (NoL > 0.0) {
float D = distribution(pixel.roughness, NoH, h);
float V = visibility(pixel.roughness, shading_NoV, NoL);
vec3  F = fresnel(pixel.f0, LoH);
Fr = (D * V) * F * pixel.energyCompensation;
}
vec3 Fd = pixel.diffuseColor * diffuse(pixel.roughness, shading_NoV, NoL, LoH);
vec3 color = (Fd + Fr) * (NoL * occlusion);
float scatterVoH = saturate(dot(shading_view, -light.l));
float forwardScatter = exp2(scatterVoH * pixel.subsurfacePower - pixel.subsurfacePower);
float backScatter = saturate(NoL * pixel.thickness + (1.0 - pixel.thickness)) * 0.5;
float subsurface = mix(backScatter, 1.0, forwardScatter) * (1.0 - pixel.thickness);
color += pixel.subsurfaceColor * (subsurface * Fd_Lambert());
return (color * light.colorIntensity.rgb) * (light.colorIntensity.w * light.attenuation);
}
