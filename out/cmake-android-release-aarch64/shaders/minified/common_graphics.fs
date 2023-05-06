
float luminance(const vec3 linear) {
return dot(linear, vec3(0.2126, 0.7152, 0.0722));
}
float computePreExposedIntensity(const highp float intensity, const highp float exposure) {
return intensity * exposure;
}
void unpremultiply(inout vec4 color) {
color.rgb /= max(color.a, FLT_EPS);
}
vec3 ycbcrToRgb(float luminance, vec2 cbcr) {
const mat4 ycbcrToRgbTransform = mat4(
1.0000,  1.0000,  1.0000,  0.0000,
0.0000, -0.3441,  1.7720,  0.0000,
1.4020, -0.7141,  0.0000,  0.0000,
-0.7010,  0.5291, -0.8860,  1.0000
);
return (ycbcrToRgbTransform * vec4(luminance, cbcr, 1.0)).rgb;
}
vec3 Inverse_Tonemap_Filmic(const vec3 x) {
return (0.03 - 0.59 * x - sqrt(0.0009 + 1.3702 * x - 1.0127 * x * x)) / (-5.02 + 4.86 * x);
}
vec3 inverseTonemapSRGB(vec3 color) {
color = clamp(color, 0.0, 1.0);
return Inverse_Tonemap_Filmic(pow(color, vec3(2.2)));
}
vec3 inverseTonemap(vec3 linear) {
return Inverse_Tonemap_Filmic(clamp(linear, 0.0, 1.0));
}
vec3 decodeRGBM(vec4 c) {
c.rgb *= (c.a * 16.0);
return c.rgb * c.rgb;
}
highp vec2 getFragCoord(const highp vec2 resolution) {
#if defined(TARGET_METAL_ENVIRONMENT) || defined(TARGET_VULKAN_ENVIRONMENT)
return vec2(gl_FragCoord.x, resolution.y - gl_FragCoord.y);
#else
return gl_FragCoord.xy;
#endif
}
vec3 heatmap(float v) {
vec3 r = v * 2.1 - vec3(1.8, 1.14, 0.3);
return 1.0 - r * r;
}
