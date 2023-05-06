
vec4 evaluateMaterial(const MaterialInputs material) {
#if defined(MATERIAL_HAS_REFLECTIONS)
PixelParams pixel;
getAnisotropyPixelParams(material, pixel);
vec4 color = vec4(0.0);
if (frameUniforms.ssrDistance > 0.0) {
vec3 r = getReflectedVector(pixel, shading_view, shading_normal);
color = evaluateScreenSpaceReflections(r);
}
#else
const vec4 color = vec4(0.0);
#endif
return color;
}
