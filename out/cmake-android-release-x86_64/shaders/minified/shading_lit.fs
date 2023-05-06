
#if defined(BLEND_MODE_MASKED)
float computeMaskedAlpha(float a) {
return (a - getMaskThreshold()) / max(fwidth(a), 1e-3) + 0.5;
}
float computeDiffuseAlpha(float a) {
return (frameUniforms.needsAlphaChannel == 1.0) ? 1.0 : a;
}
void applyAlphaMask(inout vec4 baseColor) {
baseColor.a = computeMaskedAlpha(baseColor.a);
if (baseColor.a <= 0.0) {
discard;
}
}
#else
float computeDiffuseAlpha(float a) {
#if defined(BLEND_MODE_TRANSPARENT) || defined(BLEND_MODE_FADE)
return a;
#else
return 1.0;
#endif
}
void applyAlphaMask(inout vec4 baseColor) {}
#endif
#if defined(GEOMETRIC_SPECULAR_AA)
float normalFiltering(float perceptualRoughness, const vec3 worldNormal) {
vec3 du = dFdx(worldNormal);
vec3 dv = dFdy(worldNormal);
float variance = materialParams._specularAntiAliasingVariance * (dot(du, du) + dot(dv, dv));
float roughness = perceptualRoughnessToRoughness(perceptualRoughness);
float kernelRoughness = min(2.0 * variance, materialParams._specularAntiAliasingThreshold);
float squareRoughness = saturate(roughness * roughness + kernelRoughness);
return roughnessToPerceptualRoughness(sqrt(squareRoughness));
}
#endif
void getCommonPixelParams(const MaterialInputs material, inout PixelParams pixel) {
vec4 baseColor = material.baseColor;
applyAlphaMask(baseColor);
#if defined(BLEND_MODE_FADE) && !defined(SHADING_MODEL_UNLIT)
unpremultiply(baseColor);
#endif
#if defined(SHADING_MODEL_SPECULAR_GLOSSINESS)
vec3 specularColor = material.specularColor;
float metallic = computeMetallicFromSpecularColor(specularColor);
pixel.diffuseColor = computeDiffuseColor(baseColor, metallic);
pixel.f0 = specularColor;
#elif !defined(SHADING_MODEL_CLOTH)
pixel.diffuseColor = computeDiffuseColor(baseColor, material.metallic);
#if !defined(SHADING_MODEL_SUBSURFACE) && (!defined(MATERIAL_HAS_REFLECTANCE) && defined(MATERIAL_HAS_IOR))
float reflectance = iorToF0(max(1.0, material.ior), 1.0);
#else
float reflectance = computeDielectricF0(material.reflectance);
#endif
pixel.f0 = computeF0(baseColor, material.metallic, reflectance);
#else
pixel.diffuseColor = baseColor.rgb;
pixel.f0 = material.sheenColor;
#if defined(MATERIAL_HAS_SUBSURFACE_COLOR)
pixel.subsurfaceColor = material.subsurfaceColor;
#endif
#endif
#if !defined(SHADING_MODEL_CLOTH) && !defined(SHADING_MODEL_SUBSURFACE)
#if defined(MATERIAL_HAS_REFRACTION)
const float airIor = 1.0;
#if !defined(MATERIAL_HAS_IOR)
float materialor = f0ToIor(pixel.f0.g);
#else
float materialor = max(1.0, material.ior);
#endif
pixel.etaIR = airIor / materialor;
pixel.etaRI = materialor / airIor;
#if defined(MATERIAL_HAS_TRANSMISSION)
pixel.transmission = saturate(material.transmission);
#else
pixel.transmission = 1.0;
#endif
#if defined(MATERIAL_HAS_ABSORPTION)
#if defined(MATERIAL_HAS_THICKNESS) || defined(MATERIAL_HAS_MICRO_THICKNESS)
pixel.absorption = max(vec3(0.0), material.absorption);
#else
pixel.absorption = saturate(material.absorption);
#endif
#else
pixel.absorption = vec3(0.0);
#endif
#if defined(MATERIAL_HAS_THICKNESS)
pixel.thickness = max(0.0, material.thickness);
#endif
#if defined(MATERIAL_HAS_MICRO_THICKNESS) && (REFRACTION_TYPE == REFRACTION_TYPE_THIN)
pixel.uThickness = max(0.0, material.microThickness);
#else
pixel.uThickness = 0.0;
#endif
#endif
#endif
}
void getSheenPixelParams(const MaterialInputs material, inout PixelParams pixel) {
#if defined(MATERIAL_HAS_SHEEN_COLOR) && !defined(SHADING_MODEL_CLOTH) && !defined(SHADING_MODEL_SUBSURFACE)
pixel.sheenColor = material.sheenColor;
float sheenPerceptualRoughness = material.sheenRoughness;
sheenPerceptualRoughness = clamp(sheenPerceptualRoughness, MIN_PERCEPTUAL_ROUGHNESS, 1.0);
#if defined(GEOMETRIC_SPECULAR_AA)
sheenPerceptualRoughness =
normalFiltering(sheenPerceptualRoughness, getWorldGeometricNormalVector());
#endif
pixel.sheenPerceptualRoughness = sheenPerceptualRoughness;
pixel.sheenRoughness = perceptualRoughnessToRoughness(sheenPerceptualRoughness);
#endif
}
void getClearCoatPixelParams(const MaterialInputs material, inout PixelParams pixel) {
#if defined(MATERIAL_HAS_CLEAR_COAT)
pixel.clearCoat = material.clearCoat;
float clearCoatPerceptualRoughness = material.clearCoatRoughness;
clearCoatPerceptualRoughness =
clamp(clearCoatPerceptualRoughness, MIN_PERCEPTUAL_ROUGHNESS, 1.0);
#if defined(GEOMETRIC_SPECULAR_AA)
clearCoatPerceptualRoughness =
normalFiltering(clearCoatPerceptualRoughness, getWorldGeometricNormalVector());
#endif
pixel.clearCoatPerceptualRoughness = clearCoatPerceptualRoughness;
pixel.clearCoatRoughness = perceptualRoughnessToRoughness(clearCoatPerceptualRoughness);
#if defined(CLEAR_COAT_IOR_CHANGE)
pixel.f0 = mix(pixel.f0, f0ClearCoatToSurface(pixel.f0), pixel.clearCoat);
#endif
#endif
}
void getRoughnessPixelParams(const MaterialInputs material, inout PixelParams pixel) {
#if defined(SHADING_MODEL_SPECULAR_GLOSSINESS)
float perceptualRoughness = computeRoughnessFromGlossiness(material.glossiness);
#else
float perceptualRoughness = material.roughness;
#endif
pixel.perceptualRoughnessUnclamped = perceptualRoughness;
#if defined(GEOMETRIC_SPECULAR_AA)
perceptualRoughness = normalFiltering(perceptualRoughness, getWorldGeometricNormalVector());
#endif
#if defined(MATERIAL_HAS_CLEAR_COAT) && defined(MATERIAL_HAS_CLEAR_COAT_ROUGHNESS)
float basePerceptualRoughness = max(perceptualRoughness, pixel.clearCoatPerceptualRoughness);
perceptualRoughness = mix(perceptualRoughness, basePerceptualRoughness, pixel.clearCoat);
#endif
pixel.perceptualRoughness = clamp(perceptualRoughness, MIN_PERCEPTUAL_ROUGHNESS, 1.0);
pixel.roughness = perceptualRoughnessToRoughness(pixel.perceptualRoughness);
}
void getSubsurfacePixelParams(const MaterialInputs material, inout PixelParams pixel) {
#if defined(SHADING_MODEL_SUBSURFACE)
pixel.subsurfacePower = material.subsurfacePower;
pixel.subsurfaceColor = material.subsurfaceColor;
pixel.thickness = saturate(material.thickness);
#endif
}
void getEnergyCompensationPixelParams(inout PixelParams pixel) {
pixel.dfg = prefilteredDFG(pixel.perceptualRoughness, shading_NoV);
#if !defined(SHADING_MODEL_CLOTH)
pixel.energyCompensation = 1.0 + pixel.f0 * (1.0 / pixel.dfg.y - 1.0);
#else
pixel.energyCompensation = vec3(1.0);
#endif
#if !defined(SHADING_MODEL_CLOTH)
#if defined(MATERIAL_HAS_SHEEN_COLOR)
pixel.sheenDFG = prefilteredDFG(pixel.sheenPerceptualRoughness, shading_NoV).z;
pixel.sheenScaling = 1.0 - max3(pixel.sheenColor) * pixel.sheenDFG;
#endif
#endif
}
void getPixelParams(const MaterialInputs material, out PixelParams pixel) {
getCommonPixelParams(material, pixel);
getSheenPixelParams(material, pixel);
getClearCoatPixelParams(material, pixel);
getRoughnessPixelParams(material, pixel);
getSubsurfacePixelParams(material, pixel);
getAnisotropyPixelParams(material, pixel);
getEnergyCompensationPixelParams(pixel);
}
vec4 evaluateLights(const MaterialInputs material) {
PixelParams pixel;
getPixelParams(material, pixel);
vec3 color = vec3(0.0);
evaluateIBL(material, pixel, color);
#if defined(VARIANT_HAS_DIRECTIONAL_LIGHTING)
evaluateDirectionalLight(material, pixel, color);
#endif
#if defined(VARIANT_HAS_DYNAMIC_LIGHTING)
evaluatePunctualLights(material, pixel, color);
#endif
#if defined(BLEND_MODE_FADE) && !defined(SHADING_MODEL_UNLIT)
color *= material.baseColor.a;
#endif
return vec4(color, computeDiffuseAlpha(material.baseColor.a));
}
void addEmissive(const MaterialInputs material, inout vec4 color) {
#if defined(MATERIAL_HAS_EMISSIVE)
highp vec4 emissive = material.emissive;
highp float attenuation = mix(1.0, getExposure(), emissive.w);
color.rgb += emissive.rgb * (attenuation * color.a);
#endif
}
vec4 evaluateMaterial(const MaterialInputs material) {
vec4 color = evaluateLights(material);
addEmissive(material, color);
return color;
}
